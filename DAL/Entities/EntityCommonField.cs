using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class EntityCommonField:BaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
    }
}
