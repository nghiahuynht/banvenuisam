using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class SaveResultModel
    {
        public SaveResultModel()
        {
            ValueReturn = 0;
            IsSuccess = true;
        }
        public int ValueReturn { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

    }

    public class ResultModel
    {
        public ResultModel()
        {
            ValueReturn = 0;
            IsSuccess = true;
        }
        public long ValueReturn { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

    }

    public class ResCommon<T>
    {
        public ResCommon()
        {
            IsSuccess = true;
            Desc = "Thành công";
        }
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Desc { get; set; }

    }
}
