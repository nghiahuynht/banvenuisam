using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Report
{
    public class SaleTicketMisaStatusModel
    {
        public string TicketCode { get; set; }
        public string KyHieu { get; set; }
        public string MauSoBienLai { get; set; }
        public int QuantiPrint { get; set; }
        public int QuantiMisa { get; set; }
        public int QuantiRemain { get; set; }

    }
}
