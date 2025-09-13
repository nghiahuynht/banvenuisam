using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using DAL.IService;
using DAL.Models;
using DAL.Models.Customer;
using DAL.Models.SoatVe;
using DAL.Models.Ticket;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using WebApp.Model;

namespace WebApp.Controllers
{
    public class SoatVeController : AppBaseController
    {
        private ISoatVeService soatVeService;
        private ITicketService ticketService;

        public SoatVeController(ISoatVeService soatVeService, ITicketService ticketService)
        {
            this.soatVeService = soatVeService;
            this.ticketService = ticketService;
        }

        public IActionResult Index()
        {
            ViewBag.GateList = soatVeService.GetGateDDLByUser(AuthenInfo().UserName);
            return View();
        }

        [HttpGet]
        public JsonResult ScanAction(Int64 ticketCode, string gateCode)
        {
            var res = soatVeService.UpdateScanResult(ticketCode, gateCode);
            return Json(res);
        }

        [HttpGet]
        public JsonResult DetailCheckTicketAction(string gateCode)
        {
            var res = soatVeService.DetailCheckTicketResult(gateCode);
            return Json(res);
        }


        public IActionResult SearchInOutHistory()
        {
            var modelSearch = new InOutFilterModel
            {
                FromDate = DAL.Helper.FirtDayOfMonth(),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy")

            };
            return View(modelSearch);
        }


        [HttpPost]
        public DataTableResultModel<HistoryInOutModel> SearchInOutPaging(InOutFilterModel filter)
        {
            var res = soatVeService.SearchInOutPaging(filter);
            return res;
        }

        //============================================== BÁO CÁO SOÁT VÉ QUA CỔNG =====================


        public IActionResult CheckinReport()
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
            return View(searchModel);
        }


        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<DataTableResultModel<ReportCheckinGridModel>> GetReportCheckin(SaleHistoryFilterModel filter)
        {
            var res = await soatVeService.ReportCheckin(filter, false);
            return res;
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<List<CheckinReportCounterModel>> GetReportCheckinCounter([FromBody]SaleHistoryFilterModel filter)
        {
            var res = await soatVeService.GetCounterReportCheckin(filter);
            return res;
        }

        [HttpGet]
        public async Task<FileContentResult> ExportGetReportCheckin(string filter)
        {
            var filterModel = !string.IsNullOrEmpty(filter) ? JsonConvert.DeserializeObject<SaleHistoryFilterModel>(filter) : new SaleHistoryFilterModel();

            var res = await soatVeService.ReportCheckin(filterModel, true);
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            Color colFromHex = ColorTranslator.FromHtml("#3377ff");
            Color colFromHexTextHeader = ColorTranslator.FromHtml("#ffffff");

            workSheet.Cells["A1"].Value = "Mã đơn";
            workSheet.Cells["B1"].Value = "Mã vé";
            workSheet.Cells["C1"].Value = "Mã bí mật";
            workSheet.Cells["D1"].Value = "Mã khách hàng";
            workSheet.Cells["E1"].Value = "Tên khách hàng";
            workSheet.Cells["F1"].Value = "Ngày vào cổng";
            workSheet.Cells[1, 1, 1, 6].Style.Font.Bold = true;
            int rowData = 2;
            foreach (var item in res.data)
            {

                workSheet.Cells["A" + rowData].Value = item.OrderId;
                workSheet.Cells["B" + rowData].Value = item.TicketCode;
                workSheet.Cells["C" + rowData].Value = item.ViettelCode;
                workSheet.Cells["D" + rowData].Value = item.CustomerCode;
                workSheet.Cells["E" + rowData].Value = item.CustomerName;
                workSheet.Cells["F" + rowData].Value = item.CheckinDate;
                rowData++;
            }
            workSheet.Cells["E" + rowData].Value = "Tổng cộng";
            workSheet.Cells["E" + rowData].Style.Font.Bold = true;
            workSheet.Cells["F" + rowData].Value = res.data.Count;
            workSheet.Cells["F" + rowData].Style.Font.Bold = true;

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
            return File(excel.GetAsByteArray(), ExcelExportHelper.ExcelContentType, "BaoCao-SoatVe.xlsx");
        }




    }
}