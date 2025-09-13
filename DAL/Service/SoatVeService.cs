using DAL.IService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DAL.Models.TicketOrder;
using DAL.Models.Ticket;
using DAL.Models.GatePermission;
using WebApp.Infrastructure;
using DAL.Models.SoatVe;
using System.Threading.Tasks;

namespace DAL.Service
{
    public class SoatVeService:BaseService, ISoatVeService
    {
        private EntityDataContext dtx;
        public SoatVeService(EntityDataContext dtx)
        {
            this.dtx = dtx;
        }


        public List<ComboBoxModel> GetGateDDL()
        {
            var res = new List<ComboBoxModel>();
            try
            {
                var param = new SqlParameter[] {};
                ValidNullValue(param);
                res = dtx.ComboBoxModel.FromSql("EXEC sp_GetAllGateList", param).ToList();

            }
            catch (Exception ex)
            {

            }

            return res;
        }

        public List<ComboBoxModel> GetGateDDLByUser(string userName)
        {
            var res = new List<ComboBoxModel>();
            try
            {
                var param = new SqlParameter[] {
                     new SqlParameter("@UserName",userName),
                };
                ValidNullValue(param);
                res = dtx.ComboBoxModel.FromSql("EXEC sp_GetGateListByUser @UserName", param).ToList();

            }
            catch (Exception ex)
            {

            }

            return res;
        }



        public List<GateListModel> GetAllGateFullInfo()
        {
            var res = new List<GateListModel>();
            try
            {
                var param = new SqlParameter[] { };
                ValidNullValue(param);
                res = dtx.GateListModel.FromSql("EXEC sp_GetAllGateListFullInfo", param).ToList();

            }
            catch (Exception ex)
            {

            }

            return res;
        }

        public ScanResultModel UpdateScanResult(Int64 subId, string zoneCode)
        {
            var resReturn = new ScanResultModel();
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter("@SubId",subId),
                         new SqlParameter("@GateCode",zoneCode)
                };


                var result = dtx.ScanResultModel.FromSql("EXEC sp_UpdateScanResultNew @SubId,@GateCode", param).FirstOrDefault();

                if (result == null)
                {
                    resReturn.Error = "Vé không tồn tại trên hệ thống";
                }
                else
                {
                    resReturn = result;
                }
            }
            catch (Exception ex)
            {
                resReturn.TicketCode = subId.ToString();
                resReturn.Error = ex.Message;
            }
            return resReturn;
        }



        public DetailCheckTicketModel DetailCheckTicketResult(string zoneCode)
        {
            var resReturn = new DetailCheckTicketModel();
            resReturn.TotalCheck = 0;
            resReturn.TotalSuccess = 0;
            resReturn.TotalFail = 0;
            try
            {
                var param = new SqlParameter[] {
                         new SqlParameter("@zoneCode",zoneCode)
                };


                resReturn = dtx.DetailCheckTicketModel.FromSql("EXEC DetailCheckTicket @zoneCode", param).FirstOrDefault();

              
            }
            catch (Exception ex)
            {
               
            }
            return resReturn;
        }




        public DetailCheckTicketModel ReportSoatVeMobile(string userName,string zoneCode,DateTime dateScan)
        {
            var resReturn = new DetailCheckTicketModel();
            resReturn.TotalCheck = 0;
            resReturn.TotalSuccess = 0;
            resReturn.TotalFail = 0;
            try
            {
                var param = new SqlParameter[] {
                         new SqlParameter("@ZoneCode",zoneCode),
                         new SqlParameter("@UserName",userName),
                         new SqlParameter("@Date",dateScan),
                };


                resReturn = dtx.DetailCheckTicketModel.FromSql("EXEC sp_ReportSoatVeMobile @ZoneCode,@UserName,@Date", param).FirstOrDefault();


            }
            catch (Exception ex)
            {

            }
            return resReturn;

        }

        public List<GateListModel> GetGateByParentCode(string parentCode)
        {
            var res = new List<GateListModel>();
            try
            {
                var param = new SqlParameter[] {
                     new SqlParameter("@ParentCode",parentCode),
                };
                ValidNullValue(param);
                res = dtx.GateListModel.FromSql("EXEC sp_GetGateListByParentCode @ParentCode", param).ToList();

            }
            catch (Exception ex)
            {

            }

            return res;
        }

        public List<ResultSoatVeOfflineModel> SoatVeOffline(List<SoatVeOfflineModel> data)
        {
            var resReturn = new List<ResultSoatVeOfflineModel>();
            //resReturn.TotalCheck = 0;
            //resReturn.TotalSuccess = 0;
            //resReturn.TotalFail = 0;
           
                foreach(var item in data)
                {
                    var res = new ResultSoatVeOfflineModel();
                    var param = new SqlParameter[] {
                         new SqlParameter("@ResultScan",item.ResultScan),
                         new SqlParameter("@ScanDate",item.ScanDate),
                         new SqlParameter("@ZoneCode",item.ZoneCode),
                         new SqlParameter("@SubId",item.SubId),
                         new SqlParameter("@Validscan",item.Validscan),
                         new SqlParameter("@GateCode",item.GateCode)
                    };

                    try
                    {
                        res = dtx.ResultSoatVeOfflineModel.FromSql("EXEC InsertSoatVeOffline @ResultScan,@ScanDate,@ZoneCode,@SubId,@Validscan,@GateCode", param).FirstOrDefault();
                        resReturn.Add(res);
                    }
                    catch (Exception ex)
                    {
                    resReturn.Add(new ResultSoatVeOfflineModel
                    {
                        SubId = item.SubId,
                        IsSuccess = false
                    });
                        WriteLog.writeToLogFile($"[SoatVeOffline]Exception - {ex}");
                    };
                
                }


            
            return resReturn;

        }



        public DataTableResultModel<HistoryInOutModel> SearchInOutPaging(InOutFilterModel filter)
        {
            var res = new DataTableResultModel<HistoryInOutModel>();
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@FromDate", filter.FromDate),
                    new SqlParameter("@ToDate", filter.ToDate),
                    new SqlParameter("@Keyword", filter.Keyword),
                    new SqlParameter("@Start", filter.start),
                    new SqlParameter("@Length", filter.length),
                    new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output }
                };
                ValidNullValue(param);
                var lstData = dtx.HistoryInOutModel.FromSql("EXEC sp_SearchHistoryInOut @FromDate,@ToDate,@Keyword,@Start,@Length,@TotalRow OUT", param).ToList();
                res.recordsTotal = Convert.ToInt32(param[param.Length - 1].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData.ToList();
            }
            catch (Exception ex)
            {
                res.error = ex.Message;
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<HistoryInOutModel>();
            }

            return res;
        }



        public async Task<DataTableResultModel<ReportCheckinGridModel>> ReportCheckin(SaleHistoryFilterModel filter, bool isExcel)
        {
            var res = new DataTableResultModel<ReportCheckinGridModel>();
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@IsExcel", isExcel),
                new SqlParameter("@FromDate", filter.FromDate),
                new SqlParameter("@ToDate", filter.ToDate),
                 new SqlParameter("@TicketCode", filter.TicketCode),
                new SqlParameter("@Keyword", filter.Keyword),
                new SqlParameter("@Start", filter.start),
                new SqlParameter("@Length",filter.length),
                new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int16, Direction = System.Data.ParameterDirection.Output }
            };
                ValidNullValue(param);
                var lstData = await dtx.ReportCheckinGridModel.FromSql("EXEC sp_ReportCheckin @IsExcel,@FromDate,@ToDate,@TicketCode,@Keyword,@Start,@Length,@TotalRow OUT", param).ToListAsync();
                res.recordsTotal = Convert.ToInt16(param[param.Length - 1].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData.ToList();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<ReportCheckinGridModel>();
            }

            return res;
        }

        public async Task<List<CheckinReportCounterModel>> GetCounterReportCheckin(SaleHistoryFilterModel filter)
        {
            var res = new List<CheckinReportCounterModel>();
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@FromDate", filter.FromDate),
                    new SqlParameter("@ToDate", filter.ToDate),
                    new SqlParameter("@TicketCode", filter.TicketCode),
                    new SqlParameter("@Keyword", filter.Keyword)
                
                };
                ValidNullValue(param);
                res = await dtx.CheckinReportCounterModel.FromSql("EXEC sp_ReportCheckinCounter @FromDate,@ToDate,@TicketCode,@Keyword", param).ToListAsync();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
            }
            return res;
        }




    }
}
