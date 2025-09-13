using Dapper;
using GM.CORE.Helpers;
using GM.MODEL.ViewModel;
using System.Collections.Generic;
using System;
using System.Data;
using System.Threading.Tasks;
using GM.CORE.Extensions;

namespace GM.DAL.Respository
{
    public interface IPaymentRepository
    {
        Task<OrderInforViewModel> GetOrderInforByCode(string code);
        Task<CURDResponse> UpdatePaymentStatus(PaymentInforViewModel model, string userName);
    }

    internal class PaymentRepository : RepositoryBase, IPaymentRepository
    {
        public PaymentRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }

        public async Task<OrderInforViewModel> GetOrderInforByCode(string code)
        {
            var dpars = new DynamicParameters();
            dpars.Add("@OrderCode", code);

            return await QuerySingleOrDefaultAsync<OrderInforViewModel>(StoreProcedure.GET_ORDER_INFOR, dpars);
        }

        public async Task<CURDResponse> UpdatePaymentStatus(PaymentInforViewModel model, string userName)
        {
            var res = new CURDResponse();
            try
            {
                Dictionary<string, object> dics = model.ToDict();
                var pars = new DynamicParameters(dics);
                pars.Add("@UserName", CommonHelper.CheckStringNull(userName));

                res = await QuerySingleOrDefaultAsync<CURDResponse>(StoreProcedure.UPDATE_PAYMENT_ORDER, pars);
                return res;
            }
            catch (Exception ex)
            {

                res.StatusResult = false;
                res.MessageResult = ex.Message;
                return res;
            }
        }


    }
}