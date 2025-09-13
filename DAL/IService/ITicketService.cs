using DAL.Entities;
using DAL.Models;
using DAL.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface ITicketService
    {
        /// <summary>
        /// Load tất cả chi nhánh/ địa điểm
        /// </summary>
        /// <returns></returns>
        List<Area> GetAllArea();
        /// <summary>
        /// Get thông tin ticket theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Ticket GetTicketbyId(int id);
        /// <summary>
        /// Lấy thông tin vé theo Code
        /// </summary>
        /// <param name="ticketCode"></param>
        /// <returns></returns>
        Ticket GetTicketbyCode(string ticketCode);
        /// <summary>
        /// lấy thông tin đơn hàng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        TicketOrder GetTicketOrderbyId(long orderId);
        /// <summary>
        /// thông tin chi tiết vé
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<SaveResultModel> CreateTicket(Ticket ticket, string userName);
        /// <summary>
        /// Cập nhật thông tin vé
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<SaveResultModel> UpdateTicket(Ticket ticket, string userName);
        /// <summary>
        /// Search ticket
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        DataTableResultModel<TicketGridModel> SearchTicket(TicketFilterModel filter);
        /// <summary>
        /// Xóa ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<SaveResultModel> DeleteTicket(int ticketId, string userName);
        /// <summary>
        /// lấy danh sách loại in
        /// </summary>
        /// <returns></returns>
        List<LoaiIn> GetAllLoaiIn();

        /// <summary>
        /// Chi tiết vé
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel CreateTicketSubOrder(long orderId, int quanti, string ticketCode, decimal price);
        /// <summary>
        /// Thông tin đặt vé
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loaiInCode"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ResultModel> CreateTicketOrder(TicketOrder model, string loaiInCode, string userName);
        /// <summary>
        /// Lấy danh sách thông tin vé
        /// </summary>
        /// <returns></returns>
        List<Ticket> GetAllTicket();
        /// <summary>
        /// Lấy danh sách bán hàng
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<DataTableResultModel<SaleHistoryGridModel>> GetSaleHistory(SaleHistoryFilterModel filter, bool isExcel);
        /// <summary>
        /// Báo cáo bán hàng
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<DataTableResultModel<SaleReportGridModel>> GetSaleReport(SaleReportFilterModel filter);
        /// <summary>
        /// Cập nhật thông tin ĐH
        /// </summary>
        /// <param name="ticketOD"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ResCommon<long>> UpdateTicketOrder(TicketOrder ticketOD, string userName);
        /// <summary>
        /// Lấy danh sách vé theo loại vé
        /// </summary>
        /// <param name="typeTicket"></param>
        /// <returns></returns>
        Task<List<Ticket>> GetTicketsByType(string typeTicket);
        /// <summary>
        /// lấy thông tin đơn hàng cần thanh toán
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        ResOrderInfoDto GetOrderInfo(long orderId);
        /// <summary>
        /// lấy danh sách địa điểm
        /// </summary>
        /// <returns></returns>
        List<GateList> GetAllGatelist();
        /// <summary>
        /// Báo cáo soát vé
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<DataTableResultModel<SoatVeReportGridModel>> GetSoatVeReport(SoatveReportFilter filter);
        /// <summary>
        /// Get thông tin ticket sub order theo orderId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       List<TicketOrderSubNum> GetSubOrderByOrderId(long orderId);

        TicketOrder GetTicketOderByPaymentId(long paymentId);

        ResOrderInfoSendZaloDto GetOrderInfoSendZalo(long orderId);
        Ticket GetTicketByGate(string printType);
        List<TicketGroupModel> GetTicketGroupDDL();

        bool  CreateTicketUser(string userId, string ticketId,string userName);
        bool  DeleteTicketUser(string userId);
        List<TicketUserModel> GetTicketByUser(int UserId);
        List<Ticket> GetAllTicketByUser(int UserId);
    }
}
