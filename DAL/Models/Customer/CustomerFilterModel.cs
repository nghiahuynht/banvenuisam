using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Customer
{
    public class CustomerFilterModel: DataTableDefaultParamModel
    {
        //public int CustomerTypeId { get; set; }
        //public string ProvinceCode { get; set; }
        public string Keyword { get; set; }
    }
}
