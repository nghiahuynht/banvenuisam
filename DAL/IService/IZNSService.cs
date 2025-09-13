using DAL.Models;
using DAL.Models.Ticket;
using DAL.Models.Zalo;
using DAL.Models.ZNS;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IService
{
    public interface IZNSService
    {
        AccessTokenZNSDto GetAccessTokenZNS();
        ResCommon<int> SendZalo(SendZNSModel model);
        ResOrderInfoDto GetOrderInfo(long orderId);
        ResCommon<int> SendZaloTicketOrderSuccess(long orderId);
    }
}
