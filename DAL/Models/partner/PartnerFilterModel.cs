using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.partner
{
   public class PartnerFilterModel: DataTableDefaultParamModel
    {
        public string ParnerCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
