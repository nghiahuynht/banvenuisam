using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.TicketOrder
{
    public class UpdateCustForOrderModel
    {
        public long OrderId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
    }
}
