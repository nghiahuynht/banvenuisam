using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class UserInfo:EntityCommonField
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public bool IsActive { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string RoleCode { get; set; }
        public bool IsPartner { get; set; }
        public string PartnerCode { get; set; }
    }
}
