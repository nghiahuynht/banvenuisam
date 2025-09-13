using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class MenuRole: BaseEntity
    {
       
        public string RoleCode { get; set; }
        public int MenuId { get; set; }

    }
}
