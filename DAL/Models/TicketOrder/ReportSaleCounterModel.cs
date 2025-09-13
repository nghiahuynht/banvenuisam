using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.TicketOrder
{
    public class ReportSaleCounterModel
    {
        public int Id { get; set; }
        public string TotalQuantiPrint { get; set; }
        public string TotalQuantiOnTicket { get; set; }
        public string TotalVAT { get; set; }
    }
}
