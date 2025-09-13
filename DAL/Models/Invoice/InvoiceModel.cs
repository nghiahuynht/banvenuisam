using System;
using System.Collections.Generic;
using System.Text;


namespace DAL.Models.Invoice
{
    public class InvoiceModel
    {
        public InvoiceModel()
        {
            InvoiceStatus = Contanst.InvoiceStatus_ConNo;
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public int? ObjId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string StaffCode { get; set; } // user login name
        public string InvoiceStatus { get; set; }
        public string Note { get; set; }
        public string InvoiceType { get; set; }


        


    }
}
