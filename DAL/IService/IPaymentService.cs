using CommonFW.Domain.Model.Payment;
using DAL.Entities;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface IPaymentService
    {
        /// <summary>
        /// Check tồn tại thông tin thanh toán của ĐH
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsExistPaymentInfo(long id);
        PaymentInfo GetpaymentByContractId(long contractId);
        Task<long> UpdatePayment(PaymentInfo paymentInfo, string userName);
        Task<ResCommon<long>> InsertOrUpdatePayment(PaymentInfo payment, string userName);
        // ddphuong 11/08/2023:Create service
        string GenerateQRCodePayment(QRCodePaymentModel model);
        DeeplinkInfoAppBankResponse GetInfoAppBankAndroid();
        DeeplinkInfoAppBankResponse GetInfoAppBankIOS();
    }
}
