using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.UserInfo
{
    public class ChangePassModel
    {
        public string UserName { get; set; }
        public string CurrentPass { get; set; }
        public string NewPass { get; set; }
    }
}
