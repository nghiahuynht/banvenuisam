using AutoMapper;
using DAL.IService;
using DAL.Models;
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
    public class PricePolicyService:BaseService, IPricePolicyService
    {
        private EntityDataContext dtx;
        private IMapper mapper;
        public PricePolicyService(EntityDataContext dtx, IMapper mapper)
        {
            this.dtx = dtx;
            this.mapper = mapper;
        }



        public SaveResultModel SavePricePolicy(TicketPricePolicyModel model,string userName)
        {
            var res = new SaveResultModel();
            try
            {
                var transaction = dtx.Database.BeginTransaction();
                var param = new SqlParameter[] {
                    new SqlParameter("@Id",model.Id),
                    new SqlParameter("@TicketCode", model.TicketCode),
                    new SqlParameter("@CustomerType", model.CustomerType),
                    new SqlParameter("@CustomerForm", model.CustomerForm),
                    new SqlParameter("@Price", model.Price),
                    new SqlParameter("@UserName", userName),
                };
                ValidNullValue(param);
                dtx.Database.ExecuteSqlCommand(@"EXEC sp_SavePricePolicy @Id,@TicketCode,@CustomerType,@CustomerForm,@Price,@UserName", param);
                res.IsSuccess = true;
                transaction.Commit();
                transaction.Dispose();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
                res.IsSuccess = false;
                WriteLog.writeToLogFile($"[Exception save policy price]: {ex}");
            }
            return res;
        }


        public TicketPricePolicyModel GetPolicyPriceById(int id)
        {
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@Id", id)
                };
                var data = dtx.TicketPricePolicyModel.FromSql("sp_GetPricePolicyById @Id", param).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                WebApp.Infrastructure.WriteLog.writeToLogFile($"[Exception]: {ex}");
                return new TicketPricePolicyModel();
            }
        }


        public DataTableResultModel<TicketPricePolicyModel> SearchPricePolicy(PricePolicyFilterModel filter)
        {
            var res = new DataTableResultModel<TicketPricePolicyModel>();
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@TicketCode", filter.TicketCode),
                    new SqlParameter("@Start", filter.start),
                    new SqlParameter("@Length", filter.length),
                    new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output }
                };
                ValidNullValue(param);
                var lstData = dtx.TicketPricePolicyModel.FromSql("EXEC sp_SearchPricePolicy @TicketCode,@Start,@Length,@TotalRow OUT", param).ToList();
                res.recordsTotal = Convert.ToInt32(param[param.Length - 1].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData.ToList();
            }
            catch (Exception ex)
            {
                res.error = ex.Message;
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<TicketPricePolicyModel>();
            }

            return res;
        }


        public SaveResultModel DeletePricePolicy(int id, string userName)
        {
            var res = new SaveResultModel();
            try
            {
                var transaction = dtx.Database.BeginTransaction();
                var param = new SqlParameter[] {
                    new SqlParameter("@Id",id),
                    new SqlParameter("@UserName", userName),
                };
                ValidNullValue(param);
                dtx.Database.ExecuteSqlCommand(@"EXEC sp_DeletePricePolicy @Id,@UserName", param);
                res.IsSuccess = true;
                transaction.Commit();
                transaction.Dispose();
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
                WriteLog.writeToLogFile($"[Exception Delete policy price]: {ex}");
            }
            return res;
        }

        public List<TicketPricePolicyModel> GetAllPricePloicyForSale(int userId)
        {
            var res = new List<TicketPricePolicyModel>();
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@UserId", userId)
                };
                
                res = dtx.TicketPricePolicyModel.FromSql("EXEC sp_GetAllPricePolicy @UserId", param).ToList();
            }
            catch (Exception ex)
            {
                res = new List<TicketPricePolicyModel>();
            }
            return res;
        }

    }
}
