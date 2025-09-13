using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Promotion
{
    public class PromotionFilterModel: DataTableDefaultParamModel
    {
        //public DateTime? FromDate { get; set; }
        //public DateTime? ToDate { get; set; }
        public string Keyword { get; set; }
    }

}
