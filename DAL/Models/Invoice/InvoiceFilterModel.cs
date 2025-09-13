using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Invoice
{
    public class InvoiceFilterModel: DataTableDefaultParamModel
    {
        public string InvoiceType { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Keyword { get; set; }



    }
}
