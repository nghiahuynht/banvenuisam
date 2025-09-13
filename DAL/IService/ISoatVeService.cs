using DAL.Models;
using DAL.Models.GatePermission;
using DAL.Models.SoatVe;
using DAL.Models.Ticket;
using DAL.Models.TicketOrder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface ISoatVeService
    {
        List<ComboBoxModel> GetGateDDL();
        ScanResultModel UpdateScanResult(Int64 subId, string zoneCode);
        DetailCheckTicketModel DetailCheckTicketResult(string zoneCode);
        List<GateListModel> GetAllGateFullInfo();
        List<ComboBoxModel> GetGateDDLByUser(string userName);
        DetailCheckTicketModel ReportSoatVeMobile(string userName, string zoneCode, DateTime dateScan);
        // ddphuong:28/02/2024 get gate by parent code
        List<GateListModel> GetGateByParentCode(string  parentCode);

        List<ResultSoatVeOfflineModel> SoatVeOffline(List<SoatVeOfflineModel> data);
        DataTableResultModel<HistoryInOutModel> SearchInOutPaging(InOutFilterModel filter);
        Task<DataTableResultModel<ReportCheckinGridModel>> ReportCheckin(SaleHistoryFilterModel filter, bool isExcel);
        Task<List<CheckinReportCounterModel>> GetCounterReportCheckin(SaleHistoryFilterModel filter);
    }
}
