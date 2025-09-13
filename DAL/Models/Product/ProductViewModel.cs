using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;

namespace DAL.Models.Product
{
    public class ProductViewModel
    {
        public DAL.Entities.Product Product { get; set; }
        public List<DAL.Entities.Category> Categories { get; set; }
    }
}
