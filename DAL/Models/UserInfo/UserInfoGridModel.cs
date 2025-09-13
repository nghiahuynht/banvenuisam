using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.UserInfo
{
    public class UserInfoGridModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string IsActive { get; set; }
        public string Title { get; set; }
    }
}
