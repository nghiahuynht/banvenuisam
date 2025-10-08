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
using NLog;
using NLog.Fluent;

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
       //  protected readonly ILogger logger = LogManager.GetCurrentClassLogger();
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
            var log = new StringBuilder();
            var rsNoti = new ResCommon<int>();
            log.AppendLine("--------------BEGIN SEPAY-------------");
            string strJson = System.Text.Json.JsonSerializer.Serialize<WebHookReceiveModel>(model);
            log.AppendLine("strJson :" + strJson);
            
            if (model != null && model.code.ToLower().IndexOf("dh") > -1)
            {

                var res = await ticketOrderService.SaveTranSePayWebHook(model);
                log.AppendLine("Save log db :" + res.ValueReturn);
                if (res.ValueReturn > 0 && res.IsSuccess==true)
                {
                    long orderId = DetachMaDon(model.code);
                    log.AppendLine("DetachMaDon :" + orderId);
                    var objOD = ticketService.GetOrderInfo(orderId);

                    if (objOD.Total == model.transferAmount)
                    {
                        int paymentStatus = (int)PaymentStatus.Paid; // đã thanh toán
                        var resStatus = ticketOrderService.ChangePaymentStatusTicketOrder(orderId, paymentStatus, "SePay");
                        log.AppendLine("Update status ticket :" + resStatus.IsSuccess);
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

                log.AppendLine("--------------END SEPAY-------------");
                WriteLog.writeToLogFile(log.ToString());
                return res;
            }
            else
            {
                log.AppendLine("detech content fail :" );
                log.AppendLine("--------------END SEPAY-------------");
                WriteLog.writeToLogFile(log.ToString());
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
            string refix = "dh";
            try
            {
                if (content.IndexOf(refix) > -1)
                {
                    int indexCut = content.IndexOf(refix);
                    string madon = content.Substring(indexCut, content.Length - indexCut);
                    string madon2 = madon.Replace(refix, string.Empty);
                    if (madon2.Length >= 7)
                    {
                        // Lấy 7 ký tự cuối cùng
                        string madon3 = madon2.Substring(madon2.Length - 7);
                        orderId = Convert.ToInt64(madon3);
                    }
                    else
                    {
                        // Nếu độ dài < 7, lấy toàn bộ chuỗi
                        orderId = Convert.ToInt64(madon2);
                    }
                }


            }
            catch (Exception ex)
            {
                var log = new StringBuilder();
                log.AppendLine("detech mã đơn lỗi :"+ content);
                WriteLog.writeToLogFile(log.ToString());
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