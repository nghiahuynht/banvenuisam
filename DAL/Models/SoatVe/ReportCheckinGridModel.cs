using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.SoatVe
{
    public class ReportCheckinGridModel
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string TicketCode { get; set; }
        public string ViettelCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime ScanDate { get; set; }
        public string CheckinDate { get; set; }
    }
}
