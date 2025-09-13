using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class AuthenticatedModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string PartnerCode { get; set; }

    }
}
