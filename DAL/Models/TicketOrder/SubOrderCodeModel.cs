using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.TicketOrder
{
    public class SubOrderCodeModel
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public int SubNum { get; set; }
        public string SubOrderCode { get; set; }
        public decimal? Price { get; set; }
        public int? VAT { get; set; }
        public decimal? TotalAfterVAT { get; set; }
        public string InvoiceNumber { get; set; }
        public string TransactionId { get; set; }
    }

    public class CancelInvoiceResponse
    {
        public string ErrorCode { get; set; }
        public string Description { get; set; }
    }
    public class CancelInvoiceErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        public string ErrorCode { get; set; }
    }
}
