using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Category
{
    public class CategoryDetailViewModel
    {
        public CategoryDetailViewModel()
        {
            ListParent = new List<Entities.Category>();
        }
        public DAL.Entities.Category Category { get; set; }
        public List<DAL.Entities.Category> ListParent { get; set; }
    }
}
