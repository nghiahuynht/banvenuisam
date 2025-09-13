using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Supplier
{
    public class SupplierViewModel
    {
        public SupplierViewModel()
        {
            Supplier = new Entities.Supplier();
            ListAllArea = new List<Area>();
            ListAllProvince = new List<Province>();
        }
        public DAL.Entities.Supplier Supplier { get; set; }
        public List<Area> ListAllArea { get; set; }
        public List<Province> ListAllProvince { get; set; }
    }
}
