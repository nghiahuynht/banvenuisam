using DAL;
using DAL.IService;
using DAL.Models;
using DAL.Models.TokenMisa;
using DAL.Models.Zalo;
using DAL.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class ZNSController : Controller
    {
        private IZaloService zaloService;
        private readonly IZNSService _znsService;
        private readonly ITokenMisaService _tokenMisaService;
        public ZNSController(IZaloService zaloService,
            IZNSService znsService, ITokenMisaService tokenMisaService)
        {
            this.zaloService=zaloService;
            _znsService = znsService;
            _tokenMisaService = tokenMisaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ConfigMisa()
        {
            return View();
        }


        [HttpGet]
        public ResZNSModel GetCode()
        {
            ZNSHelper.Init();
            var rs = new ResZNSModel() 
            { 
                CodeVerifier = ZNSHelper.CodeVerifier,
                CodeChallenge = ZNSHelper.CodeChallenge
            };
            
            return rs;
        }



        public IActionResult ZaloNotiConfig()
        {
            var config = zaloService.GetZaloConfigNoti();
            ViewBag.TokenMisa = _tokenMisaService.GetTokenMisa("TokenBienLai").TokenValue;
            return View(config);
        }

        [HttpGet]
        public JsonResult ChangeZaloNotiConfig(string columnName,string value,string phonereceived)
        {
            zaloService.ChangeConfigNotify(columnName, value, phonereceived);
            return Json(true);
        }
        [HttpGet]
        public JsonResult GetAccessTokenZNS()
        {
            var res = _znsService.GetAccessTokenZNS();
            return Json(true);
        }
        [HttpPost]
        public JsonResult SendZNS([FromBody]SendZNSModel model)
        {
            var res = _znsService.SendZalo(model);
            return Json(res);
        }

        [HttpGet]
        public JsonResult UpdateTokenMisa()
        {
            
            var configModel = _tokenMisaService.GetConfigMisa("LIVE");
            // Gọi API
            var tokenReturn = new APIResponseResultModel();
            try
            {
                string fullPathAPI = string.Format(@"{0}/api2/user/token?appid={1}&taxcode={2}&username={3}&password={4}", configModel.apiAddress, configModel.appId, configModel.taxCode, configModel.user, configModel.pass);

                var client = new RestSharp.RestClient(fullPathAPI);
                var request = new RestRequest();
                var response = client.Execute(request);
                var content = response.Content;
                tokenReturn = JsonConvert.DeserializeObject<APIResponseResultModel>(content);
                if (tokenReturn.Success)
                {
                    // Lưu token vào cơ sở dữ liệu
                    //userService.SaveMisaToken("TokenBienLai", tokenReturn.Data);
                    _tokenMisaService.UpdateTokenMisa("TokenBienLai", tokenReturn.Data);
                }
            }
            catch (Exception ex)
            {
                tokenReturn.Success = false;
            }
            return Json(true);
        }





    }
}
