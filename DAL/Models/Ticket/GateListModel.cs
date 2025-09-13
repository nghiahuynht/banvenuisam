using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Ticket
{
    public class GateListModel
    {
        public string GateCode { get; set; }
        public string GateName { get; set; }
        public int? OrderNum { get; set; }
        public decimal? Price { get; set; }
    }
}
