using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class Supplier: EntityCommonField
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string AreaCode { get; set; }
        public string ProvinceCode { get; set; }
        public bool IsDeleted { get; set; }
    }
}
