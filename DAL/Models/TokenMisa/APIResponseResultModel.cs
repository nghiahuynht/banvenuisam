using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.TokenMisa
{
    public class APIResponseResultModel
    {
        public APIResponseResultModel()
        {
            Errors = new List<string>();
            Success = false;
        }
        public bool Success { get; set; }
        public string Data { get; set; }
        public string NewData { get; set; }
        public List<string> Errors { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorCodeDetail { get; set; }
    }
}
