using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.SoatVe
{
   public class SoatVeOfflineModel
    {
        public string ResultScan { get; set; }
        public DateTime ScanDate { get; set; }
        public string ZoneCode { get; set; }
        public int SubId { get; set; }
        public string Validscan { get; set; }
        public string GateCode { get; set; }
    }

    public class ResultSoatVeOfflineModel
    {
        public int SubId { get; set; }
        public Boolean IsSuccess { get; set; }
    }
}
