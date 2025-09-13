using DAL.Models;
using DAL.Models.Zalo;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IService
{
    public interface IZaloService
    {
        /// <summary>
        /// gửi Zalo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResCommon<int> SendZalo(SendZNSModel model);

        void ChangeConfigNotify(string columnName, string value, string phoneReceived);
        ZaloNotiConfigModel GetZaloConfigNoti();


    }
}
