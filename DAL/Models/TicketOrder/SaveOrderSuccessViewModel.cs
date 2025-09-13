using DAL.Models.ConDao;
using System;
using System.Collections.Generic;
using System.Text;



namespace DAL.Models.TicketOrder
{
    public class SaveOrderSuccessViewModel
    {
        public SaveOrderSuccessViewModel()
        {
            LstSubCode = new List<SubOrderPrintModel>();
        }


        public long OrderId { get; set; }
        public long CartLineId { get; set; }
        public List<SubOrderPrintModel> LstSubCode { get; set; }
    }
}
