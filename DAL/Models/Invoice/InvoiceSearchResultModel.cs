using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Invoice
{
    public class InvoiceSearchResultModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceTotal { get; set; }
        public string InvoiceStatus { get; set; }
        public string ObjName { get; set; }
        public string Note { get; set; }
    }
}
