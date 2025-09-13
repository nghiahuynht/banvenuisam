using DAL.Entities;
using DAL.IService;
using DAL.Models;
using DAL.Models.Report;
using DAL.Models.Ticket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Infrastructure;

namespace DAL.Service
{
    public class TicketService : BaseService, ITicketService
    {
        private EntityDataContext dtx;
        public TicketService(EntityDataContext dtx)
        {
            this.dtx = dtx;
        }
        /// <summary>
        /// Lấy danh sách chi nhánh
        /// </summary>
        /// <returns></returns>
        public List<Area> GetAllArea()
        {
            var res = new List<Area>();
            try
            {
                res = dtx.Area.OrderBy(s => s.Name).ToList();
            }
            catch(Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                return res;
            }
            return res;
        }
        /// <summary>
        /// Lấy danh sách loại vé
        /// </summary>
        /// <returns></returns>
        public List<LoaiIn> GetAllLoaiIn()
        {
            var res = new List<LoaiIn>();
            try
            {
                res = dtx.LoaiIn.Where(s => !s.IsDeleted).ToList();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                return res;
            }
            return res;
        }
        /// <summary>
        /// Get thông tin ticket theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Ticket GetTicketbyId(int id)
        {
            var res = new Ticket();
            try
            {
                res = dtx.Ticket.FirstOrDefault(s => s.Id == id && !s.IsDeleted);
            }catch(Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                return res;
            }
            return res;
        }
        /// <summary>
        /// Get thông tin ticket sub order theo orderId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<TicketOrderSubNum>GetSubOrderByOrderId(long orderId)
        {
            var res = new List<TicketOrderSubNum>();
            try
            {
                res = dtx.TicketOrderSubNum.Where(s => s.OrderId == orderId).ToList();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                return res;
            }
            return res;
        }
        public TicketOrder GetTicketOrderbyId(long orderId)
        {
            var res = new TicketOrder();
            try
            {
                res = dtx.TicketOrder.FirstOrDefault(s => s.Id == orderId);
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                return res;
            }
            return res;
        }
        /// <summary>
        /// Lấy thông tin vé theo Code
        /// </summary>
        /// <param name="ticketCode"></param>
        /// <returns></returns>
        public Ticket GetTicketbyCode(string ticketCode)
        {
            var res = new Ticket();
            try
            {
                res = dtx.Ticket.FirstOrDefault(s => s.Code == ticketCode && !s.IsDeleted);
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                return res;
            }
            return res;
        }

        /// <summary>
        /// danh sách thông tin vé
        /// </summary>
        /// <param name="ticketCode"></param>
        /// <returns></returns>
        public List<Ticket> GetAllTicket()
        {
            var res = new List<Ticket>();
            try
            {
                res = dtx.Ticket.Where(s => !s.IsDeleted).ToList();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                return res;
            }
            return res;
        }

        /// <summary>
        /// Tạo thông tin vé
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<SaveResultModel> CreateTicket(Ticket ticket, string userName)
        {
            var res = new SaveResultModel();
            try
            {
                ticket.CreatedBy = userName;
                ticket.CreatedDate = DateTime.Now;
                await dtx.Ticket.AddAsync(ticket);
                await dtx.SaveChangesAsync();
                res.ValueReturn = ticket.Id;

            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }

            return res;
        }
        /// <summary>
        /// Cập nhật thông tin vé
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<SaveResultModel> UpdateTicket(Ticket ticket, string userName)
        {
            var res = new SaveResultModel();
            try
            {
                ticket.UpdatedBy = userName;
                ticket.UpdatedDate = DateTime.Now;
                dtx.Ticket.Update(ticket);
                await dtx.SaveChangesAsync();
                res.IsSuccess = true;
                res.ValueReturn = ticket.Id;
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        /// <summary>
        /// Cập nhật thông tin DH
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<ResCommon<long>> UpdateTicketOrder(TicketOrder ticketOD, string userName)
        {
            var res = new ResCommon<long>();
            try
            {
                ticketOD.UpdatedBy = userName;
                ticketOD.UpdatedDate = DateTime.Now;
                dtx.TicketOrder.Update(ticketOD);
                await dtx.SaveChangesAsync();
                res.IsSuccess = true;
                res.Data = ticketOD.Id;
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.IsSuccess = false;
                res.Desc = ex.Message;
            }
            return res;
        }
        /// <summary>
        /// tìm thông tin vé
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DataTableResultModel<TicketGridModel> SearchTicket(TicketFilterModel filter)
        {
            var res = new DataTableResultModel<TicketGridModel>();
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@Area", filter.Area),
                new SqlParameter("@Keyword", filter.Keyword),
                new SqlParameter("@Start", filter.start),
                new SqlParameter("@Length", filter.length),
                new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int16, Direction = System.Data.ParameterDirection.Output }
            };
                ValidNullValue(param);
                var lstData = dtx.TicketGridModel.FromSql("SearchTicketByCode @Area,@Keyword,@Start,@Length,@TotalRow OUT", param).ToList();
                res.recordsTotal = Convert.ToInt16(param[4].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData.ToList();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<TicketGridModel>();
            }

            return res;
        }
        /// <summary>
        /// Lấy danh sách vé theo loại vé
        /// </summary>
        /// <param name="typeTicket"></param>
        /// <returns></returns>
        public async Task<List<Ticket>> GetTicketsByType(string typeTicket)
        {
            var objTicket = dtx.Ticket.Where(s => s.LoaiIn == typeTicket && !s.IsDeleted).ToList() ;
            return objTicket;
        }
        /// <summary>
        /// Xóa vé
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<SaveResultModel> DeleteTicket(int ticketId, string userName)
        {
            var res = new SaveResultModel();
            try
            {
                var ticket = await dtx.Ticket.FirstOrDefaultAsync(x => x.Id == ticketId);
                ticket.IsDeleted = true;
                ticket.UpdatedBy = userName;
                ticket.UpdatedDate = DateTime.Now;
                dtx.Ticket.Update(ticket);
                await dtx.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        /// <summary>
        /// Thông tin đặt vé
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loaiInCode"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<ResultModel> CreateTicketOrder(TicketOrder model, string loaiInCode, string userName)
        {
            var res = new ResultModel();
            try
            {
                #region Lưu thông tin đặt vé
                model.CreatedBy = userName;
                model.CreatedDate = DateTime.Now;
                await dtx.TicketOrder.AddAsync(model);
                await dtx.SaveChangesAsync();


                res.IsSuccess = true;
                res.ValueReturn = model.Id;

                #endregion

                //#region Lưu chi tiết vé
                //if (loaiInCode == Contanst.LoaiIn_VeDoan)
                //{
                //    var subOrder = new TicketOrderSubNum()
                //    {
                //        OrderId = model.Id,
                //        SubNum = model.Quanti,
                //        TicketCode = model.TicketCode,
                //        Price = model.Price,
                //        TotalAfterVAT = model.Total
                //    };
                //    res = await CreateTicketSubOrder(subOrder);
                //}
                //else// vé lẻ
                //{
                //    for (int i = 0; i < model.Quanti; i++)
                //    {
                //        var subOrder = new TicketOrderSubNum()
                //        {
                //            OrderId = model.Id,
                //            SubNum = i + 1,
                //            TicketCode = model.TicketCode,
                //            Price = model.Price,
                //            TotalAfterVAT = model.Price
                //        };
                //        res = await CreateTicketSubOrder(subOrder);
                //    }
                //}





                // #endregion

            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        /// <summary>
        /// Chi tiết vé
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel CreateTicketSubOrder(long orderId,int quanti,string ticketCode,decimal price)
        {
            var res = new ResultModel();
            try
            {
                var transaction = dtx.Database.BeginTransaction();
                var param = new SqlParameter[] {
                    new SqlParameter("@OrderId",orderId),
                    new SqlParameter("@Quanti", quanti),
                    new SqlParameter("@TicketCode", ticketCode),
                    new SqlParameter("@Price", price),
                };
                ValidNullValue(param);
                dtx.Database.ExecuteSqlCommand(@"EXEC sp_CreateSubNumOrderForScan @OrderId,@Quanti,@TicketCode,@Price", param);
                res.IsSuccess = true;
                transaction.Commit();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception insert suborder]: {ex}");
            }
            return res;
        }

       
        /// <summary>
        /// lấy danh sách lịch sử bán
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<DataTableResultModel<SaleHistoryGridModel>> GetSaleHistory(SaleHistoryFilterModel filter,bool isExcel)
        {
            var res = new DataTableResultModel<SaleHistoryGridModel>();
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@SaleChanelId", filter.SaleChanelId),
                new SqlParameter("@UserName", filter.UserName),
                new SqlParameter("@TicketCode", filter.TicketCode),
                new SqlParameter("@FromDate", filter.FromDate),
                new SqlParameter("@ToDate", filter.ToDate),
                new SqlParameter("@GateCode", filter.GateCode),
                new SqlParameter("@Keyword", filter.Keyword),
                new SqlParameter("@Start", filter.start),
                new SqlParameter("@Length",filter.length),
                new SqlParameter("@PaymentType", filter.PaymentType),
                new SqlParameter("@IsExcel", isExcel),
                new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int16, Direction = System.Data.ParameterDirection.Output }
            };
                ValidNullValue(param);
                var lstData = await dtx.SaleHistoryGridModel.FromSql("EXEC sp_SearchSaleHistoryWeb @SaleChanelId,@UserName,@TicketCode,@FromDate,@ToDate,@GateCode,@Keyword,@Start,@Length,@IsExcel,@PaymentType,@TotalRow OUT", param).ToListAsync();
                res.recordsTotal = Convert.ToInt16(param[param.Length - 1].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData.ToList();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<SaleHistoryGridModel>();
            }

            return res;
        }

        /// <summary>
        /// Báo cáo bán hàng
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<DataTableResultModel<SaleReportGridModel>> GetSaleReport(SaleReportFilterModel filter)
        {
            var res = new DataTableResultModel<SaleReportGridModel>();
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@SaleChanelId", filter.SaleChanelId),
                new SqlParameter("@GateCode", filter.GateCode),
                new SqlParameter("@UserName", filter.UserName),
                new SqlParameter("@TicketCode", filter.TicketCode),
                new SqlParameter("@FromDate", filter.FromDate),
                new SqlParameter("@ToDate", filter.ToDate),
                new SqlParameter("@Start", filter.start),
                new SqlParameter("@Length", filter.length),
                new SqlParameter("@IsExcel", filter.IsExcel),
                new SqlParameter("@PaymentType", filter.PaymentType),
                new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int16, Direction = System.Data.ParameterDirection.Output }
            };
                ValidNullValue(param);
                var lstData = await dtx.SaleReportGridModel.FromSql("EXEC sp_ReportSaleByUserWeb @SaleChanelId,@GateCode,@UserName,@TicketCode,@FromDate,@ToDate,@Start,@Length,@IsExcel,@PaymentType,@TotalRow OUT", param).ToListAsync();
                res.recordsTotal = Convert.ToInt16(param[param.Length - 1].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData.ToList();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<SaleReportGridModel>();
            }

            return res;
        }



       


        /// <summary>
        /// lấy thông tin đơn hàng cần thanh toán
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ResOrderInfoDto GetOrderInfo(long orderId)
        {
            var res = new ResOrderInfoDto();
            try
            {
                var transaction = dtx.Database.BeginTransaction();
                var param = new SqlParameter[] {
                new SqlParameter("@OrderId", orderId),
                };
                ValidNullValue(param);
                res = dtx.ResOrderInfoDto.FromSql("EXEC sp_GetOrderInfo @OrderId", param).FirstOrDefault();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res = new ResOrderInfoDto();
            }

            return res;
        }
        /// <summary>
        /// Lấy danh sách địa điểm
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<GateList> GetAllGatelist()
        {
            
            try
            {
                var res = dtx.GateList.ToList();
                return res;
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                return new List<GateList>();
            }

        }
        /// <summary>
        /// Báo cáo soát vé
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<DataTableResultModel<SoatVeReportGridModel>> GetSoatVeReport(SoatveReportFilter filter)
        {
            var res = new DataTableResultModel<SoatVeReportGridModel>();
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@FromDate", filter.FromDate),
                new SqlParameter("@ToDate", filter.ToDate),
                new SqlParameter("@Zone", filter.ZoneCode),
                new SqlParameter("@Gate", filter.GateCode),
                new SqlParameter("@Keyword", filter.Keyword),
                new SqlParameter("@Vaildscan", filter.StatusScan),
                 new SqlParameter("@Start", filter.start),
                new SqlParameter("@Length",filter.length),
                new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int16, Direction = System.Data.ParameterDirection.Output }
            };
                ValidNullValue(param);
                var lstData = await dtx.SoatVeReportGridModel.FromSql("EXEC sp_ReportSoatVe @FromDate,@ToDate,@Zone,@Gate,@Keyword,@Vaildscan,@Start,@Length,@TotalRow OUT", param).ToListAsync();
                res.recordsTotal = Convert.ToInt16(param[param.Length - 1].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData.ToList();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<SoatVeReportGridModel>();
            }

            return res;
        }


        /// <summary>
        /// get ticket order theo paymentId
        /// </summary>
        /// <param name="ticketCode"></param>
        /// <returns></returns>
        public TicketOrder GetTicketOderByPaymentId(long paymentId)
        {
            var res = new TicketOrder();
            try
            {
                res = dtx.TicketOrder.Where(s => s.PaymentId == paymentId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                return res;
            }
            return res;
        }

        /// <summary>
        /// lấy thông tin đơn hàng cần thanh toán để gửi zalo
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ResOrderInfoSendZaloDto GetOrderInfoSendZalo(long orderId)
        {
            var res = new ResOrderInfoSendZaloDto();
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@OrderId", orderId),
                };
                ValidNullValue(param);
                res = dtx.ResOrderInfoSendZaloDto.FromSql("EXEC GetOrderInfoSendZalo @OrderId", param).FirstOrDefault();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res = new ResOrderInfoSendZaloDto();
            }

            return res;
        }

        public Ticket GetTicketByGate(string printType)
        {
            var res = new Ticket();
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@PrintType", printType),
                };
                ValidNullValue(param);
                res = dtx.Ticket.FromSql("EXEC sp_GetTicketByGate @PrintType", param).FirstOrDefault();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res = new Ticket();
            }

            return res;
        }


        public List<TicketGroupModel> GetTicketGroupDDL()
        {
            var res = new List<TicketGroupModel>();
            try
            {
                var param = new SqlParameter[] {
                   
                };
                ValidNullValue(param);
                res = dtx.TicketGroupModel.FromSql("EXEC sp_GetTicketGroupDDL", param).ToList();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res = new List<TicketGroupModel>();
            }

            return res;
        }

        public bool CreateTicketUser(string userId, string ticketId, string userName)
        {
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@TicketId", ticketId),
                new SqlParameter("@CreatedBy", userName)
            };
                ValidNullValue(param);
                var lstData =  dtx.Database.ExecuteSqlCommand("EXEC sp_CreateTicketWithUser @UserId,@TicketId,@CreatedBy", param);
                return true;
                
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public bool DeleteTicketUser(string userId)
        {
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@UserId", userId),

            };
                ValidNullValue(param);
                var lstData = dtx.Database.ExecuteSqlCommand("EXEC sp_DelTicketWithUserByUserId @UserId", param);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<TicketUserModel> GetTicketByUser(int userId)
        {

            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@UserId", userId),

            };
                ValidNullValue(param);
                var lstData = dtx.TicketUserModel.FromSql("EXEC sp_GetTicketByUserId @UserId", param).ToList();
                return lstData;
                

            }
            catch (Exception ex)
            {
                return new List<TicketUserModel>() ;
            }

        }

        public List<Ticket> GetAllTicketByUser(int userId)
        {
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@UserId", userId),

            };
                ValidNullValue(param);
                var lstData = dtx.Ticket.FromSql("EXEC sp_GetAllTicketByUserId @UserId", param).ToList();
                return lstData;


            }
            catch (Exception ex)
            {
                return new List<Ticket>();
            }
        }
    }
}
