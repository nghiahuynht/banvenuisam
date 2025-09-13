using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.IService;
using DAL.Models;
using DAL.Models.Invoice;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class InvoiceController : Controller
    {
        private IInvoiceService invoiceService;
        private IUserInfoService userInfoService;
        private IProductService productInfoService;
        public InvoiceController(IInvoiceService invoiceService, IUserInfoService userInfoService, IProductService productInfoService)
        {
            this.invoiceService = invoiceService;
            this.userInfoService = userInfoService;
            this.productInfoService = productInfoService;
        }
        public IActionResult Index(string id)// invoice type: SO or PO
        {
            var searchModel = new InvoiceFilterModel
            {
                InvoiceType = id,
                FromDate = DAL.Helper.FirtDayOfMonth(),
                ToDate =DateTime.Now.ToString("dd/MM/yyyy")

            };
            return View(searchModel);
        }
        public async Task<IActionResult> InvoiceDetail(string id,int? id2)
        {
            var viewModel = new InvoiceDetailViewModel();
            if (id2.HasValue)
            {
                var invoice = await invoiceService.GetInvoiceById(id2.Value);
                viewModel.Invoice = invoice;
                int objId = invoice.ObjId.HasValue ? invoice.ObjId.Value : 0;
                viewModel.ObjSelected = await invoiceService.GetObjSelected(objId, viewModel.Invoice.InvoiceType);
                viewModel.StaffSelected = await userInfoService.GetUserByUserName(invoice.StaffCode);

            }
            else
            {
                viewModel.Invoice = new InvoiceModel();
            }
            viewModel.Invoice.InvoiceType = id;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> SaveInvoice([FromBody] InvoiceDetailViewModel model)
        {
            var res = new SaveResultModel();
            if(model.Invoice.Id == 0)
            {
                res = await invoiceService.CreateInvoice(model.Invoice, model.InvoiceDetails, User.Identity.Name);
            }
            else
            {
                res = await invoiceService.UpdateInvoice(model.Invoice, model.InvoiceDetails, User.Identity.Name);
            }


            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> SearhObjAutoComplete([FromBody] AutoCompleteFilterModel model)
        {
            var lst = await invoiceService.SearcObjAutocomplte(model.Keyword, model.Type);
            return Json(lst);
        }

        [HttpPost]
        public async Task<JsonResult> SearhStaffAutoComplete([FromBody] AutoCompleteFilterModel model)
        {
            var lst = await userInfoService.SearchUserAutocomplete(model.Keyword);
            return Json(lst);
        }

        [HttpPost]
        public async Task<JsonResult> SearhProductAutoComplete([FromBody] AutoCompleteFilterModel model)
        {
            var lst = await invoiceService.SearchProductAutoComplete(model.Keyword);
            return Json(lst);
        }

        [HttpGet]
        public async Task<JsonResult> PickupProductToInvoiceDetail(int productId)
        {
            var proc = await invoiceService.PickupProductToInvoiceDetail(productId);
            return Json(proc);
        }


        [HttpGet]
        public async Task<JsonResult> GetInvoiceItemDetail(int? invoiceId)
        {
            var lstDetails = await invoiceService.GetInvoiceItemDetail(invoiceId.Value);
            return Json(lstDetails);
        }

        [HttpPost]
        public async Task<DataTableResultModel<InvoiceSearchResultModel>> SearchInvoice(InvoiceFilterModel filter)
        {
            var res =await invoiceService.SearchInvoice(filter);
            return res;
        }



    }
}