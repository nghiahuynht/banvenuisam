using DAL.IService;
using DAL.Models;
using DAL.Models.Payoo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.Service
{
    public class PayooService:BaseService, IPayooService
    {
        private EntityDataContext dtx;
        public PayooService(EntityDataContext dtx)
        {
            this.dtx = dtx;
        }


        public ResultModel SaveResultCreatePaymentLink(long orderId,string paramPostModel, ResponseResultCreatePaymentLinkModel model, string responJsonData)
        {
            var res = new ResultModel();
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter("@OrderId", orderId),
                        new SqlParameter("@PosParamJson", paramPostModel),
                        new SqlParameter("@ResponseJson", responJsonData),
                        new SqlParameter("@Result", model.result),
                        new SqlParameter("@CheckSum", model.checksum),
                        new SqlParameter("@Message",model.message),
                        new SqlParameter("@ErrorCode", model.errorcode),
                       
                    };

                ValidNullValue(param);
                dtx.Database.ExecuteSqlCommand("EXEC sp_SavePayooCreatePaymentLink @OrderId,@PosParamJson,@ResponseJson,@Result,@CheckSum,@Message,@ErrorCode", param);
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }

            return res;


        }

        public WaitingPaymentOrderModel GetOrderWaitingPayment(string userName)
        {
            var res = new WaitingPaymentOrderModel();
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@UserName", userName)
                    };
                ValidNullValue(param);
                res = dtx.WaitingPaymentOrderModel.FromSql("EXEC sp_GetOrderWaitingPayment @UserName", param).FirstOrDefault();

            }
            catch (Exception ex)
            {

            }

            return res;
        }








    }
}
