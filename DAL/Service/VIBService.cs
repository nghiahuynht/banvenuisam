using CommonFW.Domain.Model.Payment;
using DAL.IService;
using DAL.Models;
using DAL.Models.VIB;
using Microsoft.EntityFrameworkCore.Metadata;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebApp.Infrastructure;
using WebApp.Infrastructure.Configuration;
using WebApp.Infrastructure.Utilities;

namespace DAL.Service
{
    public class VIBService : BaseService, IVIBService
    {
        private EntityDataContext dtx;
        private readonly ICoreHttpClient _coreHttpClient;
        public VIBService(EntityDataContext dtx, ICoreHttpClient _coreHttpClient)
        {
            this.dtx = dtx;
            this._coreHttpClient = _coreHttpClient;
        }



        public async Task<ResCommon<string>> GenerateQRCodeVIB(VirtualAccountModel model)
        {
            ResCommon<string> res = new ResCommon<string>();
            var authorization = AppSettingServices.Get.VIBSettings.Authorization;
            var username = AppSettingServices.Get.VIBSettings.Username;
            var password = AppSettingServices.Get.VIBSettings.Password;
            string UrlAPI = AppSettingServices.Get.DomainSettings.VIBService;
            // B1. Get token 
            try
            {
                var headerAuthen = new Dictionary<string, string>
                {
                    { "Authorization", authorization },
                    { "Content-Type", "application/x-www-form-urlencoded" }
                };

                var formParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                };

                var authVIB = _coreHttpClient.PostAsyncFormUrlEncoded<AuthVIBResponse>(UrlAPI, "/token", headerAuthen, formParams).GetAwaiter().GetResult();
                //var authVIB = new AuthVIBResponse
                //{
                //    access_token = "1231"
                //};

                if (authVIB != null)
                {
                    res.IsSuccess=true;
                    res.Desc = authVIB.access_token;
                    // B2. Tạo tài khoản ảo
                    string dataToSign = $"{model.CIF}|{model.VirtualAccount}|{model.ReqId}|G4M|{model.Name}|Y|{model.Amount}";
                    var privateKeyPath = AppSettingServices.Get.GeneralSettings.privateKeyVIBPath;

                    var signeddata = Helper.SignDataVIB(dataToSign, privateKeyPath);

                    var headerAuthenCreateVirtualAccount = new Dictionary<string, string>();
                    headerAuthenCreateVirtualAccount.Add("Authorization", "Bearer "+ authVIB.access_token);
                    headerAuthenCreateVirtualAccount.Add("signeddata", signeddata);
                    var jsbodyVirtualAcc = new
                    {
                        reqid=model.ReqId,
                        code= "G4M",
                        name = model.Name,
                        isvalidamount = "Y",
                        amount = model.Amount
                    };
                    var json = JsonConvert.SerializeObject(jsbodyVirtualAcc);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var resultAPICreateVA = _coreHttpClient.PostAsync<ResponseCreateirtualAccount>(UrlAPI, $"/egatepublic/1.0.0/v1.1/virtualaccount/{model.CIF}/account/{model.VirtualAccount}", headerAuthenCreateVirtualAccount, jsbodyVirtualAcc).GetAwaiter().GetResult();
                    if(resultAPICreateVA != null && (resultAPICreateVA.Result.STATUSCODE == "000000" || resultAPICreateVA.Result.STATUSCODE == "VA0011"))
                    {
                        //B3. Tạo qr code
                        var jsbodyQRCode = new
                        {
                            requestID = model.ReqId,
                            accountNo = model.VirtualAccount,
                            amount = model.Amount,
                            isGenQR = true,
                            description = "thanh toan ve tham quan"
                        };

                        var headerAuthenCreateQRCode = new Dictionary<string, string>();
                        headerAuthenCreateQRCode.Add("Authorization", "Bearer " + authVIB.access_token);

                        var rsAPICreateQRCode = _coreHttpClient.PostAsync<QrDataResponse>(UrlAPI, "/wbgate/1.0.0/va-service/v1/oe/account-mngmt/vietqr", headerAuthenCreateQRCode, jsbodyQRCode).GetAwaiter().GetResult();
                        // 
                        if(rsAPICreateQRCode.Result.STATUSCODE == "000000")
                        {
                            res.IsSuccess = true;
                            res.Desc = rsAPICreateQRCode.Result.DATA.qrImage;
                            return res;
                        }   
                        else
                        {
                            res.IsSuccess = false;
                            res.Desc = "Lỗi tạo qr code";
                                                       return res;
                        }    
                    }    
                }
                else
                {
                    res.IsSuccess=false;
                    res.Desc = "Lỗi lấy toke VIB";
                }
            }
            catch (Exception ex)
            {
                WriteLog.writeToLogFile($"[GenerateQRCodeVIB][Get Token] [Exception]: {ex}");
                res.IsSuccess = false;
                res.Desc="Lỗi Tao QR VIB"+ex.Message;
                return res;
            }
            return  res;
        }


        //public static void WriteSimpleLog(string message)
        //{
        //    string path = @"C:\logservice\logservice.txt";
        //    var sb = new StringBuilder();
        //    sb.AppendLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}");
        //    File.AppendAllText(path, sb.ToString());
        //}
    }
}
