using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.GatePermission
{
   public class GatePermissionGridModel
    {
        public string GateCode { get; set; }
        public string GateName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
