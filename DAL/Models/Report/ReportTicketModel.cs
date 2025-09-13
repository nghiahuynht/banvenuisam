using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Report
{
    public class TicketTypeRPFilter
    {
        /// <summary>
        /// nhân viên
        /// </summary>
        public string UserName { get; set; }
        // <summary>
        /// từ ngày
        /// </summary>
        public string FromDate { get; set; }
        /// <summary>
        /// đến ngày
        /// </summary>
        public string ToDate { get; set; }
        /// <summary>
        /// Mã vé
        /// </summary>
        public string Keyword { get; set; }
    }

    public class ResGetRPSales
    {
        /// <summary>
        /// SL bán
        /// </summary>
        public int? SellQuantity { get; set; } = 0;
        /// <summary>
        /// Tổng doanh thu
        /// </summary>
        public decimal? TotalSales { get; set; } = 0;
        /// <summary>
        /// Tổng doanh thu
        /// </summary>
        public string StrTotalSales { get { return TotalSales.HasValue ? TotalSales.Value.ToString("N0") : "0"; } }
        /// <summary>
        /// dữ liệu
        /// </summary>
        public List<MockupData> Data { get; set; }
    }
    //
    public class MockupData
    {
        public string GateCode { get; set; }
        public string GateName { get; set; }
        public int Price { get; set; }
        public int SlSale { get; set; }
        public int AmountSale { get; set; }
    }


    public class ResGetRPByPartner
    {
        /// <summary>
        /// SL bán
        /// </summary>
        public int? SLBan { get; set; } = 0;
        /// <summary>
        /// Tổng doanh thu
        /// </summary>
        public decimal? TongDoanhSo { get; set; } = 0;
        /// <summary>
        /// Tổng doanh thu
        /// </summary>

        public List<SaleReportByPartnerGridModel> Data { get; set; }
    }

    public class SaleReportByPartnerGridModel
    {
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public int SLBan { get; set; }
        public decimal TongDoanhSo { get; set; }
    }

    public class PrintAgainRPFilter: DataTableDefaultParamModel
	{
		public string UserName { get; set; }
		// <summary>
		/// từ ngày
		/// </summary>
		public string FromDate { get; set; }
		/// <summary>
		/// đến ngày
		/// </summary>
		public string ToDate { get; set; }
        /// <summary>
		/// Loại thanh toán
		/// </summary>
		public string PaymentType { get; set; }
	}

}
