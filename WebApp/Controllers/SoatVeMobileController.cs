using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.IService;
using DAL.Models;
using DAL.Models.GatePermission;
using DAL.Models.SoatVe;
using DAL.Models.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoatVeMobileController : Controller
    {
        private IUserInfoService userService;
        private ISoatVeService soatVeService;

        public SoatVeMobileController(IUserInfoService userService, ISoatVeService soatVeService)
        {
            this.userService = userService;
            this.soatVeService = soatVeService;
        }


        [HttpPost("[action]")]
        public AuthenticatedModel Login(LoginModel model)
        {
            var user = userService.Login(model.UserName, model.Password);
            if (user != null)
            {
                return new AuthenticatedModel { UserId=user.Id,UserName = model.UserName, FullName = user.FullName, Role = user.RoleCode };

            }
            else
            {
                return new AuthenticatedModel();
            }
        }

        [HttpGet("[action]")]
        public List<ComboBoxModel> GetGateByUser(string userName)
        {
            var lst = soatVeService.GetGateDDLByUser(userName);
            return lst;
        }

        [HttpGet("[action]")]
        public JsonResult ScanAction(Int64 subId, string gateCode)
        {
            var res = soatVeService.UpdateScanResult(subId, gateCode);
            return Json(res);
        }


        [HttpGet("[action]")]
        public JsonResult ReportScanMobile(string gateCode,string dateScan)
        {
            var date = Convert.ToDateTime(dateScan);
            var res = soatVeService.ReportSoatVeMobile(string.Empty, gateCode,date);
            return Json(res);
        }

        [HttpGet("[action]")]
        public List<GateListModel> GetGateByParentCode(string parentCode)
        {
            var lst = soatVeService.GetGateByParentCode(parentCode);
            return lst;
        }

        [HttpPost("[action]")]
        public List<ResultSoatVeOfflineModel> SoatVeOffline(List<SoatVeOfflineModel> data)
        {
            var lst = soatVeService.SoatVeOffline(data);
            return lst;
        }
    }
}