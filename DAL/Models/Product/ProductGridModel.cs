using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Product
{
    public class ProductGridModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string QuyCach { get; set; }
        public string IsActive { get; set; }
        public string Price { get; set; }
        public string CategoryName { get; set; }
        public string Note { get; set; }
    }
}
