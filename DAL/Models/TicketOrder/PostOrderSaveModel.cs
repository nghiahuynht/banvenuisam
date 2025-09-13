using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.TicketOrder
{
    public class PostOrderSaveModel
    {
        public string TicketCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerType { get; set; }
        public string ObjType { get; set; }
        public int Quanti { get; set; }
        public decimal Price { get; set; }
        public string BienSoXe { get; set; }
        public string GateName { get; set; }
        public bool IsFree { get; set; }
        public string PrintType { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountValue { get; set; }
        public string TienKhachDua { get; set; }
        public string PaymentType { get; set; }
        public long CartLineId { get; set; }
    }
}
