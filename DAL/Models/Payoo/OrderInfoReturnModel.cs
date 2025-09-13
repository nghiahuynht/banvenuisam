using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Payoo
{
    public class OrderInfoReturnModel
    {
        public long order_id { get; set; }
        public string order_no { get; set; }
        public string amount { get; set; }
        public string payment_code { get; set; }
        public string expiry_date { get; set; }
        public string token { get; set; }
        public string payment_url { get; set; } // payment_url&locale=vi_VN  nếu hiển thị lang = Vietnam
        public QRBankInfoModel qrbank_info { get; set; }
    }
}
