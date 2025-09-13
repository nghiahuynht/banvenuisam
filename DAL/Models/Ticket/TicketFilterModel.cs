using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Ticket
{
    public class TicketFilterModel: DataTableDefaultParamModel
    {
        /// <summary>
        /// Id chi nhánh
        /// </summary>
        public string Area { get; set; }
        public string TicketGroup { get; set; }
        // <summary>
        /// Mã code Ticket
        /// </summary>
        public string Keyword { get; set; }
        //public DateTime FromDate { get; set; }
        //public DateTime ToDate { get; set; }
    }

    
    public class SaleReportFilterModel : DataTableDefaultParamModel
    {
        public int SaleChanelId { get; set; }
        /// <summary>
        /// nhân viên
        /// </summary>
        public string UserName { get; set; }
        // <summary>
        /// từ ngày
        /// </summary>
        public DateTime? FromDate { get; set; }
        /// <summary>
        /// đến ngày
        /// </summary>
        public DateTime? ToDate { get; set; }
        /// <summary>
        /// Mã vé
        /// </summary>
        public string TicketCode { get; set; }
        /// <summary>
        /// true: export excel
        /// </summary>
        public int IsExcel { get; set; } = 0;
        public string GateCode { get; set; }
        public string Keyword { get; set; }
        public string PaymentType { get; set; }
    }

    public class SaleReportFilter
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
        public string TicketCode { get; set; }
    }

    public class SoatveReportFilter: DataTableDefaultParamModel
    {
        /// <summary>
        ///Mã tuyến
        /// </summary>
        public string ZoneCode { get; set; }

        /// <summary>
        ///Mã địa điểm
        /// </summary>
        public string GateCode { get; set; }
        // <summary>
        /// trạng thái Scan
        /// </summary>
        public string StatusScan { get; set; }
        /// <summary>
        /// đến ngày
        /// </summary>
        public string ToDate { get; set; }
        /// <summary>
        /// từ ngày
        /// </summary>
        public string FromDate { get; set; }
        /// <summary>
        /// keyword
        /// </summary>
        public string Keyword { get; set; }
    }
}
