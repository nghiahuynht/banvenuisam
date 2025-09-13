using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class Category:EntityCommonField
    {
        public string Name { get; set; }
        public int Parent { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
