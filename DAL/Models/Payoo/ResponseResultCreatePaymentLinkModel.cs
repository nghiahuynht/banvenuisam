using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Payoo
{
    public class ResponseResultCreatePaymentLinkModel
    {
        public string result { get; set; }
        public string checksum { get; set; }
        public OrderInfoReturnModel order { get; set; }
        public string message { get; set; }
        public int? errorcode { get; set; }
    }
}
