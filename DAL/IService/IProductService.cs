using DAL.Entities;
using DAL.Models;
using DAL.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface IProductService
    {
        SaveResultModel CreateProduct(Product product, string userName);
        SaveResultModel UpdateProduct(Product product, string userName);
        Product GetProductById(int id);
        DataTableResultModel<ProductGridModel> SearchProduct(ProductFilterModel filter);
        SaveResultModel DeleteProduct(int productId, string userName);
    }
}
