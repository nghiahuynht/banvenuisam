using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Report
{
    public class ReportBanVeByCustTypeGridModel
    {
        public Int64 STT { get; set; }
        public string CustTypeName { get; set; }
        public int QuantiSale { get; set; }
        public decimal TotalSale { get; set; }
    }
}
