using DAL;
using DAL.Entities;
using DAL.IService;
using DAL.Models;
using DAL.Models.Ticket;
using Microsoft.AspNetCore.Mvc;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Model;
using Newtonsoft.Json;
using DAL.Models.TicketOrder;
using System.Security.Claims;

namespace WebApp.Controllers
{
    
    public class TicketController : AppBaseController
    {
        private readonly ITicketService ticketService;
        private readonly ITicketOrderService ticketOrderService;
        private readonly ICustomerService customerService;
        private IUserInfoService userService;
        private ISoatVeService soatVeService;

        public TicketController(ITicketService ticketService
            , ITicketOrderService ticketOrderService
            , ICustomerService customerService
            , IUserInfoService userService
            , ISoatVeService soatVeService)
        {
            this.ticketService = ticketService;
            this.ticketOrderService=ticketOrderService;
            this.customerService = customerService;
            this.userService = userService;
            this.soatVeService = soatVeService;
        }

        public IActionResult Index()
        {
            ViewBag.LstArea = ticketService.GetAllArea();
            return View();
        }
        /// <summary>
        /// thông tin chi tiết vé
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult TicketDetail(int? id)
        {
            var ticketinfo = new Ticket();
            if (id.HasValue)
            {
                ticketinfo = ticketService.GetTicketbyId(id.Value);
            }
            ViewBag.LstArea = ticketService.GetAllArea();
            ViewBag.LstLoaiIn = ticketService.GetAllLoaiIn();
            ViewBag.LstGroup = ticketService.GetTicketGroupDDL();
            return View(ticketinfo);
        }
        /// <summary>
        /// Lưu thông tin vé
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
     
        public async Task<JsonResult> SaveTicket([FromBody] Ticket model)
        {
            var res = new SaveResultModel();

            if (model.Id == 0)
            {
                res = await ticketService.CreateTicket(model, User.Identity.Name);
            }
            else
            {
                res = await ticketService.UpdateTicket(model, User.Identity.Name);
            }
            return Json(res);
        }
        /// <summary>
        /// Search ticket
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public DataTableResultModel<TicketGridModel> SearchTicket(TicketFilterModel filter)
        {
            var res = ticketService.SearchTicket(filter);
            return res;
        }
        /// <summary>
        /// Xóa ticket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteTicket(int id)
        {
            var res = await ticketService.DeleteTicket(id, User.Identity.Name);
            return Json(res);
        }

        public IActionResult TicketSales()
        {
            ViewBag.LstCustomer =  customerService.GetAllCustomer().GetAwaiter().GetResult();
            ViewBag.LstLoaiIn = ticketService.GetAllLoaiIn();
            
            return View();
        }
        /// <summary>
        /// Lưu thông tin đăt vé
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveTicketOrder([FromBody] ReqSaveTicket model)
        {
            var res = new ResultModel();
            try
            {
                if (!string.IsNullOrEmpty(model.TicketCode))
                {
                    var customer = customerService.GetCustomerByCode(model.CustomerCode)
                                                        .GetAwaiter().GetResult();
                    var priceTK = ticketService.GetTicketbyCode(model.TicketCode)?.Price ?? 0;
                    var objData = new TicketOrder()
                    {
                        TicketCode = model.TicketCode,
                        CustomerCode = model.CustomerCode,
                        CustomerName = customer?.CustomerName ?? string.Empty,
                        Price = priceTK,
                        Quanti = model.Quantity,
                        CustomerType = customer?.CustomerType ?? string.Empty,
                        Total = priceTK * model.Quantity
                    };
                    res = ticketService.CreateTicketOrder(objData, model.LoaiInCode, User.Identity.Name).GetAwaiter().GetResult();
                }
            }catch(Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
            return Json(res);
        }


        public async Task<IActionResult> SaleHistory()
        {
            var searchModel = new SaleHistoryFilterModel
            {
                SaleChanelId=0,
                UserName = "",
                FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                TicketCode = ""
            };
            ViewBag.LstAllTicket = ticketService.GetAllTicket();
            var roleUser = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            if(roleUser.ToUpper()=="ADMIN")
            {
                ViewBag.LstUser = await userService.ListAllUserInfo();
            }    
            else
            {
                 
                var allUserInfo = await userService.ListAllUserInfo();
                ViewBag.LstUser = allUserInfo.FindAll(x => x.UserName == User.Identity.Name);
            }    
            ViewBag.GateList = soatVeService.GetAllGateFullInfo();
           
            return View(searchModel);
        }
        /// <summary>
        /// Lấy danh sách lịch sử bán
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<DataTableResultModel<SaleHistoryGridModel>> GetSaleHistory(SaleHistoryFilterModel filter)
        {

            //filter.FromDate = Helper._ChangeFormatDate(filter.FromDate);
           // filter.ToDate = Helper._ChangeFormatDate(filter.ToDate);
            var res = await  ticketService.GetSaleHistory(filter,false);
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelFilter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileContentResult> ExportGetSaleHistory(string filter)
        {
            var filterModel = !string.IsNullOrEmpty(filter) ? JsonConvert.DeserializeObject<SaleHistoryFilterModel>(filter) : new SaleHistoryFilterModel();

            var res = await ticketService.GetSaleHistory(filterModel, true);
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            Color colFromHex = ColorTranslator.FromHtml("#3377ff");
            Color colFromHexTextHeader = ColorTranslator.FromHtml("#ffffff");

            workSheet.Cells["A1"].Value = "STT";
            workSheet.Cells["B1"].Value = "Mã vé";
            workSheet.Cells["C1"].Value = "Mã tra cứu";
            workSheet.Cells["D1"].Value = "Loại Vé";
            workSheet.Cells["E1"].Value = "Tên khách hàng";
            workSheet.Cells["E1"].Value = "Đối tượng KH";
            workSheet.Cells["F1"].Value = "Đơn giá";
            workSheet.Cells["G1"].Value = "SL";
            workSheet.Cells["H1"].Value = "Thành tiền";
            workSheet.Cells["I1"].Value = "Người bán";
            workSheet.Cells["J1"].Value = "Ngày bán";
            workSheet.Cells["K1"].Value = "Số biên lai Misa";
            workSheet.Cells["L1"].Value = "Mã tra cứu Misa";
            workSheet.Cells[1, 1, 1, 11].Style.Font.Bold = true;
            int rowData = 2;
            foreach (var item in res.data)
            {
                
                workSheet.Cells["A" + rowData].Value = item.RowNumber;
                workSheet.Cells["B" + rowData].Value = item.TicketCode;
                workSheet.Cells["C" + rowData].Value = item.SubOrderCode;
                workSheet.Cells["D" + rowData].Value = item.LoaiIn;
                workSheet.Cells["E" + rowData].Value = item.CustomerName;
                workSheet.Cells["E" + rowData].Value = item.ObjtypeName;
                workSheet.Cells["F" + rowData].Value = item.Price;
                workSheet.Cells["G" + rowData].Value = item.Quanti;
                workSheet.Cells["H" + rowData].Value = item.TotalAfterVAT;
                workSheet.Cells["I" + rowData].Value = item.FullName;
                workSheet.Cells["J" + rowData].Value = item.CreatedDate;
                workSheet.Cells["K" + rowData].Value = item.InvoiceNumber;
                workSheet.Cells["L" + rowData].Value = item.TransactionID;
                rowData++;
            }
            workSheet.Cells["G" + rowData].Value = "Tổng cộng";
            workSheet.Cells["G" + rowData].Style.Font.Bold = true;
            workSheet.Cells["H" + rowData].Value = res.data?.Sum(x => x.TotalAfterVAT).ToString();
            workSheet.Cells["H" + rowData].Style.Font.Bold = true;

            //Format table
            ExcelRange range = workSheet.Cells[1, 1, workSheet.Dimension.End.Row - 1, workSheet.Dimension.End.Column];
            ExcelTable tab = workSheet.Tables.Add(range, "Table1");
            tab.TableStyle = OfficeOpenXml.Table.TableStyles.Medium2;
            //FreezePanes
            //workSheet.View.FreezePanes(1,13);

            workSheet.Column(1).Width = 10;
            workSheet.Column(2).Width = 20;
            workSheet.Column(3).Width = 30;
            workSheet.Column(4).Width = 20;
            workSheet.Column(5).Width = 20;
            workSheet.Column(6).Width = 20;
            workSheet.Column(7).Width = 20;
            workSheet.Column(8).Width = 20;
            workSheet.Column(9).Width = 20;
            workSheet.Column(10).Width = 30;
            return File(excel.GetAsByteArray(), ExcelExportHelper.ExcelContentType, "DanhSachLichSuBanVe.xlsx");
        }
        /// <summary>
        /// View Báo cáo bán hàng
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SaleReport()
        {
            var searchModel = new SaleReportFilter
            {
                UserName = "",
                FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                TicketCode = ""
            };
            ViewBag.LstAllTicket = ticketService.GetAllTicket();
            var roleUser = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            if (roleUser.ToUpper() == "ADMIN")
            {
                ViewBag.LstUser = await userService.ListAllUserInfo();
            }
            else
            {

                var allUserInfo = await userService.ListAllUserInfo();
                ViewBag.LstUser = allUserInfo.FindAll(x => x.UserName == User.Identity.Name);
            }
            ViewBag.GateList = soatVeService.GetAllGateFullInfo();
            return View(searchModel);
        }
        /// <summary>
        /// Báo cáo bán hàng
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<DataTableResultModel<SaleReportGridModel>> GetSaleReport(SaleReportFilterModel filter)
        {

            var res = await ticketService.GetSaleReport(filter);
            return res;
        }
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ReportSaleCounterModel GetSaleReportCounter([FromBody] SaleReportFilterModel filter)
        {
            var res = ticketOrderService.ReportSaleCounterModel(filter);
            return res;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelFilter"></param>
        /// <returns></returns>
        [HttpGet]
        
        public async Task<FileContentResult> ExportGetSaleRP(SaleReportFilterModel modelFilter)
        {
            var res = await ticketService.GetSaleReport(modelFilter);
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            Color colFromHex = ColorTranslator.FromHtml("#3377ff");
            Color colFromHexTextHeader = ColorTranslator.FromHtml("#ffffff");

            workSheet.Cells["A1"].Value = "STT";
            workSheet.Cells["B1"].Value = "Tên đăng nhập";
            workSheet.Cells["C1"].Value = "Tên người dùng";
            workSheet.Cells["D1"].Value = "Mã vé";
            workSheet.Cells["E1"].Value = "Loại vé";
            workSheet.Cells["F1"].Value = "Tổng SL bán";
            workSheet.Cells["G1"].Value = "Tổng doanh số bán";
            workSheet.Cells[1, 1, 1, 6].Style.Font.Bold = true;
            int rowData = 2;
            foreach (var item in res.data)
            {

                workSheet.Cells["A" + rowData].Value = item.Id;
                workSheet.Cells["B" + rowData].Value = item.CreatedBy;
                workSheet.Cells["C" + rowData].Value = item.FullName;
                workSheet.Cells["D" + rowData].Value = item.TicketCode;
                workSheet.Cells["E" + rowData].Value = item.LoaiIn;
                workSheet.Cells["F" + rowData].Value = item.NumQuanTi;
                workSheet.Cells["G" + rowData].Value = item.TotalVAT;

                rowData++;
            }
            workSheet.Cells["E" + rowData].Value = "Tổng cộng";
            workSheet.Cells["E" + rowData].Style.Font.Bold = true;
            workSheet.Cells["F" + rowData].Value = res.data?.Sum(x => x.NumQuanTi).ToString();
            workSheet.Cells["F" + rowData].Style.Font.Bold = true;
            workSheet.Cells["G" + rowData].Value = res.data?.Sum(x => x.TotalVAT).ToString();
            workSheet.Cells["G" + rowData].Style.Font.Bold = true;

            //Format table
            ExcelRange range = workSheet.Cells[1, 1, workSheet.Dimension.End.Row - 1, workSheet.Dimension.End.Column];
            ExcelTable tab = workSheet.Tables.Add(range, "Table1");
            tab.TableStyle = OfficeOpenXml.Table.TableStyles.Medium2;
            //FreezePanes
            //workSheet.View.FreezePanes(1, 11);

            workSheet.Column(1).Width = 10;
            workSheet.Column(2).Width = 20;
            workSheet.Column(3).Width = 30;
            workSheet.Column(4).Width = 20;
            workSheet.Column(5).Width = 20;
            workSheet.Column(6).Width = 20;
            workSheet.Column(7).Width = 20;
            return File(excel.GetAsByteArray(), ExcelExportHelper.ExcelContentType, "DanhSach_BCBanHang.xlsx");
        }

        public IActionResult ReportSoatVe()
        {
            var searchModel = new SoatveReportFilter
            {
                FromDate = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
            };
            ViewBag.LstAllGateList = ticketService.GetAllGatelist();
            return View(searchModel);
        }
        /// <summary>
        /// Báo cáo soát vé
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<DataTableResultModel<SoatVeReportGridModel>> GetSoatVeReport(SoatveReportFilter filter)
        {
            var res = await ticketService.GetSoatVeReport(filter);
                return res;
        }



        #region Ticket user
        public async Task<IActionResult> TicketWithUser()
        {
            var allUser = await userService.ListAllUserInfo();
            var allLoaiVe =  ticketService.GetAllTicket();
            ViewBag.AllLoaiVe = allLoaiVe;
            return View(allUser);
        }

        [HttpPost]
       
        public JsonResult SaveTicketUser([FromBody] ReqSaveTicketUser model)
        {
            var res = new ResultModel();
            try
            {
                // Delete trước khi tạo mới
                ticketService.DeleteTicketUser(model.UserId);
               
                foreach(var id in model.TicketIds)
                {
                    ticketService.CreateTicketUser(model.UserId, id, User.Identity.Name);
                }
                res.IsSuccess = true;
                res.ErrorMessage = "Thành công";
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
            return Json(res);
        }

        [HttpGet]
        public JsonResult GetTicketByUser(int userId)
        {
            var res =  ticketService.GetTicketByUser(userId);
          
            return Json(res);
        }
        #endregion




    }
}
