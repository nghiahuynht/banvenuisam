                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class TicketOrderSubNum
    {
        /// <summary>
        /// ID subOrder
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// ID đơn hàng
        /// </summary>
        public long OrderId { get; set;}
        /// <summary>
        /// số thứ tự vé/ tổng số vé nếu mua vé đoàn
        /// </summary>
        public int SubNum { get; set; }
        /// <summary>
        /// Mã vé
        /// </summary>
        public string TicketCode { get; set; }
        /// <summary>
        /// Mã tra cứu
        /// </summary>
        public string SubOrderCode { get; set; }
        /// <summary>
        /// giá vé
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        ///Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Ngày Hủy
        /// </summary>
        public DateTime? ScanedDate { get; set; }
        /// <summary>
        /// Mã HDDT
        /// </summary>
        public string InvoiceNumber { get; set; }
        public string TransactionID { get; set; }
        /// <summary>
        /// UUID
        /// </summary>
        public Guid? UUID { get; set; }
        /// <summary>
        ///InvSeries
        /// </summary>
        public string InvSeries { get; set; }
        /// <summary>
        /// VAT
        /// </summary>
        public decimal VAT { get; set; }
        /// <summary>
        /// Tổng tiền đã VAT
        /// </summary>
        public decimal TotalAfterVAT { get; set; }
        public long? InvoiceIssued { get; set; }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   