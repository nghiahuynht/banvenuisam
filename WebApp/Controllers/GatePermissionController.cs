using DAL.IService;
using DAL.Models;
using DAL.Models.GatePermission;
using DAL.Models.Ticket;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class GatePermissionController : Controller
    {
        private IGatePermissionService gatePermissionService;
        private IUserInfoService userInfoService;
        public GatePermissionController(IGatePermissionService gatePermissionService, IUserInfoService userInfoService)
        {
            this.gatePermissionService = gatePermissionService;
            this.userInfoService = userInfoService;
        }

        public async Task<IActionResult> Index()
        { 
            ViewBag.UserList = await userInfoService.ListAllUserInfo();
            return View();
        }

        // Get all gate
        [HttpPost]
        public DataTableResultModel<GatePermissionGridModel> GetGatePermissionPaging(GatePermissionFilterModel filter)
        {
            var res = gatePermissionService.GetGatePermissionPaging( filter);
            
            return res;
        }

        // Get all gate
        [HttpPost]
        public JsonResult SaveGatePermission([FromBody]  GatePermissionModel model)
        {
            var res = gatePermissionService.SaveGatePermission(model);

            return Json(new {
                ValueReturn = 0,
                IsSuccess = res
            });
        }

        // delete gate
        [HttpGet]
        public JsonResult DeleteGate( string gateCode)
        {
            var res =  gatePermissionService.DeleteGate(gateCode);
            return   Json(new
            {
                ValueReturn = 0,
                IsSuccess = res
            }); 
        }

        // Get all gate
        [HttpPost]
        public JsonResult CreateGate([FromBody] GateModel model)
        {
            var res = gatePermissionService.CreateGate(model);

            return Json(new
            {
                ValueReturn = 0,
                IsSuccess = res
            });
        }
    }
}
