using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.SoatVe
{
    public class InOutFilterModel: DataTableDefaultParamModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Keyword { get; set; }
    }
}
