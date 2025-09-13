using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Ticket
{
   public class TicketUserModel
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
    }
}
