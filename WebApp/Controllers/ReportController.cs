using DAL.IService;
using DAL.Models;
using DAL.Models.ConDao;
using DAL.Models.Report;
using DAL.Models.Ticket;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Model;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Table;

namespace WebApp.Controllers
{
    public class ReportController : AppBaseController
    {
        private readonly ITicketService ticketService;
        private readonly IReportService reportService;
        private readonly ICustomerService customerService;
        private readonly ITicketOrderService ticketOrderService;
        private IUserInfoService userService;
        private ISoatVeService soatVeService;

        public ReportController(ITicketService ticketService
            , IReportService reportService
            , ICustomerService customerService
            , ITicketOrderService ticketOrderService
            , IUserInfoService userService
            , ISoatVeService soatVeService)
        {
            this.ticketService = ticketService;
            this.reportService = reportService;
            this.ticketOrderService = ticketOrderService;
            this.customerService = customerService;
            this.userService = userService;
            this.soatVeService = soatVeService;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// View Báo cáo loại vé
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> TicketTypeReport()
        {
            var searchModel = new TicketTypeRPFilter
            {
                FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy")
            };
            ViewBag.LstAllTicket = ticketService.GetAllTicket();
            ViewBag.LstUser = await userService.ListAllUserInfo();
            ViewBag.GateList = soatVeService.GetAllGateFullInfo();
            return View(searchModel);
        }

        /// <summary>
        /// Báo cáo doanh số bán theo loại vé
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ResGetRPSales GetReportSalesByTicketType([FromBody] TicketTypeRPFilter filter)
        {

            var res =  reportService.GetReportSalesByTicketType(filter).GetAwaiter().GetResult();
            
           
            List<MockupData> mockupData = new List<MockupData>
                {
                    new MockupData
                    {
                        GateCode = "Tuyen_1",
                        GateName = "Đảo côn sơn - Vòng các đảo nhỏ",
                        Price = 60000,
                        SlSale = 5,
                        AmountSale = 300000
                    },
                    new MockupData
                    {
                        GateCode = "Tuyen_2",
                        GateName = "Đảo Côn Sơn - Hòn Tài - Hòn Bảy Cạnh",
                        Price = 60000,
                        SlSale = 5,
                        AmountSale = 300000
                    },
                    new MockupData
                    {
                        GateCode = "Tuyen_3",
                        GateName = "Đảo Côn Sơn  - Hòn Bảy Cạnh - Hòn Cau",
                        Price = 60000,
                        SlSale = 5,
                        AmountSale = 300000
                    },
                    new MockupData
                    {
                        GateCode = "Tuyen_6",
                        GateName = "Đảo Côn Sơn - Hòn Bà - Hòn Tre Lớn",
                        Price = 60000,
                        SlSale = 5,
                        AmountSale = 300000
                    },
                     new MockupData
                    {
                        GateCode = "Tuyen_7",
                        GateName = "Đảo Côn Sơn - Hòn Trứng - Vịnh Đầm Tre",
                        Price = 50000,
                        SlSale = 10,
                        AmountSale = 500000
                    },
                      new MockupData
                    {
                        GateCode = "Tuyen_8",
                        GateName = "Ma Thiên Lãnh - Hang Đức Mẹ - Bãi Ông Đụng",
                        Price = 50000,
                        SlSale = 10,
                        AmountSale = 500000
                    },
                       new MockupData
                    {
                        GateCode = "Tuyen_9",
                        GateName = "Ma Thiên Lãnh - Hang Đức Mẹ - Đất Thắm - Bãi Bàng",
                        Price = 50000,
                        SlSale = 5,
                        AmountSale = 250000
                    }
                };
            ResGetRPSales ResSale = new ResGetRPSales()
            {
                SellQuantity = 45,
             
                TotalSales = 2100000,
                Data = mockupData
            };
            return ResSale;
        }


        [HttpGet]
        public List<ColumnChartModel> GetColumnChartValue(int year)
        {
            var res = reportService.GetColumnCharteport(year);
            return res;
        }

        public async Task<IActionResult> ReportPrintAgain()
        {
            return View();
        }

        public async Task<IActionResult> ReportSaleByCustType()
        {
            var searchModel = new SaleHistoryFilterModel
            {
                SaleChanelId = 0,
                UserName = "",
                FromDate = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy"),
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

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<DataTableResultModel<ReportBanVeByCustTypeGridModel>> GetReportSaleByCustType(SaleHistoryFilterModel filter)
        {
            var res = await reportService.BaoCaoBanVeTheoLoaiKH(filter,false);
            return res;
        }



        [HttpGet]
        public async Task<FileContentResult> ExportExcelSearchOrder(string filter)
        {
            var filterModel = JsonConvert.DeserializeObject<OrderFilterModel>(filter);
            var dataSearch = await ticketOrderService.SearchOrder(filterModel, true);



            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            Color colFromHex = ColorTranslator.FromHtml("#3377ff");
            Color colFromHexTextHeader = ColorTranslator.FromHtml("#ffffff");

            workSheet.Cells["A1"].Value = "Mã đơn";
            workSheet.Cells["B1"].Value = "Tên KH";
            workSheet.Cells["C1"].Value = "Loại KH";
            workSheet.Cells["D1"].Value = "Đối tượng";
            workSheet.Cells["E1"].Value = "Ngày mua";
            workSheet.Cells["F1"].Value = "Mã vé";
            workSheet.Cells["G1"].Value = "Đơn giá";
            workSheet.Cells["H1"].Value = "SL";
            workSheet.Cells["I1"].Value = "Thành tiền";
            workSheet.Cells["J1"].Value = "Km giảm tiền";
            workSheet.Cells["K1"].Value = "Tổng sau KM";
            workSheet.Cells["L1"].Value = "Người bán";


            workSheet.Cells[1, 1, 1, 12].Style.Font.Bold = true;
            int rowData = 2;
            int stt = 1;
            decimal sumTotalAfterDiscountd = 0;
            foreach (var item in dataSearch.data)
            {

                workSheet.Cells["A" + rowData].Value = item.Id;
                workSheet.Cells["B" + rowData].Value = item.CustomerName;
                workSheet.Cells["C" + rowData].Value = item.CustomerTypeName;
                workSheet.Cells["D" + rowData].Value = item.ObjName;
                workSheet.Cells["E" + rowData].Value = item.CreatedDate;
                workSheet.Cells["F" + rowData].Value = item.TicketCode;
                workSheet.Cells["G" + rowData].Value = item.Price;
                workSheet.Cells["H" + rowData].Value = item.Quanti;
                workSheet.Cells["I" + rowData].Value = item.Total;
                workSheet.Cells["J" + rowData].Value = item.DiscountedAmount;
                workSheet.Cells["K" + rowData].Value = item.TotalAfterDiscounted;
                workSheet.Cells["L" + rowData].Value = item.CreatedByName;

                sumTotalAfterDiscountd += item.TotalAfterDiscounted;
                rowData++;
                stt++;
            }

            workSheet.Cells["J" + rowData].Value = "Tổng";
            workSheet.Cells["K" + rowData].Value = sumTotalAfterDiscountd;
            workSheet.Cells[rowData, 10, rowData, 11].Style.Font.Bold = true;
            workSheet.Column(1).Width = 10;
            workSheet.Column(2).Width = 10;
            workSheet.Column(3).Width = 20;
            workSheet.Column(4).Width = 20;
            workSheet.Column(10).Width = 20;
            workSheet.Column(11).Width = 20;
            return File(excel.GetAsByteArray(), ExcelExportHelper.ExcelContentType, "DanhSachDonBanVe.xlsx");

        }




        [HttpGet]
        public async Task<FileContentResult> ExportExcelReportOrderDetail(string filter)
        {
            var filterModel = JsonConvert.DeserializeObject<OrderFilterModel>(filter);
            var dataSearch = await ticketOrderService.SearchOrder(filterModel, true);



            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            Color colFromHex = ColorTranslator.FromHtml("#3377ff");
            Color colFromHexTextHeader = ColorTranslator.FromHtml("#ffffff");

            workSheet.Cells["A1"].Value = "Mã đơn";
            workSheet.Cells["B1"].Value = "Tên KH";
            workSheet.Cells["C1"].Value = "Loại KH";
            workSheet.Cells["D1"].Value = "Đối tượng";
            workSheet.Cells["E1"].Value = "Ngày mua";
            workSheet.Cells["F1"].Value = "Mã vé";
            workSheet.Cells["G1"].Value = "Đơn giá";
            workSheet.Cells["H1"].Value = "SL";
            workSheet.Cells["I1"].Value = "Thành tiền";
            workSheet.Cells["J1"].Value = "Km giảm tiền";
            workSheet.Cells["K1"].Value = "Tổng sau KM";
            workSheet.Cells["L1"].Value = "Người bán";


            workSheet.Cells[1, 1, 1, 12].Style.Font.Bold = true;
            int rowData = 2;
            int stt = 1;
            decimal sumTotalAfterDiscountd = 0;
            foreach (var item in dataSearch.data)
            {

                workSheet.Cells["A" + rowData].Value = item.Id;
                workSheet.Cells["B" + rowData].Value = item.CustomerName;
                workSheet.Cells["C" + rowData].Value = item.CustomerTypeName;
                workSheet.Cells["D" + rowData].Value = item.ObjName;
                workSheet.Cells["E" + rowData].Value = item.CreatedDate;
                workSheet.Cells["F" + rowData].Value = item.TicketCode;
                workSheet.Cells["G" + rowData].Value = item.Price;
                workSheet.Cells["H" + rowData].Value = item.Quanti;
                workSheet.Cells["I" + rowData].Value = item.Total;
                workSheet.Cells["J" + rowData].Value = item.DiscountedAmount;
                workSheet.Cells["K" + rowData].Value = item.TotalAfterDiscounted;
                workSheet.Cells["L" + rowData].Value = item.CreatedByName;

                sumTotalAfterDiscountd += item.TotalAfterDiscounted;
                rowData++;
                stt++;
            }


            //Format table
            ExcelRange range = workSheet.Cells[1, 1, workSheet.Dimension.End.Row - 1, workSheet.Dimension.End.Column];
            ExcelTable tab = workSheet.Tables.Add(range, "Table1");
            tab.TableStyle = OfficeOpenXml.Table.TableStyles.Medium2;


            workSheet.Cells["J" + rowData].Value = "Tổng";
            workSheet.Cells["K" + rowData].Value = sumTotalAfterDiscountd;
            workSheet.Cells[rowData, 10, rowData, 11].Style.Font.Bold = true;
            workSheet.Column(1).Width = 10;
            workSheet.Column(2).Width = 10;
            workSheet.Column(3).Width = 20;
            workSheet.Column(4).Width = 20;
            workSheet.Column(10).Width = 20;
            workSheet.Column(11).Width = 20;
            return File(excel.GetAsByteArray(), ExcelExportHelper.ExcelContentType, "DanhSachDonBanVe.xlsx");

        }

        public async Task<IActionResult> ReportSaleByTicket()
        {
            var searchModel = new SaleReportFilter
            {
                UserName = "",
                FromDate = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                TicketCode = ""
            };
            ViewBag.LstUser = await userService.ListAllUserInfo();
            ViewBag.lstTicket = ticketService.GetAllTicketByUser(AuthenInfo().UserId);
            return View(searchModel);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<DataTableResultModel<ReportSaleByTicketGridModel>> GetReportSaleByTicket(SaleReportFilterModel filter)
        {
            filter.IsExcel = 0;
            var res = await reportService.GetReportSaleByTicket(filter);
            return res;
        }


        [HttpGet]
        public async Task<FileContentResult> ExportExcelReportSaleByTicket(string filter)
        {
            var filterModel = JsonConvert.DeserializeObject<SaleReportFilterModel>(filter);
            filterModel.IsExcel = 1;
            var dataSearch = await reportService.GetReportSaleByTicket(filterModel);



            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            Color colFromHex = ColorTranslator.FromHtml("#3377ff");
            Color colFromHexTextHeader = ColorTranslator.FromHtml("#ffffff");

            workSheet.Cells["A1"].Value = "Mã vé";
            workSheet.Cells["B1"].Value = "Mô tả";
            workSheet.Cells["C1"].Value = "Loại vé";
            workSheet.Cells["D1"].Value = "Tổng SL";
            workSheet.Cells["E1"].Value = "Tổng tiền";
            workSheet.Cells["F1"].Value = "Tiền KM";
            workSheet.Cells["G1"].Value = "Tổng sau KM";
  


            workSheet.Cells[1, 1, 1, 12].Style.Font.Bold = true;
            int rowData = 2;
            int stt = 1;
            decimal sumQuanti = 0;
            decimal sumTotal = 0;
            decimal sumKM = 0;
            decimal sumTotalAfterDiscountd = 0;
            foreach (var item in dataSearch.data)
            {

                workSheet.Cells["A" + rowData].Value = item.TicketCode;
                workSheet.Cells["B" + rowData].Value = item.Description;
                workSheet.Cells["C" + rowData].Value = item.TicketGroup;
                workSheet.Cells["D" + rowData].Value = item.TongSL;
                workSheet.Cells["E" + rowData].Value = item.TongTien;
                workSheet.Cells["F" + rowData].Value = item.TienKM;
                workSheet.Cells["G" + rowData].Value = item.TongTienSauKM;

                sumQuanti += item.TongSL;
                sumTotal += item.TongTien;
                sumKM += item.TienKM;
                sumTotalAfterDiscountd += item.TongTienSauKM;
                rowData++;
                stt++;
            }


            //Format table
            ExcelRange range = workSheet.Cells[1, 1, workSheet.Dimension.End.Row - 1, workSheet.Dimension.End.Column];
            ExcelTable tab = workSheet.Tables.Add(range, "Table1");
            tab.TableStyle = OfficeOpenXml.Table.TableStyles.Medium2;

            workSheet.Cells["D" + rowData].Value = sumQuanti;
            workSheet.Cells["E" + rowData].Value = sumTotal;
            workSheet.Cells["F" + rowData].Value = sumKM;
            workSheet.Cells["G" + rowData].Value = sumTotalAfterDiscountd;
            workSheet.Cells[rowData, 4, rowData, 11].Style.Font.Bold = true;
            workSheet.Column(1).Width = 10;
            workSheet.Column(2).Width = 10;
            workSheet.Column(3).Width = 20;
            workSheet.Column(4).Width = 20;
            workSheet.Column(10).Width = 20;
            workSheet.Column(11).Width = 20;
            return File(excel.GetAsByteArray(), ExcelExportHelper.ExcelContentType, "DanhSachDonBanVe.xlsx");

        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<DataTableResultModel<ReportInLaiModel>> GetReportPrintAgain(SaleReportFilterModel filter)
        {
            var res = await reportService.GetReportInLai(filter);
            return res;
        }

		[HttpPost]
		// [ValidateAntiForgeryToken]
		public async Task<DataTableResultModel<ReportInLaiModel>> GetReportPrintAgainByFilter([FromForm] PrintAgainRPFilter filter)
		{

			var res = await reportService.GetReportPrintAgainByFilter(filter);
			return res;
		}

	

	}
}