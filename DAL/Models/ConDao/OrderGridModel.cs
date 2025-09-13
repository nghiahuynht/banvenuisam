using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.ConDao
{
    public class OrderGridModel
    {
        public long Id { get; set; }// orderId
        public string CreatedDate { get; set; }
        public string VisitDate { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string TicketCode { get; set; }
        public string Price { get; set; }
        public int? Quanti { get; set; }
        public string Total { get; set; }
        public string PaymentFee { get; set; }
        public string PaymentMethod { get; set; }
        public int? PaymentStatus { get; set; }
        public string GateName { get; set; }
        public string ObjName { get; set; }
        public string PartnerCode { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal TotalAfterDiscounted { get; set; }
        public string CustomerTypeName { get; set; }
        public string CreatedByName { get; set; }
    }
}
