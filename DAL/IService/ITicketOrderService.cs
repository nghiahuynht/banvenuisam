using DAL.Models;
using DAL.Models.ConDao;
using DAL.Models.Ticket;
using DAL.Models.TicketOrder;
using DAL.Models.WebHookSePay;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface ITicketOrderService
    {
        List<DAL.Entities.TicketOrderSubNum> GetSubOrderCodeByOrderId(long orderId);
        SaveResultModel ChangePaymentStatusTicketOrder(long OrderId, int newStatus, string userName, string paymentValue = "", long paymentID = 0);
        Task<SaveResultModel> ChangeStatusTicketOrder(long OrderId, string newStatus, string userName);
        Task<DataTableResultModel<OrderGridModel>> SearchOrder(OrderFilterModel filter, bool isExcel);
        Task<List<SubOrderPrintModel>> GetSubCodePrintInfo(long orderId);
        PrintPdfOrderModel GetPrintPdfSubOrderDetail(long subid);
        List<PrintPdfOrderModel> GetListPrintPdfByOrderId(long orderId);
        ReportSaleCounterModel ReportSaleCounterModel(SaleReportFilterModel filter);
        TicketOrderModel CheckPayment(string Description, double Amount);

        Task<SaveResultModel> SaveTranSePayWebHook(WebHookReceiveModel model);
        ResultModel SaveOrderToData(PostOrderSaveModel model, string userName, string gateName);
        void AssignSubIdForMapping(Int64 orderId);
        SaveResultModel DeleteObjectOrder(long id, int deleteType, string userName);

        void SavePrintAgain(Int64 orderId,string userName, int quanti);
        ResultModel UpdateCustomerForTicketOrder(UpdateCustForOrderModel model);


    }
}
