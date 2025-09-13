using DAL.IService;
using DAL.Models;
using DAL.Models.Report;
using DAL.Models.Ticket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Infrastructure;

namespace DAL.Service
{
    public class ReportService: BaseService, IReportService
    {
        private EntityDataContext dtx;
        public ReportService(EntityDataContext dtx)
        {
            this.dtx = dtx;
        }
        /// <summary>
        /// lấy báo cáo doanh số bán theo loại vé
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<ResGetRPSales> GetReportSalesByTicketType(TicketTypeRPFilter filter)
        {
            var res = new ResGetRPSales();
            //try
            //{
            //    var param = new SqlParameter[] {
            //    new SqlParameter("@UserName", filter.UserName),
            //    new SqlParameter("@FromDate", filter.FromDate),
            //    new SqlParameter("@ToDate", filter.ToDate),
            //    new SqlParameter("@Keyword", filter.Keyword),
            //    new SqlParameter { ParameterName = "@TotalSL", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output },
            //    new SqlParameter { ParameterName = "@TotalAmount", DbType = System.Data.DbType.Decimal, Direction = System.Data.ParameterDirection.Output }
            //};
            //    ValidNullValue(param);
            //    var lstData = await dtx.TicketTypeRPGridModel.FromSql("EXEC Sp_ReportSaleByTickeType @UserName,@FromDate,@ToDate,@Keyword,@TotalSL OUT,@TotalAmount OUT", param)
            //        .ToListAsync();



            //    res.SellQuantity = Convert.ToInt32(param[param.Length - 2]?.Value ?? 0);
            //    res.TotalSales = Convert.ToDecimal(param[param.Length - 1]?.Value);
            //    if (lstData != null
            //        && lstData.Count == 1
            //        && lstData.Any(x => x.LoaiVe.ToLower().Equals("tổng")))
            //        res.Data = new List<TicketTypeRPGridModel>();
            //    else
            //        res.Data = lstData.ToList();
            //}
            //catch (Exception ex)
            //{
            //    WriteLog.writeToLogFile($"[Exception]: {ex}");
            //    res.SellQuantity = 0;
            //    res.TotalSales = 0;
            //    res.Data = new List<TicketTypeRPGridModel>();
            //}

            return null;
        }


        public List<StaffSaleCounterModel> ReportStaffSaleCounter(string dateView)
        {
            var res = new List<StaffSaleCounterModel>();
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@DateView",dateView),
                };
                res = dtx.StaffSaleCounterModel.FromSql("EXEC sp_GetSaleStaffInDay @DateView", param).ToList();

            }
            catch (Exception ex)
            {

            }
            return res;
        }




        public List<SaleTicketMisaStatusModel> ReportTicketMisaStatus(string fromDate, string toDate)
        {
            var res = new List<SaleTicketMisaStatusModel>();
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@FromDate",fromDate),
                    new SqlParameter("@ToDate",toDate),
                };
                
                res = dtx.SaleTicketMisaStatusModel.FromSql("EXEC sp_GetTicketMisaStatus @FromDate,@ToDate", param).ToList();
                


            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public List<ColumnChartModel> GetColumnCharteport(int year)
        {
            var resData = new List<ColumnChartModel>();
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@Year",year)
                };
                resData = dtx.ColumnChartModel.FromSql("EXEC sp_GetColumnChartDashBoar @Year", param).ToList();
               

            }
            catch (Exception ex)
            {
                resData = new List<ColumnChartModel>();
            }
            return resData;
        }


        /// <summary>
        /// lấy báo cáo doanh số bán theo đối tác
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<ResGetRPByPartner> GetReportSalesByPartner(TicketTypeRPFilter filter)
        {
            var res = new ResGetRPByPartner();
            try
            {
                DateTime _fromDate;
                DateTime.TryParseExact(filter.FromDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
                DateTime _toDate;
                DateTime.TryParseExact(filter.ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _toDate);


                var param = new SqlParameter[] {
                new SqlParameter("@PartnerCode", filter.UserName),
                new SqlParameter("@FromDate", _fromDate),
                new SqlParameter("@ToDate", _toDate),
                new SqlParameter { ParameterName = "@TotalNumberSale", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output },
                    new SqlParameter { ParameterName = "@TotalPrice", DbType = System.Data.DbType.Decimal, Direction = System.Data.ParameterDirection.Output }
            };
                ValidNullValue(param);
                var lstData = await dtx.SaleReportByPartnerGridModel.FromSql("EXEC sp_GetDataReportPartner @PartnerCode,@FromDate,@ToDate,@TotalNumberSale OUT,@TotalPrice OUT", param).ToListAsync();
                res.TongDoanhSo = Convert.ToDecimal(param[param.Length - 1].Value);
                res.SLBan = Convert.ToInt32(param[param.Length - 2].Value);
                res.Data = lstData.ToList();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.SLBan = 0;
                res.TongDoanhSo = 0;
                res.Data = new List<SaleReportByPartnerGridModel>();
            }

            return res;
        }



        public async Task<DataTableResultModel<ReportBanVeByCustTypeGridModel>> BaoCaoBanVeTheoLoaiKH(SaleHistoryFilterModel filter,bool isExcel)
        {
            var res = new DataTableResultModel<ReportBanVeByCustTypeGridModel>();
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@SaleChanelId", filter.SaleChanelId),
                new SqlParameter("@UserName", filter.UserName),
                new SqlParameter("@TicketCode", filter.TicketCode),
                new SqlParameter("@FromDate", filter.FromDate),
                new SqlParameter("@ToDate", filter.ToDate),
                new SqlParameter("@GateCode", filter.GateCode),
                new SqlParameter("@Start", filter.start),
                new SqlParameter("@Length",filter.length),
                new SqlParameter("@IsExcel", isExcel),
                new SqlParameter("@PaymentType", filter.PaymentType),
                new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int64, Direction = System.Data.ParameterDirection.Output }
            };
                ValidNullValue(param);
                var lstData = await dtx.ReportBanVeByCustTypeGridModel.FromSql("EXEC sp_ReportSaleByCustType @SaleChanelId,@UserName,@TicketCode,@FromDate,@ToDate,@GateCode,@Start,@Length,@IsExcel,@PaymentType,@TotalRow OUT", param).ToListAsync();
                res.recordsTotal = Convert.ToInt32(param[param.Length - 1].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData.ToList();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<ReportBanVeByCustTypeGridModel>();
            }

            return res;

        }


        public async Task<DataTableResultModel<ReportSaleByTicketGridModel>> GetReportSaleByTicket(SaleReportFilterModel filter)
        {
            var res = new DataTableResultModel<ReportSaleByTicketGridModel>();
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@UserName", filter.UserName),
                new SqlParameter("@FromDate", filter.FromDate),
                new SqlParameter("@ToDate", filter.ToDate),
                new SqlParameter("@Start", filter.start),
                new SqlParameter("@Length", filter.length),
                new SqlParameter("@IsExcel", filter.IsExcel),
                new SqlParameter("@PaymentType", filter.PaymentType),
                new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int16, Direction = System.Data.ParameterDirection.Output }
            };
                ValidNullValue(param);
                var lstData = await dtx.ReportSaleByTicketGridModel.FromSql("EXEC sp_ReportSaleByTicket @UserName,@FromDate,@ToDate,@Start,@Length,@IsExcel,@PaymentType,@TotalRow OUT", param).ToListAsync();
                res.recordsTotal = Convert.ToInt16(param[param.Length - 1].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData.ToList();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<ReportSaleByTicketGridModel>();
            }

            return res;
        }



        public async Task<DataTableResultModel<ReportInLaiModel>> GetReportInLai(SaleReportFilterModel filter)
        {
            var res = new DataTableResultModel<ReportInLaiModel>();
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@UserName", filter.UserName),
                new SqlParameter("@FromDate", filter.FromDate),
                new SqlParameter("@ToDate", filter.ToDate),
                new SqlParameter("@Keyword",filter.Keyword),
                new SqlParameter("@Start", filter.start),
                new SqlParameter("@Length", filter.length),
                new SqlParameter("@IsExcel", filter.IsExcel),
                new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int16, Direction = System.Data.ParameterDirection.Output }
            };
                ValidNullValue(param);
                var lstData = await dtx.ReportInLaiModel.FromSql("EXEC sp_ReportPrintAgain @UserName,@FromDate,@ToDate,@Keyword,@Start,@Length,@IsExcel,@TotalRow OUT", param).ToListAsync();
                res.recordsTotal = Convert.ToInt32(param[param.Length - 1].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData.ToList();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<ReportInLaiModel>();
            }

            return res;
        }

		public async Task<DataTableResultModel<ReportInLaiModel>> GetReportPrintAgainByFilter(PrintAgainRPFilter filter)
		{
			var res = new DataTableResultModel<ReportInLaiModel>();
			try
			{
				var param = new SqlParameter[] {
				//new SqlParameter("@UserName", filter.UserName),
			new SqlParameter("@FromDate", DateTime.ParseExact(filter.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
new SqlParameter("@ToDate", DateTime.ParseExact(filter.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                new SqlParameter("@PaymentType", filter.PaymentType),
                new SqlParameter("@Start", filter.start),
				new SqlParameter("@Length", filter.length),

				new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int16, Direction = System.Data.ParameterDirection.Output }
			};
				ValidNullValue(param);
				var lstData = await dtx.ReportInLaiModel.FromSql("EXEC sp_GetPrintAgainHistoryByFilter @FromDate,@ToDate,@Start,@Length,@PaymentType,@TotalRow OUT", param).ToListAsync();
				res.recordsTotal = Convert.ToInt32(param[param.Length - 1].Value);
				res.recordsFiltered = res.recordsTotal;
				res.data = lstData.ToList();
			}
			catch (Exception ex)
			{
				WriteLog.writeToLogFile($"[Exception]: {ex}");
				res.recordsTotal = 0;
				res.recordsFiltered = 0;
				res.data = new List<ReportInLaiModel>();
			}

			return res;
		}
	}
}
