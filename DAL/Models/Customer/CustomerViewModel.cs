using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Customer
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {
            Customer = new Entities.Customer();
            ListCustType = new List<CustomerType>();
            ListAllArea = new List<Area>();
            ListAllProvince = new List<Province>();
        }
        public DAL.Entities.Customer Customer { get; set; }
        public List<CustomerType> ListCustType { get; set; }
        public List<Area> ListAllArea { get; set; }
        public List<Province> ListAllProvince { get; set; }
    }
}
