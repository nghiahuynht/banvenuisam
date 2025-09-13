using WebApp.Infrastructure.HttpClient;
using DAL.IService;
using DAL.Models;
using DAL.Models.Zalo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApp.Infrastructure.Utilities;
using WebApp.Infrastructure.Configuration;
using WebApp;
using WebApp.Infrastructure;
using System.Text.Json;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Service
{
    public class ZaloService :BaseService, IZaloService
    {
        private readonly ICoreHttpClient _coreHttpClient;
        private EntityDataContext dtx;
        public ZaloService(EntityDataContext dtx, ICoreHttpClient _coreHttpClient)
        {
            this.dtx = dtx;
            this._coreHttpClient = _coreHttpClient;
        }
        #region zalo
        public ResCommon<int> SendZalo(SendZNSModel model)
        {
            var rs = new ResCommon<int>();
            var log = new StringBuilder("");
            string UrlAPI = AppSettingServices.Get.DomainSettings.ZaloService;
            var templateID = AppSettingServices.Get.ZaloSettings.TemplateID;
            var campainID = AppSettingServices.Get.ZaloSettings.CampainID;
            var ctaCode = AppSettingServices.Get.ZaloSettings.CTACode;
            try
            {
                var input = new
                {
                    CampaignID = campainID,
                    TemplateID = templateID,
                    Param = new
                    {
                        ma_khach_hang = model.CustomerName,
                        ma_tra_cuu = model.SubOrderCode,
                        price = model.StrPrice,
                        so_luong = model.Quanti,
                        total = model.StrTotal,
                        date = model.StrCreatedDate,
                        qr = model.SubOrderId,
                        code = ctaCode,
                        phone = model.PhoneNumber
                    }
                };
                //Call api lấy token
                var token = GetTokenAuthenZalo().GetAwaiter().GetResult();
                if (string.IsNullOrEmpty(token))//fail
                {
                    rs.IsSuccess = false;//thất bại
                    rs.Desc = "Lấy token thất bại";
                    return rs;
                }
               
                //Call api send zns
                var headerAuthen = new Dictionary<string, string>();
                headerAuthen.Add("Authorization", "Bearer " + token);
                var resultCallAPI = _coreHttpClient.PostAsync<ResZNSDto>(UrlAPI, UrlsConfig.ZaloOperations.CreateZNS, headerAuthen, input).GetAwaiter().GetResult();
                string strJsonZalo = System.Text.Json.JsonSerializer.Serialize<ResZNSDto>(resultCallAPI);
                if (resultCallAPI != null
                    && resultCallAPI?.data?.status == 200
                    && resultCallAPI?.data?.response?.status == 1000)
                {
                    rs.IsSuccess = true;//thành công
                    rs.Desc = "Gửi tin zalo thành công";
                    return rs;
                }
                else
                {
                    rs.IsSuccess = false;//thất bại
                    rs.Desc = "Gửi tin zalo thất bại";
                    return rs;
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;//thất bại
                rs.Desc = "Đã xảy ra lỗi trong quá trình xử lý!";
                WriteLog.writeToLogFile($"[SendZalo][Exception]: {ex}");
                return rs;
            }

        }
        public ResCommon<int> SendZaloOld(SendZNSModel model)
        {
            var rs = new ResCommon<int>();
            var log = new StringBuilder("");
            string UrlAPI = AppSettingServices.Get.DomainSettings.ZaloService;
            var templateID = AppSettingServices.Get.ZaloSettings.TemplateID;
            var campainID = AppSettingServices.Get.ZaloSettings.CampainID;
            try
            {
                var input = new
                {
                    CampaignID = campainID,
                    TemplateID = templateID,
                    //brandname = "",
                    Param = new
                    {
                        phone = model.PhoneNumber,
                        customer_name = model.CustomerName,
                        bill = model.SubOrderCode,
                        id = model.SubOrderId
                    }
                };

                //Call api lấy token
                var token = GetTokenAuthenZalo().GetAwaiter().GetResult();
                if (string.IsNullOrEmpty(token))//fail
                {
                    rs.IsSuccess = false;//thất bại
                    rs.Desc = "Lấy token thất bại";
                    return rs;
                }
                //Call api send zns
                var headerAuthen = new Dictionary<string, string>();
                headerAuthen.Add("Authorization", "Bearer " + token);
                var resultCallAPI = _coreHttpClient.PostAsync<ResZNSDto>(UrlAPI, UrlsConfig.ZaloOperations.CreateZNS, headerAuthen, input).GetAwaiter().GetResult();
                if (resultCallAPI != null
                    && resultCallAPI?.data?.status == 200
                    && resultCallAPI?.data?.response?.status == 1000)
                {
                    rs.IsSuccess = true;//thành công
                    rs.Desc = "Gửi tin zalo thành công";
                    return rs;
                }
                else
                {
                    rs.IsSuccess = false;//thất bại
                    rs.Desc = "Gửi tin zalo thất bại";
                    return rs;
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;//thất bại
                rs.Desc = "Đã xảy ra lỗi trong quá trình xử lý!";
                WriteLog.writeToLogFile($"[SendZalo][Exception]: {ex}");
                return rs;
            }

        }
        public async Task<string> GetTokenAuthenZalo()
        {
            var apiKey = AppSettingServices.Get.ZaloSettings.APIKey;
            var apiSecert = AppSettingServices.Get.ZaloSettings.APISecert;
            string UrlAPI = AppSettingServices.Get.DomainSettings.TokenZaloUrl;

            var inputAPI = new
            {
                api_key = apiKey,
                api_secert = apiSecert
            };
            var result = string.Empty;
            try
            {
                var keyCache = "Cache_Get_Token_Authen_Zalo";
                //Lấy dữ liệu đã cache
                var token = MemoryCacheHelper.GetValue(keyCache);
                if (token == null)
                {
                    //Call API login voucher
                    #region Call API token
                    var objJson = await _coreHttpClient.PostAsync<ResTokenDto>(UrlAPI, UrlsConfig.ZaloOperations.GetToKen, inputAPI);
                    if (objJson == null)
                    {
                        return string.Empty;
                    }
                    #endregion
                    //lấy thông tin từ api
                    if (objJson.data?.status != 200
                        || objJson.data?.response?.status != "1000")
                    {
                        return string.Empty;
                    }
                    var strToken = objJson.data?.response?.data?.IsToken ?? string.Empty;
                    var expiredTime = objJson?.data?.response?.data?.Expried;
                    var expTime = DateTimeOffset.Parse(expiredTime).AddMinutes(-1);
                    //Cache token lại theo expiredTime
                    if (!string.IsNullOrEmpty(strToken))
                    {
                        MemoryCacheHelper.Add(keyCache, strToken, expTime);
                        token = MemoryCacheHelper.GetValue(keyCache);
                    }
                }
                result = token?.ToString() ?? string.Empty;

            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[GetTokenAuthenZalo][Exception]: {ex}");
                return string.Empty;
            }
            return result;
        }


        public ZaloNotiConfigModel GetZaloConfigNoti()
        {
            try
            {

                var param = new SqlParameter[] {};
                ValidNullValue(param);
                var res = dtx.ZaloNotiConfigModel.FromSql(@"EXEC sp_GetConfigNoti", param).FirstOrDefault();
                return res;
            }
            catch (Exception ex)
            {

                WriteLog.writeToLogFile($"[Exception insert change noti]: {ex}");
                return new ZaloNotiConfigModel();
            }
        }


        public void ChangeConfigNotify(string columnName, string value,string phoneReceived)
        {
            try
            {

                var param = new SqlParameter[] {
                    new SqlParameter("@ColumnName",columnName),
                    new SqlParameter("@Value", value),
                    new SqlParameter("@PhoneReceive", phoneReceived),
                };
                ValidNullValue(param);
                dtx.Database.ExecuteSqlCommand(@"EXEC sp_ChangeZaloNotiConfig @ColumnName,@Value,@PhoneReceive", param);
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception insert change noti]: {ex}");
            }
        }
        #endregion zalo
    }
}
