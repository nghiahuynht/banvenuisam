using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.TicketOrder
{
    public class ScanResultModel
    {
        public string TicketCode { get; set; }
        public string SaleDate { get; set; }
        public int Quanti { get; set; }
        public string TicketType { get; set; }
        public string ZoneName { get; set; }
        public string ResultScan { get; set; }
        public string Error { get; set; }
        public string Validscan { get; set; }
    }
}
