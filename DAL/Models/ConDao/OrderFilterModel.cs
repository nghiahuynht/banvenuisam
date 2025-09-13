using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.ConDao
{
    public class OrderFilterModel: DataTableDefaultParamModel
    {
        public int ChanelId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }
        public string GateCode { get; set; }
        public string Keyword { get; set; }
        public string PartnerCode { get; set; }
        public string CustomerType { get; set; }
        public string GroupTicket { get; set; }
        public string ObjType { get; set; }
        public string TicketCode { get; set; }
        public bool IsFree { get; set; }
        public string UserName { get; set; }

    }
}
