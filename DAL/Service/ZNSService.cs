using DAL.Entities;
using DAL.IService;
using DAL.Models;
using DAL.Models.Ticket;
using DAL.Models.Zalo;
using DAL.Models.ZNS;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Infrastructure;
using WebApp.Infrastructure.Configuration;
using WebApp.Infrastructure.Consts;
using WebApp.Infrastructure.HttpClient;
using WebApp.Infrastructure.Utilities;

namespace DAL.Service
{
    public class ZNSService: BaseService,IZNSService
    {
        private readonly ICoreHttpClient _coreHttpClient;
        private EntityDataContext dtx;
        public ZNSService(EntityDataContext dtx, ICoreHttpClient _coreHttpClient)
        {
            this.dtx = dtx;
            this._coreHttpClient = _coreHttpClient;
        }
        
        /// <summary>
        /// Lấy thông tin Accesstoken
        /// </summary>
        /// <returns></returns>
        public AccessTokenZNSDto GetAccessTokenZNS()
        {
            try
            {
                #region Call authen GM để lấy token ZNS
                var input = new
                {
                    OAId = AppSettingServices.Get.ZNSSettings.OAId,
                    AppId = AppSettingServices.Get.ZNSSettings.AppId
                };
                var objData = _coreHttpClient.PostAsyncHttp<InternalBaseDto<AccessTokenZNSDto>>(HttpClientName.OAuthGamanService
                    , UrlsConfig.OAuthGamanOperations.GetAccessToKen, input).Result;
                return objData.Data;
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[GetAccessTokenZNS][Exception]: {ex}");
                return null;
            }
        }

        public ResCommon<int> SendZalo(SendZNSModel model)
        {
            var rs = new ResCommon<int>();
            var log = new StringBuilder("");
            string UrlAPI = AppSettingServices.Get.DomainSettings.ZNSService;
            var templateID = AppSettingServices.Get.ZNSSettings.TemplateID;
            var ctaCode = AppSettingServices.Get.ZNSSettings.CTACode;
            try
            {
                ZNSHelper.Init();
                var input = new ReqSendZNSModel()
                {
                    //Mode = "development",
                    Phone = Helper.ConvertPhoneToFormatVN(model.PhoneNumber),
                    TemplateId= templateID,
                    Data = new ReqTemplateDataModel()
                    {
                        CustomerName = model.CustomerName,
                        TicketCode = model.SubOrderCode,
                        Price = model.StrPrice,
                        Quantity = model.Quanti.ToString(),
                        Total = model.StrTotal,
                        CreatedDate = model.StrCreatedDate,
                        QRCode = model.SubOrderId.ToString(),
                        CTACode = ctaCode,
                        CompanyName = AppSettingServices.Get.ZNSSettings.CompanyName,
                        VisitDate = model.StrVisitDate,
                        PlaceVisit = model.GateName
                    },
                    TrackingId = ZNSHelper.CodeVerifier
                };
                //Call api lấy token
                var objToken = GetAccessTokenZNS();
                if (string.IsNullOrEmpty(objToken?.AccessToken))//fail
                {
                    rs.IsSuccess = false;//thất bại
                    rs.Desc = "Lấy token thất bại";
                    WriteLog.writeToLogFile("[SendZalo]: Lấy token thất bại");
                    return rs;
                }

                //Call api send zns
                var headerAuthen = new Dictionary<string, string>();
                headerAuthen.Add("access_token", objToken.AccessToken);
                var resultCallAPI = _coreHttpClient.PostAsync<ResSendZNSModel>(UrlAPI, UrlsConfig.ZNSOperations.SendZNS, headerAuthen, input).GetAwaiter().GetResult();
                if (resultCallAPI != null
                    && resultCallAPI.Error == 0)
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


        public ResOrderInfoDto GetOrderInfo(long orderId)
        {
            var res = new ResOrderInfoDto();
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@OrderId", orderId),
                };
                ValidNullValue(param);
                res = dtx.ResOrderInfoDto.FromSql("EXEC sp_GetOrderInfo @OrderId", param).FirstOrDefault();
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[Exception]: {ex}");
                res = new ResOrderInfoDto();
            }

            return res;
        }


        public ResCommon<int> SendZaloTicketOrderSuccess(long orderId)
        {
            var orderInfo = GetOrderInfo(orderId);
            var model = new SendZNSModel
            {
                PhoneNumber = orderInfo.Phone,
                CustomerName= orderInfo.CustomerName,
                SubOrderCode = orderInfo.SubOrderCode,
                StrPrice = orderInfo.StrPrice,
                Quanti = orderInfo.Quanti,
                StrTotal = orderInfo.StrTotal,
                StrCreatedDate = orderInfo.StrCreatedDate,
                SubOrderId = orderInfo.SubOrderCodeId,
                StrVisitDate = orderInfo.StrVisitDate,
                GateName = orderInfo.TicketDescription
            };


            var rs = new ResCommon<int>();
            var log = new StringBuilder("");
            string UrlAPI = AppSettingServices.Get.DomainSettings.ZNSService;
            var templateID = AppSettingServices.Get.ZNSSettings.TemplateID;
            var ctaCode = AppSettingServices.Get.ZNSSettings.CTACode;
            try
            {
                ZNSHelper.Init();
                var input = new ReqSendZNSModel()
                {
                    //Mode = "development",
                    Phone = Helper.ConvertPhoneToFormatVN(model.PhoneNumber),
                    TemplateId = templateID,
                    Data = new ReqTemplateDataModel()
                    {
                        CustomerName = model.CustomerName,
                        TicketCode = model.SubOrderCode,
                        Price = model.StrPrice,
                        Quantity = model.Quanti.ToString(),
                        Total = model.StrTotal,
                        CreatedDate = model.StrCreatedDate,
                        QRCode = model.SubOrderId.ToString(),
                        CTACode = ctaCode,
                        CompanyName = AppSettingServices.Get.ZNSSettings.CompanyName,
                        VisitDate = model.StrVisitDate,
                        PlaceVisit = model.GateName
                    },
                    TrackingId = ZNSHelper.CodeVerifier
                };
                //Call api lấy token
                var objToken = GetAccessTokenZNS();
                if (string.IsNullOrEmpty(objToken?.AccessToken))//fail
                {
                    rs.IsSuccess = false;//thất bại
                    rs.Desc = "Lấy token thất bại";
                    WriteLog.writeToLogFile("[SendZalo]: Lấy token thất bại");
                    return rs;
                }

                //Call api send zns
                var headerAuthen = new Dictionary<string, string>();
                headerAuthen.Add("access_token", objToken.AccessToken);
                var resultCallAPI = _coreHttpClient.PostAsync<ResSendZNSModel>(UrlAPI, UrlsConfig.ZNSOperations.SendZNS, headerAuthen, input).GetAwaiter().GetResult();
                if (resultCallAPI != null
                    && resultCallAPI.Error == 0)
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





    }
}
