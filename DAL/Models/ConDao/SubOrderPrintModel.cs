using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.ConDao
{
    public class SubOrderPrintModel
    {
        public long SubId { get; set; }
        public int SubNum { get; set; }
        public string SubOrderCode { get; set; }
        public string CreatedBy { get; set; }
        public string CardNum { get; set; }
    }
}
