using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.IService;
using DAL.Models;
using DAL.Models.WebHookSePay;
using Microsoft.AspNetCore.Mvc;
using DAL;
using System.Text;
using WebApp.Infrastructure.Configuration;
using System.IO;
using QRCoder;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;
using DAL.Models.Zalo;
using DAL.Enum;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class SePayWebHookController : Controller
    {
        private ITicketOrderService ticketOrderService;
        private ITicketService ticketService;
        private readonly IZaloService zaloService;
        private readonly IZNSService znsService;
        private readonly IEmailService emailService;
        public SePayWebHookController(ITicketOrderService ticketOrderService, ITicketService ticketService, IZaloService zaloService, IEmailService emailService
            , IZNSService znsService)
        {
            this.ticketOrderService = ticketOrderService;
            this.ticketService = ticketService;
            this.zaloService = zaloService;
            this.emailService = emailService;
            this.znsService = znsService;
        }

        [HttpPost("[action]")]
        public async Task<SaveResultModel> PaymenTran([FromBody] WebHookReceiveModel model)
        {
            var rsNoti = new ResCommon<int>();
            if (model != null && model.content.IndexOf("VQGCD") > -1)
            {

 
               var res = await ticketOrderService.SaveTranSePayWebHook(model);
                if (res.ValueReturn > 0 && res.IsSuccess==true)
                {
                    long orderId = DetachMaDon(model.content);
                   
                    var objOD = ticketService.GetOrderInfo(orderId);

                    if (objOD.Total == model.transferAmount)
                    {
                        int paymentStatus = (int)PaymentStatus.Paid; // đã thanh toán
                        var resStatus = ticketOrderService.ChangePaymentStatusTicketOrder(orderId, paymentStatus, "SePay");
                        if (resStatus.IsSuccess)
                        {
                            ticketService.CreateTicketSubOrder(objOD.Id, objOD.Quanti, objOD.TicketCode, objOD.Price);
                            string subCode = string.Empty;
                            long subId = 0;
                            CreateQRCodeForSubCode(objOD.Id,out subCode,out subId);
                            var objODLast = ticketService.GetSubOrderByOrderId(orderId).FirstOrDefault();
                            objOD.SubOrderCode = objODLast.SubOrderCode;
                            objOD.SubOrderCodeId = objODLast.Id;
                            var modelZNS = new SendZNSModel()
                            {
                                CustomerName = objOD.CustomerName,
                                StrPrice = objOD.StrPrice,
                                StrTotal = objOD.StrTotal,
                                Quanti = objOD.Quanti,
                                StrCreatedDate = objOD.StrCreatedDate,
                                SubOrderCode = objOD.SubOrderCode,
                                PhoneNumber = objOD.Phone,
                                SubOrderId = objOD.SubOrderCodeId,
                                UrlQRCode = $"{AppSettingServices.Get.DomainSettings.WebService}{string.Format(AppSettingServices.Get.ZaloSettings.URLQRCode, objODLast.Id)}",
                                StrVisitDate = objOD.StrVisitDate,
                                GateName = objOD.TicketDescription
                            };
                            emailService.SendEMail(objOD);
                            rsNoti = znsService.SendZalo(modelZNS);
                           // rsNoti = znsService.SendZaloTicketOrderSuccess(orderId);



                        }
                    }

                }  


                return res;
            }
            else
            {
                return new SaveResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "object receive null",
                    ValueReturn = 0
                };
            }
        }


        public Int64 DetachMaDon(string content)
        {
            
            Int64 orderId = 0;
            content = content.ToLower();
            string refix = "VQGCD";
            try
            {
                if (content.IndexOf(refix) > -1)
                {
                    int indexCut = content.IndexOf(refix);
                    string madon = content.Substring(indexCut, content.Length - indexCut);
                    string madon2 = madon.Replace(refix, string.Empty);
                    string madon3 = madon2.Substring(0, 7);
                    orderId = Convert.ToInt64(madon3);
                }


            }
            catch (Exception ex)
            {

            }
            return orderId;


        }



        private void CreateQRCodeForSubCode(long orderId, out string subCodeReturn, out long subId)
        {
            var log = new StringBuilder();
            try
            {
                string rootFolder = Path.GetFullPath(AppSettingServices.Get.GeneralSettings.RootFolder);//config["General:RootFolder"];
                log.AppendLine($"RootFolder: {rootFolder}");
                var lstSubCode = ticketOrderService.GetSubCodePrintInfo(orderId).Result;
                if (lstSubCode.Any())
                {
                    foreach (var subCode in lstSubCode)
                    {
                        subCodeReturn = subCode.SubOrderCode;
                        subId = subCode.SubId;
                        log.AppendLine($"SubCode: {JsonConvert.SerializeObject(subCode)}");
                        using (QRCodeGenerator QrGenerator = new QRCodeGenerator())
                        {
                            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(subCode.SubId.ToString(), QRCodeGenerator.ECCLevel.Q);
                            QRCode QrCode = new QRCode(QrCodeInfo);
                            using (Bitmap bitMap = QrCode.GetGraphic(20))
                            {
                                string fileFullPath = string.Format(rootFolder, subCode.SubId);
                                if (!System.IO.File.Exists(fileFullPath))
                                {
                                    bitMap.Save(fileFullPath, ImageFormat.Jpeg);
                                }

                                bitMap.Dispose();
                            }




                        }
                    }
                }
                else
                {
                    subId = 0;
                    subCodeReturn = string.Empty;
                }

            }
            catch (Exception ex)
            {
                subCodeReturn = string.Empty;
                subId = 0;
                log.AppendLine($"[Exception]: {ex}");
            }
            finally
            {
                subCodeReturn = string.Empty;
                subId = 0;
                WriteLog.writeToLogFile(log.ToString());
            }

        }


        [HttpGet]
        public JsonResult UserConfirmPayment(long orderId)
        {
            var res = new SaveResultModel();
            
            var objOrder = ticketService.GetOrderInfo(orderId);
            if (objOrder.PaymentStatus == 1 && !string.IsNullOrEmpty(objOrder.SubOrderCode))
            {
                emailService.SendEMail(objOrder);
                var rsNoti = znsService.SendZaloTicketOrderSuccess(orderId);
                res.ErrorMessage = "";
            }
            else
            {
                res.IsSuccess = false;
                res.ErrorMessage = "Thanh toán không thành công";
            }
            return Json(res);
        }



    }
}