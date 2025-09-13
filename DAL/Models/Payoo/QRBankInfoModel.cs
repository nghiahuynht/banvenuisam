using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Payoo
{
    public class QRBankInfoModel
    {
        public string qr_string { get; set; }
        public string qr_code_uri { get; set; }
        public string qr_code { get; set; }
        public string bank_code { get; set; }
        public string bank_name { get; set; }
        public string bank_account_no { get; set; }
        public string bank_account_owner { get; set; }
        public string description { get; set; }
        public string bank_logo { get; set; }
    }
}
