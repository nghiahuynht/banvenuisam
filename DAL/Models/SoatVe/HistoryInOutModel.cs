using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.SoatVe
{
    public class HistoryInOutModel
    {
        public string id { get; set; }
        public string create_time { get; set; }
        public string update_time { get; set; }
        public string first_in_time { get; set; }
        public string last_name { get; set; }
        public string last_out_time { get; set; }
        public string name { get; set; }
        public string subcode { get; set; }
        public string reader_name_in { get; set; }
        public string reader_name_out { get; set; }
    }
}
