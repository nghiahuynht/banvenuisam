using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.IService;
using DAL.Models;
using DAL.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private IProductService productService;
        private ICategoryService categoryService;
        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }
        public IActionResult Index()
        {
            var lstCategories = categoryService.LstAllCategoies();
            return View(lstCategories);
        }
        public IActionResult ProductDetail(int? id)
        {
            var viewModel = new ProductViewModel();
            if (id.HasValue)
            {
                viewModel.Product = productService.GetProductById(id.Value);
            }
            else
            {
                viewModel.Product = new DAL.Entities.Product();
            }
            viewModel.Categories = categoryService.LstAllCategoies();
            return View(viewModel);
        }
        [HttpPost]
        public JsonResult SaveProduct([FromBody] Product model)
        {
            var res = new SaveResultModel();
            if (model.Id == 0)
            {
                res = productService.CreateProduct(model, User.Identity.Name);
            }
            else
            {
                res = productService.UpdateProduct(model, User.Identity.Name);
            }
            return Json(res);
        }

        [HttpPost]
        public DataTableResultModel<ProductGridModel> SearchProduct(ProductFilterModel filter)
        {
            var res = productService.SearchProduct(filter);
            return res;
        }

        [HttpPost]
        public JsonResult DeleteProduct(int id)
        {
            var res = productService.DeleteProduct(id, User.Identity.Name);
            return Json(res);
        }


    }
}