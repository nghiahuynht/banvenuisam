using DAL.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IService
{
    public interface IEmailService
    {
        /// <summary>
        /// Gửi Email
        /// </summary>
        /// <param name="orderId"></param>
        void SendEMail(ResOrderInfoDto orderDTO);
    }
}
