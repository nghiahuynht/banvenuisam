using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    /// <summary>
    /// Bán vé
    /// </summary>
    public class TicketOrder
    {
        public TicketOrder() { }
        /// <summary>
        /// ID đơn hàng
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Mã vé
        /// </summary>
        public string TicketCode { get; set; }
        /// <summary>
        /// Mã KH
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// Tên KH
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// Giá vé đã VAT
        /// </summary>
        public decimal Price { get; set; } = 0;
        /// <summary>
        /// Số lượng vé
        /// </summary>
        public int Quanti { get; set; } = 0;
        /// <summary>
        /// Tổng tiền vé đã VAT
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Người Tạo
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Mã HDDT
        /// </summary>
        public string MaHDDT { get; set; }
        /// <summary>
        /// Loại KH
        /// </summary>
        public string CustomerType { get; set; }
        /// <summary>
        /// Biển số xe
        /// </summary>
        public string BienSoXe { get; set; }
       
        /// <summary>
        /// Kênh bán Online/offline
        /// </summary>
        public int SaleChannelId { get; set; }
        /// <summary>
        /// tình trạng thanh toán: 1 đã thanh toán, 0 chưa thanh toán, 2 Hủy
        /// </summary>
        public int PaymentStatus { get; set; }
        /// <summary>
        /// PTTT
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        /// Ngày thanh toán thành công
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        /// <summary>
        /// json nhận được khi call đến api webhook_handler
        /// </summary>
        public string PaymentValue { get; set; }
        /// <summary>
        /// id của payment trả về từ webhook_handler
        /// </summary>
        public long PaymentId { get; set; }

        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
        /// <summary>
        /// Người cập nhật
        /// </summary>
        public string UpdatedBy { get; set; }
        public decimal PaymentFee { get; set; }
        /// <summary>
        /// Ngày dự kiến thăm quan
        /// </summary>
        public DateTime? VisitDate { get; set; }
        public string GateName { get; set; }
        public string ObjType { get; set; }
        public string OrderStatus { get; set; }
        public string PrintType { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal TotalAfterDiscounted { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsFree { get; set; }
    }
}
