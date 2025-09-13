using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Threading.Tasks;
using CommonFW.Domain.Model.Payment;
using DAL;
using DAL.IService;
using DAL.Models;
using DAL.Models.Payoo;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using WebApp.Infrastructure.Configuration;

namespace WebApp.Controllers
{
    public class PayooController : AppBaseController
    {
        private readonly IPayooService payooService;
        private readonly ITicketService ticketService;
        private readonly ITicketOrderService ticketOrderService;
        private readonly IPaymentService paymentService;
        public PayooController(IPayooService payooService, ITicketService ticketService, ITicketOrderService ticketOrderService, IPaymentService paymentService)
        {
            this.payooService = payooService;
            this.ticketOrderService = ticketOrderService;
            this.ticketService = ticketService;
            this.paymentService = paymentService;
        }





        [HttpGet]
        public ResultModel CreatePaymentLink(long orderId)
        {
            var res = new ResultModel();



            // ===== repare data post =========================================
            var orderInfo = ticketService.GetTicketOrderbyId(orderId);
            string orderCode = DAL.Helper.GenMaDon(orderId);


            DateTime timePayment = DateTime.Now.AddMinutes(30);
            string paymentValidityTime = timePayment.ToString("yyyyMMddHHmmss");
            string orderXML = @"<shops><shop><username>SB_GAMAN</username><shop_id>12166</shop_id><shop_title>GAMAN</shop_title><shop_domain>gamanjsc.com</shop_domain><shop_back_url>https%3A%2F%2Fquanlyve.gamanjsc.com%2Fticketorder%2Frevieworderbycustomer</shop_back_url><order_no>"+ orderCode + "</order_no><order_cash_amount>"+ orderInfo.Total + "</order_cash_amount><order_description>GM"+ orderCode + "</order_description><notify_url>https%3A%2F%2Fgticketapi.gamanjsc.com%2Fpayoo%2Fapi%2Fpayment-result</notify_url><validity_time>" + paymentValidityTime + "</validity_time><mdd1>"+ orderId + "</mdd1><mdd2>LangbiangLand</mdd2><count_down>0</count_down><direct_return_time>5</direct_return_time><jsonresponse>true</jsonresponse></shop></shops>";

            string checkSumString = GenCheckSumvalue("MzZmYzM4MTE5NzcwYjgxZDNiMzZjNWU5NzQwMjhkNTg=", orderXML);
            var dataPost = new CreatePaymentLinkModel
            {
                data = orderXML,
                checksum = checkSumString,
                refer= "gamanjsc.com",
                payment_group = "qr-pay",
                method = "qr-pay",
                bank = "ABB",
                create_ref_qrbank_id = 1,
                qr_standard = 0,
                create_payment_code = 0
            };




            //================ post to Payoo===============================================================
            string apiAddress = AppSettingServices.Get.PayooSettings.CreatePaymentAddress;//config["General:RootFolder"];
            RestClient client = new RestClient(apiAddress);
            string bodyPost = Newtonsoft.Json.JsonConvert.SerializeObject(dataPost);
            var request = new RestRequest(string.Empty, Method.POST);
            request.AddParameter("application/json", bodyPost, RestSharp.ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            try
            {
                var okmen = client.Execute(request);

                if (okmen.Content != null)
                {
                    var contentResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseResultCreatePaymentLinkModel>(okmen.Content);
                    res = payooService.SaveResultCreatePaymentLink(orderId, bodyPost, contentResult, okmen.Content);
                }
                else
                {
                    res = payooService.SaveResultCreatePaymentLink(orderId, bodyPost, null, okmen.ErrorMessage);
                }



            }
            catch (Exception error)
            {
                res = payooService.SaveResultCreatePaymentLink(orderId, bodyPost, null, error.Message);
            }

            return res;
        }



        public string GenCheckSumvalue(string checkSumKey, string xmlOrer)
        {
            return SHA512(string.Format(@"{0}{1}", checkSumKey, xmlOrer));
        }


        public static string SHA512(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        private static void InitiateSSLTrust()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback =
                   new RemoteCertificateValidationCallback(
                        delegate
                        { return true; }
                    );
            }
            catch (Exception ex)
            {

            }
        }



        public IActionResult WaitingPayment()
        {
            var UserName = AuthenInfo().UserName;
            var res = payooService.GetOrderWaitingPayment(UserName);
            string imgQR = "~/img/qr-default.jpg";
            if (res!=null)
            {
                string madon = Helper.GenMaDon(res.OrderId);
                QRCodePaymentModel jsbody = new QRCodePaymentModel()
                {
                    accountNo = "6090201043889",
                    accountName = "Bảo Tàng - Thư Viện Tỉnh Bà Rịa - Vũng Tàu",
                    acqId = AppSettingServices.Get.GenerateQRCodeSettings.AcqId ?? "970405", // Agrribank
                    addInfo = "BTTV" + res.OrderId,
                    amount = res.Total.ToString(),
                    template = AppSettingServices.Get.GenerateQRCodeSettings.Template ?? "compact2",
                };
                imgQR = paymentService.GenerateQRCodePayment(jsbody);
            }
           
            ViewBag.ImgQRPayment = imgQR;


            //var qrInfo = new ResponseResultCreatePaymentLinkModel();
            //if (res != null)
            //{
            //    qrInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseResultCreatePaymentLinkModel>(res.ResponseJson);
            //    string fileFullPath = string.Format("~/QRCodeThanhToan/{0}.jpg", res.OrderId);
            //    if(qrInfo == null)
            //    {
            //        qrInfo = new ResponseResultCreatePaymentLinkModel();
            //        qrInfo.order = new OrderInfoReturnModel();
            //        qrInfo.order.qrbank_info = new QRBankInfoModel();
            //    }    
            //    qrInfo.order.qrbank_info.bank_name = "Ngân hàng Nông nghiệp và Phát triển Nông thôn Việt Nam";
            //    qrInfo.order.qrbank_info.qr_code_uri = fileFullPath;
            //}

            
            //ViewBag.QRPayoo = qrInfo;
            return View(res);
        }














    }
}