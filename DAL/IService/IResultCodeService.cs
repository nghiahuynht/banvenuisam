using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IService
{
    public interface IResultCodeService
    {
        ResultCode GetResultCodeByCode(int code);
    }
}
