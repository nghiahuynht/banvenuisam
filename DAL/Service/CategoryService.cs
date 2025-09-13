using DAL.Entities;
using DAL.IService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Service
{
    public class CategoryService: BaseService, ICategoryService
    {
        private EntityDataContext dtx;
        public CategoryService(EntityDataContext dtx)
        {
            this.dtx = dtx;
        }

        public List<Category> LstAllCategoies()
        {
            var lst = dtx.Category.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList();
            return lst;
        }


        public SaveResultModel CreateCategory(Category category, string userName)
        {
            var res = new SaveResultModel();
            try
            {
                category.CreatedBy = userName;
                category.CreatedDate = DateTime.Now;
                category.IsActive = true;
                dtx.Category.Add(category);
                dtx.SaveChanges();
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
            return res;

        }


        public SaveResultModel UpdateCategory(Category category, string userName)
        {
            var res = new SaveResultModel();
            try
            {
                category.UpdatedBy = userName;
                category.UpdatedDate = DateTime.Now;
                dtx.Category.Update(category);
                dtx.SaveChanges();
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
            return res;

        }


        public Category GetCategoryById(int categoryId)
        {
            var res = dtx.Category.FirstOrDefault(x => x.Id == categoryId);
            return res;
        }

        public List<Category> SearchCategory(string keyword)
        {
            var res = new List<Category>();
            if (!string.IsNullOrEmpty(keyword))
            {
                res = dtx.Category.Where(x => x.Name.Contains(keyword) && !x.IsDeleted).OrderBy(x => x.Id).ToList();
            }
            else
            {
                res = dtx.Category.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList();
            }
            return res;
        }

        public SaveResultModel DeleteCategory(int categoryId, string userName)
        {
            var res = new SaveResultModel();
            try
            {
                var cate = dtx.Category.FirstOrDefault(x => x.Id == categoryId);
                cate.IsDeleted = true;
                cate.UpdatedBy = userName;
                cate.UpdatedDate = DateTime.Now;
                dtx.Category.Update(cate);
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
