using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Email
{
    public class EmailNofiModel
    {
        public string From { get; set; }
        public string ToList { get; set; }
        public string CCList { get; set; }
        public string Subject { get; set; }
        public string EmailContent { get; set; }
    }
}
