using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Ticket
{
    public class PricePolicyFilterModel: DataTableDefaultParamModel
    {
        public string TicketCode { get; set; }
    }
}
