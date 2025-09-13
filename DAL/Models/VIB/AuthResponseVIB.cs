using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.VIB
{
    public class AuthVIBResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

    public class VirtualAccountModel
    {
        public string CIF { get; set; }
        public string VirtualAccount { get; set; }
        public string ReqId { get; set; } // id of ticketorder
        public string Code { get; set; } // 
        public string Name { get; set; } // customer name
        public string IsvalidAmount { get; set; }
        public string Amount { get; set; }// total
    }

    public class ResponseCreateirtualAccount
    {
        public ResultDataCreateVA Result { get; set; }
    }

    public class ResultDataCreateVA
    {
        public string STATUSCODE { get; set; }
    }

    public class QrDataResponse
    {
        public ResultData Result { get; set; }
    }

    public class ResultData
    {
        public string STATUSCODE { get; set; }
        public QrData DATA { get; set; }
    }

    public class QrData
    {
        public string requestID { get; set; }
        public string qrData { get; set; }
        public string qrImage { get; set; }
        public string accountNo { get; set; }
        public string amount { get; set; }
        public string description { get; set; }
        public bool isGenQR { get; set; }
    }

}
