using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.UserInfo
{
    public class UserInfoFilterModel: DataTableDefaultParamModel
    {
        public string RoleCode { get; set; }
        public string Keyword { get; set; }
    }
}
