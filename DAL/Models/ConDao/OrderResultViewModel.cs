using CommonFW.Domain.Model.Payment;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.ConDao
{
    public class OrderResultViewModel
    {
        public OrderResultViewModel()
        {
            ListSubCode = new List<TicketOrderSubNum>();
            GateListAll = new List<GateList>();
        }
        public DAL.Entities.TicketOrder TicketOrder { get; set; }
        public DAL.Entities.Customer Customer { get; set; }
        public List<TicketOrderSubNum> ListSubCode { get; set; }
        public List<GateList> GateListAll { get; set; }
    }
}
