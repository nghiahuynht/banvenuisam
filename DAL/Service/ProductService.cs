using DAL.Entities;
using DAL.IService;
using DAL.Models;
using DAL.Models.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Service
{
    public class ProductService: BaseService,IProductService
    {
        private EntityDataContext dtx;
        public ProductService(EntityDataContext dtx)
        {
            this.dtx = dtx;
        }

      

        public SaveResultModel CreateProduct(Product product,string userName)
        {
            var res = new SaveResultModel();
            try
            {
                product.CreatedBy = userName;
                product.CreatedDate = DateTime.Now;
                dtx.Product.Add(product);
                dtx.SaveChanges();
                res.ValueReturn = product.Id;

            }
            catch(Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }


            return res;
        }
        public SaveResultModel UpdateProduct(Product product,string userName)
        {
            var res = new SaveResultModel();
            try
            {
                product.UpdatedBy = userName;
                product.UpdatedDate = DateTime.Now;
                dtx.Product.Update(product);
                dtx.SaveChanges();
                res.IsSuccess = true;
                res.ValueReturn = product.Id;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        public Product GetProductById(int id)
        {
            var proc = dtx.Product.FirstOrDefault(x => x.Id == id);
            return proc;
        }

        public DataTableResultModel<ProductGridModel> SearchProduct(ProductFilterModel filter)
        {
            var res = new DataTableResultModel<ProductGridModel>();
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@CategoryId", filter.CategoryId),
                new SqlParameter("@Keyword", filter.Keyword),
                new SqlParameter("@Start", filter.start),
                new SqlParameter("@Length", filter.length),
                new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int16, Direction = System.Data.ParameterDirection.Output }
            };
                ValidNullValue(param);
                var lstData = dtx.ProductGridModel.FromSql("sp_SearchProduct @CategoryId,@Keyword,@Start,@Length,@TotalRow OUT", param).ToList();
                res.recordsTotal = Convert.ToInt16(param[4].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData.ToList();
            }
            catch (Exception ex)
            {
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<ProductGridModel>();
            }

            return res;
        }

        public SaveResultModel DeleteProduct(int productId,string userName)
        {
            var res = new SaveResultModel();
            try
            {
                var proc = dtx.Product.FirstOrDefault(x => x.Id == productId);
                proc.IsDeleted = true;
                proc.UpdatedBy = userName;
                proc.UpdatedDate = DateTime.Now;
                dtx.Product.Update(proc);
                dtx.SaveChanges();
               
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
            return res;

        }

      




    }
}
