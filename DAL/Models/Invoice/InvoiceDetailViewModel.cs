using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models.Invoice;

namespace DAL.Models.Invoice
{
    public class InvoiceDetailViewModel
    {
        public InvoiceDetailViewModel()
        {
            ObjSelected = new ObjAutoCompleteModel();
            StaffSelected = new Entities.UserInfo();
            InvoiceDetails = new List<DAL.Models.Invoice.InvoiceDetailGridModel>();
        }
        public InvoiceModel Invoice { get; set; }
        public ObjAutoCompleteModel ObjSelected { get; set; }
        public DAL.Entities.UserInfo StaffSelected { get; set; }
        public List<DAL.Models.Invoice.InvoiceDetailGridModel> InvoiceDetails { get; set; }
    }
}
