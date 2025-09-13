using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.TicketOrder
{
    public class DetailCheckTicketModel
    {
        public int TotalCheck { get; set; }
        public int TotalSuccess { get; set; }
        public int TotalFail { get; set; }
    }
}
