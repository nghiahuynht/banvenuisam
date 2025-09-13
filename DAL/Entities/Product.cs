using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class Product: EntityCommonField
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string QuyCach { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public decimal? Price { get; set; }
        public string NameNoUniCode { get; set; }
        public string CategoryIds { get; set; }
        public string Note { get; set; }
    }
}
