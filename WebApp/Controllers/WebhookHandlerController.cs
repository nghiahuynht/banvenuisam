using DAL.Enum;
using DAL.IService;
using DAL.Models.Zalo;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Infrastructure.Configuration;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookHandlerController : ControllerBase
    {
        protected readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly ITicketOrderService ticketOrderService;
        private readonly ITicketService ticketService;
        private readonly IZaloService zaloService;
        private readonly IEmailService emailService;
        private readonly IZNSService znsService;

        public WebhookHandlerController(ITicketOrderService ticketOrderService, ITicketService ticketService, IZaloService zaloService, IEmailService emailService
            , IZNSService znsService)
        {
            this.ticketOrderService = ticketOrderService;
            this.ticketService = ticketService;
            this.zaloService = zaloService;
            this.emailService = emailService;
            this.znsService = znsService;
        }

        [HttpPost("webhook_handler")]
        public IActionResult webhook_handler(webhook model)
        {
            try
            {
                
                logger.Debug("-------------------------------");
                string strJson = System.Text.Json.JsonSerializer.Serialize<webhook>(model);
                logger.Debug("strJson :" + strJson);
                // Kt xem giao dịch đã đc xử lý hay chưa
                var rsCheck = ticketService.GetTicketOderByPaymentId(model.data[0].id);
                if (rsCheck == null)
                {

                    // Get id of ticket Order
                    var ticketOrder = ticketOrderService.CheckPayment(model.data[0].description, model.data[0].amount);
                   
                    if ( ticketOrder != null && ticketOrder.Id > 0)
                    {
                        logger.Debug("ticketOrderId :" + ticketOrder.Id.ToString());
                        // update status 1
                        var resUpdateStatus = ticketOrderService.ChangePaymentStatusTicketOrder(ticketOrder.Id, 1, "auto aync", strJson, model.data[0].id);
                        if (resUpdateStatus.IsSuccess == true)
                        {
                            var objOD = ticketService.GetOrderInfo(ticketOrder.Id);
                            // create sub order
                            var rsCreateSubOrder = ticketService.CreateTicketSubOrder(ticketOrder.Id, objOD.Quanti, objOD.TicketCode, objOD.Price);
                            if(rsCreateSubOrder.IsSuccess == true)
                            {
                                CreateQRCode(ticketOrder.Id);
                                var objODNew = ticketService.GetOrderInfoSendZalo(ticketOrder.Id);
                                if (objOD.PaymentStatus == (int)PaymentStatus.Paid)//thanh toán thành công
                                {
                                    var data = new SendZNSModel()
                                    {
                                        CustomerName = objOD.CustomerName,
                                        StrPrice = objOD.StrPrice,
                                        StrTotal = objOD.StrTotal,
                                        Quanti = objOD.Quanti,
                                        StrCreatedDate = objOD.StrCreatedDate,
                                        SubOrderCode = objODNew.SubOrderCode,
                                        PhoneNumber = objOD.Phone,
                                        SubOrderId = objODNew.SubOrderCodeId,
                                        UrlQRCode = $"{AppSettingServices.Get.DomainSettings.WebService}{string.Format(AppSettingServices.Get.ZaloSettings.URLQRCode, objODNew.SubOrderCodeId)}",
                                        StrVisitDate = objOD.StrVisitDate,
                                        GateName = objOD.TicketDescription
                                    };
                                    string strJsonZalo = System.Text.Json.JsonSerializer.Serialize<SendZNSModel>(data);
                                    logger.Debug("Zalo info :" + strJsonZalo);
                                    var rsSendZalo = znsService.SendZalo(data);
                                    logger.Debug("SendZalo :" + rsSendZalo.IsSuccess.ToString());
                                    if (rsSendZalo.IsSuccess == false)
                                    {
                                        logger.Debug("SendZalo fail :" + rsSendZalo.Desc);
                                        znsService.SendZalo(data);
                                    }
                                    emailService.SendEMail(objOD);
                                    logger.Debug("Check Payment ===> OKE");
                                }
                            } 
                            else
                            {
                                logger.Debug("Create SubOrder Fail :" );
                            }    
                           
                        }

                    }

                    else
                    {
                        logger.Debug("Check Payment ===> FAIL");
                    }
                }
                else
                {
                    logger.Debug(" Payment processed");
                }
                return  Ok("Oke");
            }
            catch(Exception ex)
            {

                logger.Error("Exception :" + ex.Message);
                return Ok("Oke");
            }
        }

        private void CreateQRCode(long orderId)
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

                                //bitMap.Dispose();
                            }




                        }
                    }
                }

            }
            catch (Exception ex)
            {
                log.AppendLine($"[Exception]: {ex}");
            }
            finally
            {
                WriteLog.writeToLogFile(log.ToString());
            }

        }
    }

    public class webhook
    {
        public int error { get; set; }
        public List<webhookDetail> data { get; set; }
    }

    public class webhookDetail
    {
        public int id { get; set; }
        public string when { get; set; }
        public string description { get; set; }
        public double amount { get; set; }
        public double cusum_balance { get; set; }
        public string tid { get; set; }
        public string subAccId { get; set; }
        public string order { get; set; }
    }
}
