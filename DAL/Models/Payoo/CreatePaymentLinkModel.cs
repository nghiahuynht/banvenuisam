using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Payoo
{
    public class CreatePaymentLinkModel
    {
        public string data { get; set; }
        public string checksum { get; set; }
        public string refer { get; set; }
        public string payment_group { get; set; }
        public string method { get; set; }
        public string bank { get; set; }
        public int create_ref_qrbank_id { get; set; }
        public int qr_standard { get; set; }        public int create_payment_code { get; set; }
    }
}
