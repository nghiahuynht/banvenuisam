using DAL.IService;
using DAL.Models.Report;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardMobileController : Controller
    {
        private IReportService reportService;
        public DashboardMobileController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet("[action]")]
        public List<ColumnChartModel> GetColumnChartValue(int year)
        {
            var res = reportService.GetColumnCharteport(year);
            return res;
        }

        [HttpGet("[action]")]
        public List<StaffSaleCounterModel> StaffSaleCounter(string dateView)
        {
            var res = reportService.ReportStaffSaleCounter(dateView);
            return res;
        }

        [HttpGet("[action]")]
        public List<SaleTicketMisaStatusModel> TicketSaleMisaStatus(string fromDate, string toDate)
        {
            var res = reportService.ReportTicketMisaStatus(fromDate, toDate);
            return res;
        }
    }
}
