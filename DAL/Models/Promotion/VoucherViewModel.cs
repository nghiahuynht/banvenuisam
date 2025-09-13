using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Promotion
{
   public class InfoVoucherViewModel
    {
        public string VoucherCode { get; set; }
        public decimal? VoucherValue { get; set; }
        public bool? IsPercent { get; set; }
        public int? PercentValue { get; set; }
    }
}
