using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class Menu: BaseEntity
    {
        public string Name { get; set; }       
        public int Parent { get; set; }
        public string MenuIcon { get; set; }
        public string URL { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; }
    }
}
