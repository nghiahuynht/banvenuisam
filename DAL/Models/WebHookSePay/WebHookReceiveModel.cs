using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.WebHookSePay
{
    public class WebHookReceiveModel
    {
        public int id { get; set; }
        public string gateway { get; set; }
        public DateTime? transactionDate { get; set; }
        public string accountNumber { get; set; }
        public decimal? transferAmount { get; set; }
        public decimal? accumulated { get; set; }
        public string code { get; set; }
        public string content { get; set; }
        public string referenceCode { get; set; }
    }
}
