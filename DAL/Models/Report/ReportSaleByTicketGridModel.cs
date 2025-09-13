using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Report
{
    public class ReportSaleByTicketGridModel
    {
        public int Id { get; set; }
        public string TicketCode { get; set; }
        public string Description { get; set; }
        public string TicketGroup { get; set; }
        public int TongSL { get; set; }
        public decimal TongTien { get; set; }
        public decimal TienKM { get; set; }
        public decimal TongTienSauKM { get; set; }
    }
}
