using DAL.Entities;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IService
{
    public interface ICategoryService
    {
        SaveResultModel CreateCategory(Category category, string userName);
        SaveResultModel UpdateCategory(Category category, string userName);
        Category GetCategoryById(int categoryId);
        List<Category> SearchCategory(string keyword);
        List<Category> LstAllCategoies();
        SaveResultModel DeleteCategory(int categoryId, string userName);
    }
}
