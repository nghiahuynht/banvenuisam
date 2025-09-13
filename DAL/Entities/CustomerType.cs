using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class CustomerType: BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
    }
}
