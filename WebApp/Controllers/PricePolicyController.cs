using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.IService;
using DAL.Models;
using DAL.Models.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class PricePolicyController : AppBaseController
    {
        private IPricePolicyService pricePolicyService;
        private ITicketService ticketService;
        private ICustomerService customerService;

        public PricePolicyController(IPricePolicyService pricePolicyService, ITicketService ticketService, ICustomerService customerService)
        {
            this.pricePolicyService = pricePolicyService;
            this.ticketService = ticketService;
            this.customerService = customerService;
        }

        public IActionResult Index()
        {
            ViewBag.TicketList = ticketService.GetAllTicket();
            return View();
        }


        [HttpPost]
        public DataTableResultModel<TicketPricePolicyModel> SearchPricePolicy(PricePolicyFilterModel filter)
        {
            var res = pricePolicyService.SearchPricePolicy(filter);
            return res;
        }

        [HttpGet]
        public async Task<PartialViewResult> _PartialPricePolicy(int id)
        {
            var model = new TicketPricePolicyModel();
            if (id != 0)
            {
                model = pricePolicyService.GetPolicyPriceById(id);
            }
            ViewBag.LstTicketDDL = ticketService.GetAllTicket();
            ViewBag.ListCustType = await customerService.LstAllCustomerType();
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SavePricePolicy([FromBody] TicketPricePolicyModel model)
        {
            var res = pricePolicyService.SavePricePolicy(model, User.Identity.Name);
            
            return Json(res);
        }



        [HttpPost]
        public JsonResult DeletePricePolicy(int id)
        {
            var res = pricePolicyService.DeletePricePolicy(id, User.Identity.Name);
            return Json(res);
        }

        [HttpGet]
        public JsonResult GetAllPricePolicy()
        {
            var userId = AuthenInfo().UserId;
            var res = pricePolicyService.GetAllPricePloicyForSale(userId);
            return Json(res);
        }

    }
}