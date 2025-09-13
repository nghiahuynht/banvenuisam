using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Zalo
{
    /// <summary>
    /// 
    /// </summary>
    public class SendZNSModel
    {
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// mã tra cứu
        /// </summary>
        public string SubOrderCode { get; set; }
        /// <summary>
        /// Giá vé đã VAT
        /// </summary>
        public string StrPrice { get; set; }
        /// <summary>
        /// Số lượng vé
        /// </summary>
        public int Quanti { get; set; } = 1;
        /// <summary>
        /// Tổng tiền vé đã VAT
        /// </summary>
        public string StrTotal { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public string StrCreatedDate { get; set; }
        /// <summary>
        /// URL image QR
        /// </summary>
        public string UrlQRCode { get; set; }
        public string PhoneNumber { get; set; }
        /// <summary>
        ///id suborderId
        /// </summary>
        public long SubOrderId { get; set; }
        /// <summary>
        /// Ngày thăm quan
        /// </summary>
        public string StrVisitDate { get; set; }
        /// <summary>
        /// Điểm thăm quan
        /// </summary>
        public string GateName { get; set; }
    }

    public class ResTokenDto
    {
        public string error { get; set; }
        public ResDataToken data { get; set; }
        public string trace_id { get; set; }

        public class ResDataToken
        {
            public int status { get; set; }
            public string message { get; set; }
            public ResponseDto response { get; set; }
            
            public class ResponseDto
            {
                public string status { get; set; }
                public string message { get; set; }
                public DataResponseDto data { get; set; }

                public class DataResponseDto
                {
                    public string IsToken { get; set; }
                    public string Createat { get; set; }
                    public string Expried { get; set; }
                    public bool IsLonglive { get; set; }
                }
                
            }
        }
    }
    public class ResZNSDto
    {
        public string error { get; set; }
        public ResDataToken data { get; set; }
        public string trace_id { get; set; }

        public class ResDataToken
        {
            public int status { get; set; }
            public string message { get; set; }
            public ResponseDto response { get; set; }

            public class ResponseDto
            {
                public int status { get; set; }
                public string message { get; set; }
                public List<string> data { get; set; }
                public RsMetaDto  meta { get; set; }
                public string trace_id { get; set; }

                public class RsMetaDto
                {
                    public int limit { get; set; }
                    public int offset { get; set; }
                    public int total { get; set; }
                }

            }
        }
    }
}
