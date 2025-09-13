using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Invoice
{
    public class InvoiceDetailGridModel
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode  {get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public int Quanti { get; set; }
        public decimal Total { get; set; }
    }
}
