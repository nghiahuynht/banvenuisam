using DAL.Models.Ticket;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models.TicketOrder
{
    public class PrintPdfOrderModel
    {
        public long OrderId { get; set; }
        public long SubId { get; set; }
        public string TicketCode { get; set; }
        public string SubOrderCode { get; set; }
        public decimal Price { get; set; }
        public int Quanti { get; set; }
        public decimal TotalAfterVAT { get; set; }
        public string KyHieu {get;set;}
        public string TotalByText { get; set; }
        public DateTime? CreatedDate { get; set; }

        // ddphuong add prop
        public string ObjDesc { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }

       // Nghia
        public string CustomerTypeName { get; set; }
        public string ObjTypeName { get; set; }
        public string TicketGroup { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountedAmount { get; set; }
        public string PaymentValue { get; set; }
        public decimal PaymentRemain { get; set; }
        public string PaymentType { get; set; }
        public decimal? TotalAfterDiscounted { get; set; }
        public string ViettelCode { get; set; }

        [NotMapped]
        public List<GateListModel> ListGate { get; set; }= new List<GateListModel>();
        /// <summary>
        /// tên điểm tham quan
        /// </summary>
        public string AreaName { get; set; }
    }
}
