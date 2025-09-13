using AutoMapper;
using CommonFW.Domain.Model.Payment;
using DAL.Entities;
using DAL.IService;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApp.Infrastructure;
using WebApp.Infrastructure.Configuration;
using WebApp.Infrastructure.HttpClient;
using WebApp.Infrastructure.Utilities;

namespace DAL.Service
{
    public class PaymentService : BaseService, IPaymentService
    {
        private EntityDataContext dtx;
        private readonly ICoreHttpClient _coreHttpClient;
        public PaymentService(EntityDataContext dtx, ICoreHttpClient _coreHttpClient)
        {
            this.dtx = dtx;
            this._coreHttpClient = _coreHttpClient;
        }
        /// <summary>
        /// Check đã tồn tại thông tin Thanh toán hay chưa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExistPaymentInfo(long id)
        {
            var entity = dtx.PaymentInfo.FirstOrDefault(x => x.ContractId == id);
            if (entity != null)
                return true;
            return false;
        }
        public PaymentInfo GetpaymentByContractId(long contractId)
        {
            var entity = dtx.PaymentInfo.FirstOrDefault(x => x.ContractId == contractId);
            return entity;
        }
        public async Task<long> UpdatePayment(PaymentInfo paymentInfo, string userName)
        {
            try
            {
                paymentInfo.UpdatedBy = userName;
                paymentInfo.UpdateDate = DateTime.Now;
                dtx.PaymentInfo.Update(paymentInfo);
                await dtx.SaveChangesAsync();
                return paymentInfo.Id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<ResCommon<long>> InsertOrUpdatePayment(PaymentInfo payment, string userName)
        {
            var res = new ResCommon<long>();
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@ContractId", payment.ContractId),
                    new SqlParameter("@ContractCode", payment.ContractCode),
                    new SqlParameter("@Command", payment.Command),
                    new SqlParameter("@Locale", payment.Locale),
                    new SqlParameter("@CurrencyCode", payment.CurrencyCode),
                    new SqlParameter("@MerchTxnRef", payment.MerchTxnRef),
                    new SqlParameter("@Merchant", payment.Merchant),
                    new SqlParameter("@Amount", payment.Amount),
                    new SqlParameter("@TxnResponseCode", payment.TxnResponseCode),
                    new SqlParameter("@TransactionNo", payment.TransactionNo),
                    new SqlParameter("@Message", payment.Message),
                    new SqlParameter("@AdditionData", payment.AdditionData),
                    new SqlParameter("@SecureHash", payment.SecureHash),
                    new SqlParameter("@NotePayment", payment.NotePayment),
                    new SqlParameter("@CreatedBy", payment.CreatedBy),
                    new SqlParameter("@UpdatedBy", payment.UpdatedBy),
                    new SqlParameter("@ValidatedHash", payment.ValidatedHash)
                };
                ValidNullValue(param);
                var resPayment = await dtx.ResPaymentInfoModel.FromSql("InsertOrUpdatePaymenInfo @ContractId,@ContractCode,@Command,@Locale,@CurrencyCode,@MerchTxnRef,@Merchant,@Amount, @TxnResponseCode, @TransactionNo, @Message, @AdditionData, @SecureHash, @NotePayment, @CreatedBy, @UpdatedBy, @ValidatedHash", param).FirstOrDefaultAsync();
                if(resPayment.Code == 0)// thất bại
                {
                    res.IsSuccess = false;
                    res.Desc = resPayment.Desc;
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Desc = ex.Message;
            }
            return res;
        }

        public string GenerateQRCodePayment(QRCodePaymentModel jsbody)
        {
            var apiKey = AppSettingServices.Get.GenerateQRCodeSettings.ApiKey;
            var clientId = AppSettingServices.Get.GenerateQRCodeSettings.ClientId;
            string UrlAPI = AppSettingServices.Get.DomainSettings.GenerateQRCodePayment;

            
            var result = string.Empty;
            try
            {
                var headerAuthen = new Dictionary<string, string>();
                headerAuthen.Add("x-client-id", clientId);
                headerAuthen.Add("x-api-key", apiKey);
                var json = JsonConvert.SerializeObject(jsbody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var resultCallAPI = _coreHttpClient.PostAsync<ResponQRCodePaymentModel>(UrlAPI, UrlsConfig.PaymentOperations.GetURIQRCodePayment, headerAuthen, jsbody).GetAwaiter().GetResult();
                if (resultCallAPI != null&& resultCallAPI.code == "00")
                {
                    return resultCallAPI.data.qrDataURL;
                }
                else
                {
                    WriteLog.writeToLogFile($"[GenerateQRCodePayment Fail: {resultCallAPI.desc}");
                    return "";
                }
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[GenerateQRCodePayment][Exception]: {ex}");
                return string.Empty;
            }
            return result;
        }

        public DeeplinkInfoAppBankResponse GetInfoAppBankAndroid()
        {
            var urlAndroid = AppSettingServices.Get.DeeplinkBanks.Android;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.GetAsync(urlAndroid).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync().Result;

                        var result = JsonConvert.DeserializeObject<DeeplinkInfoAppBankResponse>(jsonString);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý lỗi nếu cần
                Console.WriteLine($"Lỗi gọi API: {ex.Message}");
            }

            return new DeeplinkInfoAppBankResponse();
        }

        public DeeplinkInfoAppBankResponse GetInfoAppBankIOS()
        {
            var urlIOS = AppSettingServices.Get.DeeplinkBanks.IOS;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.GetAsync(urlIOS).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync().Result;

                        var result = JsonConvert.DeserializeObject<DeeplinkInfoAppBankResponse>(jsonString);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý lỗi nếu cần
                Console.WriteLine($"Lỗi gọi API: {ex.Message}");
            }

            return new DeeplinkInfoAppBankResponse();
        }

        
    }
}
