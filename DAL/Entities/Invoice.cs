using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class Invoice:EntityCommonField
    {
        public string Code { get; set; }
        public int? ObjId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string StaffCode { get; set; } // user login name
        public string InvoiceStatus { get; set; }
        public string Note { get; set; }
        public string InvoiceType { get; set; }




    }
}
