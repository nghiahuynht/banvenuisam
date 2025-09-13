using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonFW.Domain.Model.Payment;
using DAL.Entities;
using DAL.IService;
using DAL.Models;
using DAL.Models.Ticket;
using Microsoft.AspNetCore.Mvc;
using DAL;
using Microsoft.AspNetCore.Http;
using System.Net;
using DAL.Enum;
using DAL.Models.ConDao;
using DAL.Models.Zalo;
using Microsoft.AspNetCore.Routing;
using QRCoder;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Hosting.Server;
using System.Drawing.Imaging;
using Microsoft.Extensions.Configuration;
using WebApp.Infrastructure.Configuration;
using System.Text;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net.Http;
using DAL.Models.VIB;

namespace WebApp.Controllers
{
    public class ConDaoController : Controller
    {
        private readonly ITicketService ticketService;
        private readonly ITicketOrderService ticketOrderService;
        private readonly ICustomerService customerService;
        private readonly IPaymentService paymentService;
        private readonly IResultCodeService codeService;
        private readonly IZaloService zaloService;
        private readonly IZNSService znsService;
        private readonly IConfiguration config;
        private readonly IEmailService emailService;
        private readonly ISoatVeService soatVeService;
        private readonly IVIBService vibService;
       
        public ConDaoController(ITicketService ticketService
            , ITicketOrderService ticketOrderService
            , ICustomerService customerService
            , IPaymentService paymentService
            , IResultCodeService codeService
            , IZaloService zaloService
            , IConfiguration config
            , IEmailService emailService
            , IZNSService znsService
            , ISoatVeService soatVeService
            , IVIBService vibService)
        {
            this.ticketService = ticketService;
            this.ticketOrderService = ticketOrderService;
            this.customerService = customerService;
            this.paymentService = paymentService;
            this.codeService = codeService;
            this.zaloService = zaloService;
            this.emailService = emailService;
            this.config = config;
            this.znsService = znsService;
            this.soatVeService = soatVeService;
            this.vibService = vibService;
        }

        public IActionResult Index(string id)
        {
            var lstTicket = new List<Ticket>();
            lstTicket = ticketService.GetAllTicket().Where(x => x.Id == 2 || x.Id == 8 || x.Id == 6).ToList();
            ViewBag.Ticket = lstTicket;
            return View();
        }
        

        /// <summary>
        /// Nếu có thông tin OrderId => flow cập nhật load lại thông tin đơn hàng
        /// </summary>
        /// <param name="ticketCode"> mã vé</param>
        /// <param name="orderId"> ID đơn hàng</param>
        /// <returns></returns>

        public IActionResult TicketOrder(string partner, string ticketCode, int quanti, string visitDate)
        {

            string printType = "Ve_Doan";// mua online thì mặc định = Ve_Doan
            var ticketDetail = ticketService.GetTicketbyCode(ticketCode);
           

            var viewModel = new ResOrderInfoDto();

            viewModel.TicketCode = ticketDetail.Code;
            viewModel.Price = (ticketDetail != null? ticketDetail.Price:0);
            viewModel.Quanti = quanti;
            viewModel.VisitDate = DateTime.Now;
            viewModel.TicketDescription = ticketDetail.Description;
            viewModel.PartnerCode = partner;
            return View(viewModel);
        }
        /// <summary>
        /// Tạo đơn hàng đặt vé
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveOrder([FromBody] ReqCreateOrder model)
        {
            WriteLog.writeToLogFile(JsonConvert.SerializeObject(model));
            var log = new StringBuilder();
            var res = new ResCommon<long>();
            try
            {
                log.AppendLine($"RequestURL: /ConDao/SaveOrder");
                log.AppendLine($"Input: {JsonConvert.SerializeObject(model)}");
                if (!string.IsNullOrEmpty(model.TicketCode))
                {
                    /// <summary>
                    /// Lưu thông tin đơn hàng
                    /// </summary>
                    #region lưu thông tin KH
                    var (isSuccess, cusCode) = GenerateCustomerCode();
                    if (!isSuccess)
                    {
                        res.IsSuccess = false;
                        res.Desc = "Đã xảy ra lỗi khi tạo mã khách hàng";
                        return Json(res);
                    }
                    var inputCus = new Customer()
                    {
                        CustomerCode = cusCode,
                        CustomerName = model.FullName,
                        Email = model.Email,
                        Phone = model.PhoneNumber,
                        SaleChannelId = (int)SaleChannelId.Online,
                        AgencyName = model.AgencyName,
                        TaxCode = model.TaxCode,
                        TaxAddress = model.TaxAddress,
                        CustomerType = string.IsNullOrEmpty(model.TaxCode) ? "Khach_Le" : "ToChuc"
                    };
                    var resCus = customerService.CreateCustomer(inputCus, "online").GetAwaiter().GetResult();
                    #endregion
                    #region Lưu thông tin Đơn hàng
                    if (resCus.IsSuccess)
                    {
                        var objTicket = ticketService.GetTicketbyCode(model.TicketCode);
                        if (objTicket == null)
                        {
                            res.IsSuccess = false;
                            res.Desc = "Không tìm thấy thông tin vé";
                            return Json(res);
                        }

                       
                        var objData = new TicketOrder()
                        {
                            TicketCode = model.TicketCode,
                            CustomerCode = cusCode,
                            CustomerName = model.FullName,
                            Price = objTicket.Price,
                            Quanti = model.Quantity,
                            CustomerType = objTicket.LoaiIn,
                            Total = (objTicket.Price * model.Quantity),
                            SaleChannelId = (int)SaleChannelId.Online,
                            VisitDate  = model.VisitDate,
                            GateName = objTicket.Description,
                            ObjType= "NguoiLon",// mua từ webonline mặc định = 1 đối tượng bình thường,
                            PaymentType=Contanst.PaymentType_CK
                            
                        };


                        WriteLog.writeToLogFile(JsonConvert.SerializeObject(objData));


                        var rsCreate = ticketService.CreateTicketOrder(objData, objTicket.LoaiIn, "Online").GetAwaiter().GetResult();

                        if (res.IsSuccess && rsCreate.ValueReturn > 0)
                        {
                            res.IsSuccess = true;
                            res.Desc = "Tạo đơn hàng thành công!";
                            res.Data = rsCreate.ValueReturn;
                        }
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.Desc = "Lưu thông tin KH thất bại!";
                    }
                        #endregion

                }
                else
                {
                    res.Desc = "Không tìm thấy thông tin vé";
                    res.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Desc = ex.Message;
            }
            finally
            {
                WriteLog.writeToLogFile(log.ToString());
            }
            return Json(res);
        }
        /// <summary>
        /// lấy thông tin thanh toán
        /// </summary>
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetUrlPayment([FromBody] ReqPaymentDto model)
        {
            var res = new ResCommon<string>();
            try
            {
                if (model.OrderId > 0)
                {
                    var objOrder = ticketService.GetTicketOrderbyId(model.OrderId);
                    var orderInfo = ticketService.GetOrderInfo(model.OrderId);
                    if (objOrder == null)
                    {
                        res.IsSuccess = false;
                        res.Desc = "Không tìm thấy thông tin đơn hàng";
                        return Json(res);
                    }
                    #region check đã thanh toán ?
                    if (objOrder.PaymentStatus == 1)//đã thanh toán
                    {
                        WriteLog.writeToLogFile($"[GetUrlPayment]: Đơn hàng {model.OrderId}) -  DaThanhToan");
                        res.IsSuccess = false;
                        res.Desc = "Đơn hàng đã thanh toán không được thanh toán nữa!";
                        return Json(res);
                    }
                    #endregion
                    /// <summary>
                    /// Lưu thông tin đơn hàng trước khi Thanh toán
                    /// </summary>
                    #region lưu thông tin thanh toán đơn hàng
                    //Cập nhật thông tin chọn thanh toán
                    objOrder.PaymentType = model.PaymentType.ToString();
                    objOrder.PaymentFee = model.PaymentType.ToString().ToLower().Equals(Contanst.PaymentType_OnePay.ToLower()) ? 
                            Contanst.PaymentFee_OnePay : Contanst.PaymentFee_TrucTiep;
                    objOrder.Total = (objOrder.Price * objOrder.Quanti) + objOrder.PaymentFee;
                    var rsOrder = ticketService.UpdateTicketOrder(objOrder, "Online").GetAwaiter().GetResult();
                    if (!rsOrder.IsSuccess)
                    {
                        res.IsSuccess = false;
                        res.Desc = "Cập nhật thông tin thanh toán thất bại";
                        return Json(res);
                    }
                    #endregion

                    #region Lấy thông tin thanh toán
                    if(model.PaymentType == PaymentTypeEnum.OnePay)// thánh toán online qua onepay
                    {
                        var objOD = ticketService.GetOrderInfo(rsOrder.Data);
                        if (objOD == null)
                        {
                            res.IsSuccess = false;
                            res.Desc = "Không tìm thấy thông tin đơn hàng.";
                            return Json(res);
                        }
                        var input = new PaymentModel()
                        {
                            OrderId = objOD.Id,
                            OrderCode = objOD.Id.ToString(),
                            PhoneNumber = objOD.Phone,
                            CustomerCode = objOrder.CustomerCode,
                            TotalVAT = objOD.Total
                        };
                        var rs = GetInfoPayment(input);
                        if (string.IsNullOrEmpty(rs))
                        {
                            res.Desc = "Lấy thông tin thanh toán thất bại";
                            res.IsSuccess = false;
                            return Json(res);
                        }
                        res.Data = rs;
                    }

                    #endregion

                    #region ddphuong Lấy qr code chuyển khoản VIB 
                    //VirtualAccountModel vibmodel = new VirtualAccountModel()
                    //{
                    //    CIF = "89022756",
                    //    VirtualAccount = $"G4M{orderInfo.Id.ToString().PadLeft(12, '0')}",// replace theo định dạng G4M000000000000
                    //    Amount = orderInfo.StrTotal.Replace(",", ""),
                    //    Code = "G4M",
                    //    Name = orderInfo.CustomerName,
                    //    IsvalidAmount = "Y",
                    //    ReqId = orderInfo.Id.ToString()
                    //};
                    //var vibrs = await vibService.GenerateQRCodeVIB(vibmodel);
                    //string madon = Helper.GenMaDon(orderInfo.Id);
                    //res.IsSuccess = vibrs.IsSuccess;
                    //res.Desc = vibrs.Desc;

                    #endregion

                    #region ddphuong Lấy qr code chuyển khoản Agribank
                    string madon = Helper.GenMaDon(orderInfo.Id);
                    //QRCodePaymentModel jsbody = new QRCodePaymentModel()
                    //{
                    //    accountNo = string.Concat("v", AppSettingServices.Get.GenerateQRCodeSettings.ServiceCode ?? "970405", string.Format("TVVT{0}", madon)),
                    //    accountName = AppSettingServices.Get.GenerateQRCodeSettings.AccountName ?? "Thư viện - Bảo Tàng tỉnh Bà Rịa - Vũng Tàu",
                    //    acqId = AppSettingServices.Get.GenerateQRCodeSettings.AcqId ?? "970405", // Agrribank
                    //    addInfo = "Mua vé TVBT BRVT",
                    //    amount = orderInfo.Total.ToString(),
                    //    template = AppSettingServices.Get.GenerateQRCodeSettings.Template ?? "compact2",
                    //};

                    QRCodePaymentModel jsbody = new QRCodePaymentModel() // đoanh này Nghĩa làm tạm để thanh toán thủ công trước
                    {
                        accountNo = "6090201043889",
                        accountName = "Bảo Tàng - Thư Viện Tỉnh Bà Rịa - Vũng Tàu",
                        acqId = AppSettingServices.Get.GenerateQRCodeSettings.AcqId ?? "970405", // Agrribank
                        addInfo = "BTTV" + model.OrderId,
                        amount = objOrder.Total.ToString(),
                        template = AppSettingServices.Get.GenerateQRCodeSettings.Template ?? "compact2",
                    };

                    string imageBase64 = paymentService.GenerateQRCodePayment(jsbody);
                    if (!string.IsNullOrEmpty(imageBase64))
                    {
                        res.Desc = imageBase64;
                    }
                    else
                    {
                        res.Desc = "Get QR Code thất bại";
                        res.IsSuccess = false;
                    }
                    #endregion
                }
                else
                {
                    res.Desc = "Không tìm thấy thông tin đơn hàng";
                    res.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.IsSuccess = false;
                res.Desc = "Đã xảy ra lỗi trong quá trình xử lý!";
            }
            return Json(res);
        }

       



        public async Task<ActionResult> Finish()
        {
            var paymentInfo = await UpdatePaymentSuccessfull();
            var objOD = ticketService.GetOrderInfo(paymentInfo.ContractId);
            if (objOD.PaymentStatus == (int)PaymentStatus.Paid)//thanh toán thành công
            { var model = new SendZNSModel()
                {
                    CustomerName = objOD.CustomerName,
                    StrPrice = objOD.StrPrice,
                    StrTotal = objOD.StrTotal,
                    Quanti = objOD.Quanti,
                    StrCreatedDate = objOD.StrCreatedDate,
                    SubOrderCode = objOD.SubOrderCode,
                    SubOrderId = objOD.SubOrderCodeId,
                    PhoneNumber = objOD.Phone,
                    UrlQRCode = $"{AppSettingServices.Get.DomainSettings.WebService}{string.Format(AppSettingServices.Get.ZaloSettings.URLQRCode, objOD.SubOrderCodeId)}",
                    StrVisitDate = objOD.StrVisitDate,
                    GateName = objOD.TicketDescription
                };
                znsService.SendZalo(model);
                emailService.SendEMail(objOD);
            }
            var desctiptionVN = codeService.GetResultCodeByCode(paymentInfo.TxnResponseCode)?.DescriptionVN ?? string.Empty;
            ViewBag.DescriptionVN = desctiptionVN;
            RouteValueDictionary RouteInfo = new RouteValueDictionary();
            RouteInfo.Add("id", paymentInfo.ContractId);
            return RedirectToAction("FinishOrder", RouteInfo);
        }
        
        public async Task<IActionResult> FinishOrder(int? id)// dành cho chuyển khoản
        {

            OrderResultViewModel viewmodel = new OrderResultViewModel();
            try
            {
                if (id.HasValue)
                {
                    var objTicket = ticketService.GetTicketOrderbyId(id.Value);

                    viewmodel.TicketOrder = objTicket;
                    viewmodel.Customer = await customerService.GetCustomerByCode(viewmodel.TicketOrder.CustomerCode);
                   // viewmodel.ListSubCode =await ticketOrderService.GetSubCodePrintInfo(id.Value);
                }

            }
            catch(Exception ex)
            {
            }

            
            return View(viewmodel);
        }


        #region payment
        public string GetInfoPayment(PaymentModel model)
        {
            var rs = string.Empty;
            try
            {
                string SECURE_SECRET = Contanst.SECURE_SECRET;
                var urlPayment = $"{AppSettingServices.Get.DomainSettings.OnepayService}{UrlsConfig.OnepayOperations.GetURLPayment}";
                PaymentHelper pm = new PaymentHelper(urlPayment);
                pm.SetSecureSecret(SECURE_SECRET);
                // Add the Digital Order Fields for the functionality you wish to use
                // Core Transaction Fields
                pm.AddDigitalOrderField("Title", "SongMinhTech");
                pm.AddDigitalOrderField("vpc_Locale", "vn");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
                pm.AddDigitalOrderField("vpc_Version", "2");
                pm.AddDigitalOrderField("vpc_Command", "pay");
                pm.AddDigitalOrderField("vpc_Merchant", "TESTONEPAY");
                pm.AddDigitalOrderField("vpc_AccessCode", "6BEB2546");
                pm.AddDigitalOrderField("vpc_MerchTxnRef", $"GD{Guid.NewGuid().ToString("n")}");
                pm.AddDigitalOrderField("vpc_OrderInfo", $"{model.OrderId}");
                pm.AddDigitalOrderField("vpc_Amount", $"{decimal.Truncate(model.TotalVAT)}00");
                pm.AddDigitalOrderField("vpc_Currency", "VND");
                ///url trả về khi thanh toán thành công
                pm.AddDigitalOrderField("vpc_ReturnURL", $"{AppSettingServices.Get.DomainSettings.WebService}{UrlsConfig.PaymentOperations.GetURLReturnPayment}");
                // Thong tin them ve khach hang. De trong neu khong co thong tin
                pm.AddDigitalOrderField("vpc_Customer_Phone", model.PhoneNumber);
                pm.AddDigitalOrderField("vpc_Customer_Email", "");
                pm.AddDigitalOrderField("vpc_Customer_Id", model.CustomerCode);
                // Dia chi IP cua khach hang
                pm.AddDigitalOrderField("vpc_TicketNo", GetIPAddress());
                //Link trang thanh toán của website trước khi chuyển sang OnePAY
                pm.AddDigitalOrderField("AgainLink", string.Format($"{AppSettingServices.Get.DomainSettings.WebService}{UrlsConfig.PaymentOperations.GetAgainLinkPayment}", model.OrderId));//"https://localhost:44377/condao/ticketorder");
                // Chuyen huong trinh duyet sang cong thanh toan
                rs = pm.Create3PartyQueryString();

            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[GetInfoPayment][Exception]: {ex}");
                return rs;
            }
            return rs;
        }
        [HttpGet]
       // [ValidateAntiForgeryToken]
        public JsonResult SendNoti(long id)
        {
            var rs = new ResCommon<int>();
            var objOD = ticketService.GetOrderInfo(id);
            


            if (objOD.PaymentStatus == (int)PaymentStatus.Paid)//thanh toán thành công
            {
                var lstSubCode = ticketOrderService.GetListPrintPdfByOrderId(id);
                foreach (var item in lstSubCode)
                {

                    var model = new SendZNSModel()
                    {
                        CustomerName = objOD.CustomerName,
                        StrPrice = item.Price.ToString("N0"),
                        StrTotal = item.TotalAfterVAT.ToString("N0"),
                        Quanti = item.Quanti,
                        StrCreatedDate = item.CreatedDate.Value.ToString("dd/MM/yyyy"),
                        SubOrderCode = item.SubOrderCode,
                        PhoneNumber = objOD.Phone,
                        SubOrderId = item.SubId,
                        UrlQRCode = $"{AppSettingServices.Get.DomainSettings.WebService}{string.Format(AppSettingServices.Get.ZaloSettings.URLQRCode, item.SubId)}",
                        StrVisitDate = objOD.StrVisitDate,
                        GateName = item.AreaName
                    };
                    rs = znsService.SendZalo(model);



                    //var model = new SendZNSModel()
                    //{
                    //    CustomerName = objOD.CustomerName,
                    //    StrPrice = objOD.StrPrice,
                    //    StrTotal = objOD.StrTotal,
                    //    Quanti = objOD.Quanti,
                    //    StrCreatedDate = objOD.StrCreatedDate,
                    //    SubOrderCode = objOD.SubOrderCode,
                    //    PhoneNumber = objOD.Phone,
                    //    SubOrderId = objOD.SubOrderCodeId,
                    //    UrlQRCode = $"{AppSettingServices.Get.DomainSettings.LangBiangService}{string.Format(AppSettingServices.Get.ZaloSettings.URLQRCode, objOD.SubOrderCodeId)}",
                    //    StrVisitDate = objOD.StrVisitDate,
                    //    GateName = objOD.TicketDescription
                    //};
                    //rs = znsService.SendZalo(model);
                }


                emailService.SendEMail(objOD);
                //rs.Desc = SendMailOrderTicketSuccess(objOD.Email);
            }
            else
            {
                rs.IsSuccess = false;
                rs.Desc = "Đơn hàng chưa thanh toán, không gửi zalo!";
            }
            return Json(rs);
        }
        protected string GetIPAddress()
        {
            string result = string.Empty;
            try
            {
                string hostName = Dns.GetHostName();
                IPAddress[] ipAddress = Dns.GetHostAddresses(hostName);
                result = string.Join(",", ipAddress.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                result = string.Empty;
            }
            return result;
        }


        public async Task<PaymentInfo> UpdatePaymentSuccessfull()
        {
            var modelPayment = new PaymentInfo();
            try
            {
                var urlPayment = $"{AppSettingServices.Get.DomainSettings.OnepayService}{UrlsConfig.OnepayOperations.GetURLPayment}";

                if (HttpContext.Request.Query.Count <= 0)// k có kết quả trả về của cổng thanh toán onepay
                {
                    return new PaymentInfo();
                }
                string SECURE_SECRET = Contanst.SECURE_SECRET;
                PaymentHelper pm = new PaymentHelper(urlPayment);
                pm.SetSecureSecret(SECURE_SECRET);
                string hashvalidateResult = pm.Process3PartyResponse(HttpContext.Request.Query);
                // Xu ly tham so tra ve va kiem tra chuoi du lieu ma hoa
                // Lay gia tri tham so tra ve tu cong thanh toan
                var ticketODId = Helper.CheckIntNull(pm.GetResultField("vpc_OrderInfo", null));
                var ticketInfo = ticketService.GetTicketOrderbyId(ticketODId);
                modelPayment = new PaymentInfo()
                {
                    ContractId = ticketInfo.Id,
                    //ContractCode = ticketInfo.Code,
                    TxnResponseCode = Helper.CheckIntNull(pm.GetResultField("vpc_TxnResponseCode", null)),
                    Amount = Helper.CheckDecimalNull(pm.GetResultField("vpc_Amount", null)) / 100,
                    Locale = pm.GetResultField("vpc_Locale", ""),
                    Command = pm.GetResultField("vpc_Command", ""),
                    //Version = pm.GetResultField("vpc_Version", ""),
                    Merchant = pm.GetResultField("vpc_Merchant", ""),
                    MerchTxnRef = pm.GetResultField("vpc_MerchTxnRef", ""),
                    TransactionNo = Helper.CheckIntNull(pm.GetResultField("vpc_TransactionNo", null)),
                    Message = pm.GetResultField("vpc_Message", ""),
                    CurrencyCode = pm.GetResultField("vpc_CurrencyCode", ""),
                    AdditionData = Helper.CheckIntNull(pm.GetResultField("vpc_AdditionData", null)),
                    SecureHash = HttpContext.Request.Query["vpc_SecureHash"],
                    ValidatedHash = hashvalidateResult

                };
                // Sua lai ham check chuoi ma hoa du lieu
                if (hashvalidateResult == "CORRECTED" && modelPayment.TxnResponseCode == 0)
                {
                    modelPayment.NotePayment = "Transaction was paid successful";
                }
                else if (hashvalidateResult == "INVALIDATED" && modelPayment.TxnResponseCode == 0)
                {
                    modelPayment.NotePayment = "Transaction is pending";
                }
                else
                {
                    modelPayment.NotePayment = "Transaction was not paid successful";
                }
                //Cập nhật thông tin vào bảng thanh toán
                var rsPayment = await paymentService.InsertOrUpdatePayment(modelPayment, "Online");
                if (rsPayment.IsSuccess)//Cập nhật/insert thông tin Thanh toán thành công => cập nhật trạng thái thanh toán vào đơn hàng
                {
                    var contractObj = ticketService.GetTicketOrderbyId(modelPayment.ContractId);
                    contractObj.PaymentStatus = hashvalidateResult == "CORRECTED" && modelPayment.TxnResponseCode == 0 ? 1 : 0;
                    contractObj.PaymentDate = DateTime.Now;
                    contractObj.PaymentType = "Onepay";
                    await ticketService.UpdateTicketOrder(contractObj, "Onlline");
                }

                return modelPayment;
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[UpdatePaymentSuccessfull][Exception]: {ex}");
                return new PaymentInfo();
            }

        }
        #endregion
        public IActionResult PaymentOrder(int? id)
        {
            var objOrder = ticketService.GetOrderInfo(id.Value);

            var infoAppBankAndroid = paymentService.GetInfoAppBankAndroid();
            var infoAppBankIOS = paymentService.GetInfoAppBankIOS();
            ViewBag.LstInfoAppBankAndroid = infoAppBankAndroid.Apps;
            ViewBag.LstInfoAppBankIOS = infoAppBankIOS.Apps;
            return View(objOrder);
        }

        /// <summary>
        /// Tạo mã Customer
        /// </summary>
        /// <returns></returns>
        public (bool, string) GenerateCustomerCode()
        {
            string cusCode = string.Empty;
            bool result = false;

            int i = 0;
            do
            {
                var (isSuccess, code) = customerService.GenerateCustomerCode();
                if (isSuccess)
                {
                    var checkDuplicate = customerService.GetCustomerByCode(code).GetAwaiter().GetResult();
                    if (checkDuplicate == null)// chưa có mã đơn hàng trùng=> hợp lệ
                    {
                        result = true;
                        cusCode = code;
                        break;
                    }
                }
                i = i + 1;
            } while (i < 5);

            return (result, cusCode);
        }





        public string SendMailOrderTicketSuccess(string mailTo)
        {
            string resultMail = "ok";
            try
            {
                string mailFrom = Path.GetFullPath(AppSettingServices.Get.EmailSettings.UserName);
                string passSend = Path.GetFullPath(AppSettingServices.Get.EmailSettings.Password);
                string smtpAddress = Path.GetFullPath(AppSettingServices.Get.EmailSettings.SMTPAddress);
                int smtpPort = AppSettingServices.Get.EmailSettings.SMTPPort;



                MailMessage mail = new MailMessage();
                mail.To.Add(new MailAddress(mailTo, "Người mua"));
                mail.From = new MailAddress(mailFrom,"Côn Đảo");
                mail.Subject = "Thông báo mua vé thành công";
                mail.Body = "<strong>Mua vé test</strong>";
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(smtpAddress, smtpPort);
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(mailFrom, passSend); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);



                //MailMessage mail = new MailMessage();
                //mail.To.Add(new MailAddress(mailTo, "GAMAN"));
                //MailAddress add = new MailAddress(mailFrom,"hehe");
                //mail.From = add;
                //mail.Bcc.Add(new MailAddress("vtmthi_zeta@zetaprocess.com.vn", "Vo Thi Minh Thi"));
                //mail.IsBodyHtml = true;
                //StringBuilder body = new StringBuilder();
                //mail.Subject = "Thông báo mua vé thành công";
                //body.Append("Duy ngã độc tôn");
                //mail.Body = body.ToString();

                //SmtpClient smtpclient = new SmtpClient(smtpAddress);  //your real server goes here
                //smtpclient.Timeout = Convert.ToInt32(20000);
                //smtpclient.Send(mail);
            }
            catch (Exception ex)
            {
                resultMail = ex.Message;
            }
            return resultMail;
        }











    }
}