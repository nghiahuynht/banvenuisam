using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class DataTableResultModel<T>
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<T> data { get; set; }
        public string error { get; set; }
    }
}
