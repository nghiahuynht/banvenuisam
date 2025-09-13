using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Payoo
{
    public class WaitingPaymentOrderModel
    {
        public long OrderId { get; set; }
        public string ResponseJson { get; set; }
        public string TicketCode { get; set; }
        public int Quanti { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TotalAfterDiscounted { get; set; }
    }
}
