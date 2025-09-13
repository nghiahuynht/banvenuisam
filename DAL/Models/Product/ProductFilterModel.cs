using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Product
{
    public class ProductFilterModel: DataTableDefaultParamModel
    {
        public int CategoryId { get; set; }
        public string Keyword { get; set; }
    }
}
