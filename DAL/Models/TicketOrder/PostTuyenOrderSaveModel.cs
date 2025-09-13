using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.TicketOrder
{
    public class PostTuyenOrderSaveModel
    {
        public string GateCode { get; set; }
        public string CustomerCode { get; set; }
        public int ObjType { get; set; }
        public string PrintType { get; set; }
        public int Quanti { get; set; }
        public decimal Price { get; set; }

    }
}
