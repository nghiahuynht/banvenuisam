using System;
using System.Collections.Generic;
using System.Text;

namespace GM.CORE.Commons
{
    public abstract class AuditableBase
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string  UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
