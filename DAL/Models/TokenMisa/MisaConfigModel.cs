using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.TokenMisa
{
    public class MisaConfigModel
    {
        public string apiAddress { get; set; }
        public string appId { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public string taxCode { get; set; }
        public string addressBienLai { get; set; }
        public string addressToken { get; set; }
        public string HSMUser { get; set; }
        public string HSMPass { get; set; }

    }
}
