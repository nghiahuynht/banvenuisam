using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.IService;
using DAL.Models.Supplier;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class SupplierController : Controller
    {
        private ICustomerService customerService;
        private ISupplierService supplierService;
        public SupplierController(ICustomerService customerService, ISupplierService supplierService)
        {
            this.customerService = customerService;
            this.supplierService = supplierService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SupplierDetail(int? id)
        {
            var viewModel = new SupplierViewModel();
            if (id.HasValue)
            {
                viewModel.Supplier = await supplierService.GetSupplierById(id.Value);
            }
            viewModel.ListAllArea = await customerService.LstAllArea();
            viewModel.ListAllProvince = await customerService.LstAllProvince();
            return View(viewModel);
        }








    }
}