using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Invoice
{
    public class ObjAutoCompleteModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
