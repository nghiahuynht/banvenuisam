using DAL.Models;
using DAL.Models.Payoo;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IService
{
    public interface IPayooService
    {
        ResultModel SaveResultCreatePaymentLink(long orderId, string paramPostModel, ResponseResultCreatePaymentLinkModel model, string responJsonData);
        WaitingPaymentOrderModel GetOrderWaitingPayment(string userName);
    }
}
