using DAL.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Ticket
{
    public class TicketModel
    {
        /// <summary>
        /// ID vé
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Mã vé
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// giá vé
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Tên chi nhánh/ địa điểm
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// Mẫu biên lai
        /// </summary>
        public string BillTemplate { get; set; }
        /// <summary>
        /// Mẫu hợp đồng điện tử
        /// </summary>
        public string EContractTemplate { get; set; }
        /// <summary>
        /// Tình trạng vé: true: xóa, false chưa xóa
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Người tạp
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public string CreatedDate { get; set; }
        /// <summary>
        /// Người cập nhật
        /// </summary>
        public string UpdatedBy { get; set; }
        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public string UpdatedDate { get; set; }
    }

    public class ReqSaveTicket
    {
        /// <summary>
        /// Loại KH
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// Loại In
        /// </summary>
        public string LoaiInCode { get; set; }
        /// <summary>
        /// Số lượng vé
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Mã vé
        /// </summary>
        public string TicketCode { get; set; }
    }

    public class ReqCreateOrder
    {
        /// <summary>
        /// ID đơn hàng
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// Họ tên KH
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        ///SĐT KH
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Số lượng vé
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Mã vé
        /// </summary>
        public string TicketCode { get; set; }
        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string AgencyName { get; set; }
        /// <summary>
        /// Mã số thuế
        /// </summary>
        public string TaxCode { get; set; }
        /// <summary>
        /// Địa chỉ xuất hóa đơn
        /// </summary>
        public string TaxAddress { get; set; }
        /// <summary>
        /// Ngày dự kiến thăm quan
        /// </summary>
        public DateTime? VisitDate { get; set; }
        public string GateCode { get; set; }
    }

    public class SaleHistoryFilterModel: DataTableDefaultParamModel
    {
        public int SaleChanelId { get; set; }
        /// <summary>
        /// UserName Nhân viên mặc định rỗng : tất cả
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Từ ngày
        /// </summary>
        public string FromDate { get; set; }
        /// <summary>
        /// Đến ngày
        /// </summary>
        public string ToDate { get; set; }
        public string GateCode { get; set; }
        /// <summary>
        /// mã vé
        /// </summary>
        public string TicketCode { get; set; }
        public string Keyword { get; set; }
        public string PaymentType { get; set; }
    }

    public class ResOrderInfoDto
    {
        public long Id { get; set; }
        /// <summary>
        /// Loại vé
        /// </summary>
        public string LoaiIn { get; set; }
        public string LoainInName
        {
            get
            {
                if(LoaiIn == Contanst.LoaiIn_VeDoan)
                {
                    return "Vé đoàn";
                }
                else
                {
                    return "Vé lẻ";
                }
            }
        }
        /// <summary>
        /// Mã vé
        /// </summary>
        public string TicketCode { get; set; }
        /// <summary>
        /// Giá vé đã VAT
        /// </summary>
        public decimal Price { get; set; } = 0;
        public string StrPrice { get { return this.Price.ToString("N0"); } }
        /// <summary>
        /// Số lượng vé
        /// </summary>
        public int Quanti { get; set; } = 1;
        /// <summary>
        /// Tổng tiền vé đã VAT
        /// </summary>
        public decimal Total { get; set; } = 0;
        /// <summary>
        /// Tổng tiền vé đã VAT
        /// </summary>
        public string StrTotal { get { return this.Total.ToString("N0"); } }
        /// <summary>
        /// Mã KH
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// Tên KH
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// SĐT KH
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Email Kh
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Mã số thuế
        /// </summary>
        public string TaxCode { get; set; }
        /// <summary>
        /// Địa chỉ xuaasrt hóa đơn
        /// </summary>
        public string TaxAddress { get; set; }
        /// <summary>
        /// tên đơn vị
        /// </summary>
        public string AgencyName { get; set; }
        public string SubOrderCode { get; set; }
        /// <summary>
        /// Phia thanh toasn
        /// </summary>
        public decimal PaymentFee { get; set; }
        /// <summary>
        /// loại PTTT
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        /// Tình trạng thanh toán
        /// </summary>
        public int PaymentStatus { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public string StrCreatedDate { get { return CreatedDate?.ToString("dd/MM/yyyy HH:mm:ss"); } }
        /// <summary>
        /// SubOrderCodeId
        /// </summary>
        public long SubOrderCodeId { get; set; }
        /// <summary>
        /// Ngày dự kiến tham quan
        /// </summary>
        public DateTime? VisitDate { get; set; }
        /// <summary>
        /// str Ngày dự kiến tham quan
        /// </summary>
        public string StrVisitDate { get { return VisitDate.HasValue ? VisitDate.Value.ToString("dd/MM/yyyy") : string.Empty; } }

        public string TicketDescription { get; set; }
        public string PartnerCode { get; set; }
        public decimal? DiscountedAmount { get; set; } = 0;
        public decimal? TotalAfterDiscounted { get; set; } = 0;

    }

    public class ReqPaymentDto
    {
        /// <summary>
        /// ID đơn hàng
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        ///loại thanh toán
        /// </summary>
        public PaymentTypeEnum PaymentType { get; set; }
    }

    public class ResOrderInfoSendZaloDto
    {
       
        public string SubOrderCode { get; set; }
        public long SubOrderCodeId { get; set; }
        
    }

    public class ReqSaveTicketUser
    {
        public string UserId { get; set; }
        public List<string> TicketIds { get; set; }
    }
}
