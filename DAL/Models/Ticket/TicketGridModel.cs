using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models.Ticket
{
    public class TicketGridModel
    {
        /// <summary>
        /// id vé
        /// </summary>
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string MauSoBienLai { get; set; }
        public string KyHieu { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Loại vé: vé lẻ / vé đoàn
        /// </summary>
        public string LoaiIn { get; set; }
        public string AreaName { get; set; }
    }

    public class SaleHistoryGridModel
    {
        /// <summary>
        /// STT
        /// </summary>
        public long RowNumber { get; set; }
        /// <summary>
        /// id subOrder
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Mã vé
        /// </summary>
        public string TicketCode { get; set; }
        /// <summary>
        /// Mã tra cứu
        /// </summary>
        public string SubOrderCode { get; set; }
        /// <summary>
        /// Loại vé
        /// </summary>
        public string LoaiIn { get; set; }
        /// <summary>
        /// Tên KH
        /// </summary>
        public string CustomerName { get; set; }
        public string ObjtypeName { get; set; }
        /// <summary>
        /// Đơn giá
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// số lượng
        /// </summary>
        public int Quanti { get; set; }
        /// <summary>
        /// thành tiền 
        /// </summary>
        public decimal TotalAfterVAT { get; set; }
        /// <summary>
        /// Ngày bán
        /// </summary>
        public string CreatedDate { get; set; }

        /// <summary>
        /// Người bán
        /// </summary>
        public string FullName { get; set; }
        public string InvoiceNumber { get; set; }
        public string TransactionID { get; set; }
        public string GateName { get; set; }
        public Int64 OrderId { get; set; }
    }

    public class SaleReportGridModel
    {
        /// <summary>
        /// STT
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Tên Đăng nhập
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Tên người dùng
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Mã vé
        /// </summary>
        public string TicketCode { get; set; }
        /// <summary>
        /// Loại vé
        /// </summary>
        public string LoaiIn { get; set; }
        /// <summary>
        /// Tổng số lượng
        /// </summary>
        public int NumQuanTi { get; set; }
        /// <summary>
        /// Tổng tiền
        /// </summary>
        public decimal TotalVAT { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal TotalAfterDiscounted { get; set; }
    }

    public class SoatVeReportGridModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long STT { get; set; }
        public string TicketCode { get; set; }
        public int SLBan { get; set; }
        public string SubOrderCode { get; set; }
        public string NgaySoatVe { get; set; }
        public string NgayMuaVe { get; set; }
        public string DiaDiem { get; set; }
        public string ResultScan { get; set; }
        public string Gate { get; set; }
    }

    public class TicketOrderModel
    {
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

    }
}
