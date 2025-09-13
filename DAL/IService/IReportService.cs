using DAL.Models;
using DAL.Models.Report;
using DAL.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface IReportService
    {
        /// <summary>
        /// Lấy báo cáo doanh số bán theo loại vé
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<ResGetRPSales> GetReportSalesByTicketType(TicketTypeRPFilter filter);

        List<StaffSaleCounterModel> ReportStaffSaleCounter(string dateView);
        List<SaleTicketMisaStatusModel> ReportTicketMisaStatus(string fromDate, string toDate);

        List<ColumnChartModel> GetColumnCharteport(int year);
        Task<ResGetRPByPartner> GetReportSalesByPartner(TicketTypeRPFilter filter);
        Task<DataTableResultModel<ReportBanVeByCustTypeGridModel>> BaoCaoBanVeTheoLoaiKH(SaleHistoryFilterModel filter, bool isExcel);
        Task<DataTableResultModel<ReportSaleByTicketGridModel>> GetReportSaleByTicket(SaleReportFilterModel filter);
        Task<DataTableResultModel<ReportInLaiModel>> GetReportInLai(SaleReportFilterModel filter);
        Task<DataTableResultModel<ReportInLaiModel>> GetReportPrintAgainByFilter(PrintAgainRPFilter filter);
    }
}
