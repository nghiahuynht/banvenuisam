using DAL.IService;
using DAL.Models;
using DAL.Models.partner;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebApp.Infrastructure;

namespace DAL.Service
{
    public class PartnerService :BaseService,IPartnerService
    {
        private EntityDataContext dtx;
        public PartnerService(EntityDataContext dtx)
        {
            this.dtx = dtx;
        }



        public InfoPartnerViewModel GetInfoPartner(int Id)
        {
            
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@Id", Id)
                };
                var data = dtx.InfoPartnerViewModel.FromSql("GetInfoPartner @Id", param).FirstOrDefault();
                return data;
            }
            catch(Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                return new InfoPartnerViewModel();
            }
        }

        public ResultModel InsertUpdatePartner(PartnerModel model)
        {
            var res = new ResultModel();
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter("@Id",0),
                        new SqlParameter("@PartnerCode", model.PartnerCode),
                        new SqlParameter("@PartnerName", model.PartnerName),
                        new SqlParameter("@PartnerPhone",model.PhoneNumber),
                        new SqlParameter("@PartnerAddress", model.Address),
                        new SqlParameter("@Status", 0),
                        new SqlParameter("@FrontUrl", model.FrontUrl),
                        new SqlParameter("@BackUrl", model.BackUrl),
                        new SqlParameter("@BankAccount", model.BankAccount),
                        new SqlParameter("@BankName", model.BankName),
                        new SqlParameter { ParameterName = "@PartnerId", DbType = System.Data.DbType.Int64, Direction = System.Data.ParameterDirection.Output }

                    };


                ValidNullValue(param);
                dtx.Database.ExecuteSqlCommand("EXEC sp_InsertUpdatePartner @Id,@PartnerCode,@PartnerName,@PartnerPhone,@PartnerAddress,@Status,@FrontUrl,@BackUrl,@BankAccount,@BankName,@PartnerId OUT", param);


                res.ValueReturn = Convert.ToInt64(param[param.Length - 1].Value);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
                res.IsSuccess = false;

            }
            return res;
        }

        public DataTableResultModel<PartnerGridModel> SearchTicket(PartnerFilterModel filter)
        {
            var res = new DataTableResultModel<PartnerGridModel>();
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@FromDate", filter.FromDate),
                new SqlParameter("@ToDate", filter.ToDate),
                new SqlParameter("@PartnerCode", filter.ParnerCode),
                new SqlParameter("@Start", filter.start),
                new SqlParameter("@Length", filter.length),
                new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int16, Direction = System.Data.ParameterDirection.Output }
            };
                ValidNullValue(param);
                var lstData = dtx.PartnerGridModel.FromSql("sp_SearchPartner @FromDate,@ToDate,@PartnerCode,@Start,@Length,@TotalRow OUT", param).ToList();
                res.recordsTotal = Convert.ToInt16(param[4].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData.ToList();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<PartnerGridModel>();
            }

            return res;
        }

        public bool ApprovalPartner(int PartnerId, string ApprovalBy)
        {
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@PartnerId", PartnerId),
                    new SqlParameter("@ApprovalBy", ApprovalBy)
                };
               var rs= dtx.Database.ExecuteSqlCommand(@"EXEC sp_ApprovalPartner @PartnerId,@ApprovalBy", param);
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                return false;
            }
        }

        public PartnerModelViewModel GetPartnerById(int Id)
        {
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@Id", Id)
                };
                var data = dtx.PartnerModelViewModel.FromSql("GetPartnerById @Id", param).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                return new PartnerModelViewModel();
            }
        }

        public bool CancelApproval(int Id, string Note, string ApprovalBy)
        {
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@Id", Id),
                    new SqlParameter("@UserId", ApprovalBy),
                    new SqlParameter("@Note", Note)
                };
                var rs = dtx.Database.ExecuteSqlCommand(@"EXEC sp_CancelApproval @Id,@UserId,@Note", param);
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                return false;
            }
        }

        public bool DeletePartner(int PartnerId, string ApprovalBy)
        {
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@PartnerId", PartnerId),
                    new SqlParameter("@DeletedBy", ApprovalBy)
                };
                var rs = dtx.Database.ExecuteSqlCommand(@"EXEC sp_DeletePartner @PartnerId,@DeletedBy", param);
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                return false;
            }
        }
    }
}
