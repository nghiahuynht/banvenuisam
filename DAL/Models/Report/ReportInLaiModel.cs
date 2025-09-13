using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Report
{
    public class ReportInLaiModel
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string TicketCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public int Quanti { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
}
}
