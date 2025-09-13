using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class InvoiceDetail:BaseEntity
    {
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public int Quanti { get; set; }
        public decimal Total { get; set; }
    }
}
