using System;

namespace GM.MODEL.ViewModel
{
    public class OrderInforViewModel
    {
        public string OrderCode { get; set; }
        public decimal? TotalAmount { get; set; }
        public string PaymentName { get; set; }
        public string Note { get; set; }
    }

    public class PaymentInforViewModel
    {
        public string orderCode { get; set; }
        public decimal paymentAmount { get; set; }
        public DateTime paymentDateTime { get; set; }
    }

    public class CURDResponse
    {
        public bool StatusResult { get; set; }
        public string CodeResult { get; set; }
        public string MessageResult { get; set; }
    }

}