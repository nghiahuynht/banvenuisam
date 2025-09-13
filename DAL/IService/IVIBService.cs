using CommonFW.Domain.Model.Payment;
using DAL.Models;
using DAL.Models.VIB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface IVIBService
    {
        Task<ResCommon<string>> GenerateQRCodeVIB(VirtualAccountModel model);
    }
}
