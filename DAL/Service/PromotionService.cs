using DAL.IService;
using DAL.Models.Promotion;
using DAL.Models;
using DAL.Models.Promotion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using WebApp.Infrastructure;
using DAL.Models.Ticket;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq;
using DAL.Models.GatePermission;
using Nancy.Session;

namespace DAL.Service
{
    public class PromotionService:BaseService, IPromotionService
    {
        private EntityDataContext dtx;
        public PromotionService(EntityDataContext dtx)
        {
            this.dtx = dtx;
        }


       
        public async Task<DataTableResultModel<PromotionGridModel>> SearchPromotion(PromotionFilterModel filter)
        {
            var res = new DataTableResultModel<PromotionGridModel>();
            try
            {
                var param = new SqlParameter[] {
                //new SqlParameter("@BranchId", filter.BranchId),
                new SqlParameter("@Keyword", filter.Keyword),
                new SqlParameter("@Start", filter.start),
                new SqlParameter("@Length", filter.length),
                new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int16, Direction = System.Data.ParameterDirection.Output }
                };
                ValidNullValue(param);
                var lstData = await dtx.PromotionGridModel.FromSql("SearchPromotionByCode @Keyword,@Start,@Length,@TotalRow OUT", param).ToListAsync();
                res.recordsTotal = Convert.ToInt16(param[3].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData;
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<PromotionGridModel>();
            }

            return res;
        }
        public SaveResultModel CreatePromotion(PromotionCreateModel model, string userName)
        {
            var result = new SaveResultModel();
            var param = new SqlParameter[] {
                new SqlParameter("@PromotionCode", model.PromotionCode),
                new SqlParameter("@PromotionValue",  model.PromotionValue),
                new SqlParameter("@IsPercent",  model.IsPercent),
                new SqlParameter("@PercentValue", model.PercentValue),
                new SqlParameter("@UserName",  userName)
            };
            ValidNullValue(param);
            var rs = dtx.Database.ExecuteSqlCommand("EXEC sp_CreatePromotion @PromotionCode,@PromotionValue,@IsPercent,@PercentValue,@UserName", param);
            try
            {
                result.ValueReturn = rs ;

            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                result.ValueReturn= 0 ;
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }

            return result;

        }
        public SaveResultModel UpdatePromotion(PromotionModel model, string userName)
        {
            var result = new SaveResultModel();
            var param = new SqlParameter[] {
                new SqlParameter("@PromotionCode", model.PromotionCode),
                new SqlParameter("@PromotionValue",  model.PromotionValue),
                new SqlParameter("@EffectFrom",  model.EffectFrom),
                new SqlParameter("@EffectTo", model.EffectTo),
                new SqlParameter("@UserName",  userName),
                new SqlParameter("@PercentValue",  model.PercentValue)
            };
            ValidNullValue(param);
            var rs = dtx.Database.ExecuteSqlCommand("EXEC sp_CreatePromotion @PromotionCode,@PromotionValue,@EffectFrom,@EffectTo,@UserName,@PercentValue", param);
            try
            {
                result.ValueReturn = rs;

            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                result.ValueReturn= 0;
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }

            return result;

    
        }
        public InfoVoucherViewModel GetInfoVoucher(string VoucherCode)
        {
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@VoucherCode", VoucherCode)
                };
                var data = dtx.InfoVoucherViewModel.FromSql("sp_GetInfoVoucher @VoucherCode", param).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                WebApp.Infrastructure.WriteLog.writeToLogFile($"[Exception]: {ex}");
                return new InfoVoucherViewModel();
            }
        }






        

        public SaveResultModel DeleteProtmotion(int Id, string userName)
        {
            var result = new SaveResultModel();
            var param = new SqlParameter[] {
                new SqlParameter("@Id", Id),
                new SqlParameter("@UserName", userName)
            };
            ValidNullValue(param);
            var rs = dtx.Database.ExecuteSqlCommand("EXEC sp_DeletePromotion @Id,@UserName", param);
            try
            {
                result.ValueReturn = rs;

            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                result.ValueReturn = 0;
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}
