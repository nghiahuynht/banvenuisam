using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Customer
{
    public class CustomerGridModel
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string TaxCode { get; set; }
        public string TaxAddress { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string CustomerTypeName { get; set; }
    }
    
    
}
