using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.IService;
using DAL.Models;
using DAL.Models.Customer;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerService customerService;
        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }


        public async Task<IActionResult> Index()
        {
            //ViewBag.LstCustomerType = await customerService.LstAllCustomerType();
            //ViewBag.LstProvince = await customerService.LstAllProvince();
            return View();
        }
        public async Task<IActionResult> CustomerDetail(int? id)
        {
            var viewModel = new CustomerViewModel(); 
            if (id.HasValue)
            {
                viewModel.Customer = await customerService.GetCustomerById(id.Value);
            }
            viewModel.ListCustType = await customerService.LstAllCustomerType();
            //viewModel.ListAllArea = await customerService.LstAllArea();
            //viewModel.ListAllProvince = await customerService.LstAllProvince();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> SaveCustomer([FromBody] Customer model)
        {
            var res = new SaveResultModel();
            if (model.Id == 0)
            {
                // check data tồn tại theo mã số thuế và code
                var dataExsts= await customerService.GetCustomerByCodeAndTaxCode(model.CustomerCode,model.TaxCode);
                if(dataExsts != null)
                {
                    res.IsSuccess = false;
                    res.ErrorMessage = "Mã khách hàng hoặc mã số thuế đã tồn tại trong hệ thống! ";
                    return Json(res);
                }    
                res = await customerService.CreateCustomer(model, User.Identity.Name);
            }
            else
            {
                res = await customerService.UpdateCustomer(model, User.Identity.Name);
            }
            return Json(res);
        }

        [HttpPost]
        public DataTableResultModel<CustomerGridModel> SearchCustomer(CustomerFilterModel filter)
        {
            var res = customerService.SearchCustomerPaging(filter);
            return res;
        }


        [HttpPost]
        public async Task<JsonResult> DeleteCustomer(int id)
        {
            var res = await customerService.DeleteCustomer(id, User.Identity.Name);
            return Json(res);
        }


        [HttpGet]
        public async Task<JsonResult> GetListCustomerByCustType(string customerType)
        {
            var res = await customerService.ListCustomerByType(customerType);
            return Json(res);
        }


    }
}