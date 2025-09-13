using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommonFW.Domain.Model.Payment
{
    public class PaymentModel
    {
        public long OrderId { get; set; }
        public string OrderCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public decimal TotalVAT { get; set; }
        public string TotalVATTXT { get { return TotalVAT.ToString("#,##0"); } }
        /// <summary>
        /// tình trạng thanh toán
        /// </summary>
        public bool PaymentStatus { get; set; }
        /// <summary>
        /// tình trạng thanh toán: true: đã thanh toán, false: chưa thanh toán
        /// </summary>
        public string PaymentStatusTXT {
            get {
                return this.PaymentStatus ? "Đã Thanh toán" : "Chưa thanh toán";
            }
        }
        /// <summary>
        /// Ngày thanh toán
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        /// <summary>
        /// Ngày thanh toán txt
        /// </summary>
        public string PaymentDateTXT { 
            get
            {
                return this.PaymentDate.HasValue ? this.PaymentDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
             }
        }
        /// <summary>
        /// Ngày lên đơn
        /// </summary>
        public DateTime? ContractDate { get; set; }
        /// <summary>
        /// Ngày lên đơn txt
        /// </summary>
        public string ContractDateTXT
        {
            get
            {
                return this.ContractDate.HasValue ? this.ContractDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
            }
        }
        /// <summary>
        /// Phương thức thanh toán cuối cùng
        /// </summary>
        public string PaymentTypeFinal { get; set; }
    }

    public class PaymentInfoModel {
        public long Id { get; set; }
        /// <summary>
        /// id hợp đồng
        /// </summary>
        public long ContractId { get; set; }
        /// <summary>
        /// Mã code hợp đồng
        /// </summary>
        public string ContractCode { get; set; }
        /// <summary>
        /// Giá trị của vpc_Command đã gửi đi trong file DO được trả lại trên file DR
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// Ngôn ngữ hiển thị khi thanh toán. en-Tiếng Anh, vnTiếng Việt
        /// </summary>
        public string Locale { get; set; }
        /// <summary>
        /// Loại tiền thực hiện thanh toán, mặc định là VND
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// Giá trị của đối số vpc_MerchTxnRef
        /// </summary>
        public string MerchTxnRef { get; set; }
        /// <summary>
        /// Giá trị của đối số vpc_Merchant
        /// </summary>
        public string Merchant { get; set; }
        /// <summary>
        /// Giá trị của đối số vpc_Amount
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Mã giao dịch được sinh ra bởi cổng thanh toán để chỉ
        ///trạng thái giao dịch.
        ///Giá trị là “0” (zero) cho biết giao dịch đã được xử lý thành
        ///công.Tất cả các giá trị khác cho biết giao dịch đã bị từ
        ///chối. Tham khảo bảng Response Code
        /// </summary>
        public int TxnResponseCode { get; set; }
        /// <summary>
        /// Là một số duy nhất được sinh ra từ cổng thanh toán
        ///cho mỗi giao dịch.
        ///Thông tin này được lưu trên cổng thanh toán để ánh xạ
        ///cho phép người sử dụng thực hiện các chức năng như
        ///refund hay capture
        /// </summary>
        public int TransactionNo { get; set; }
        /// <summary>
        /// Mô tả lỗi giao dịch khi thanh toán
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 6 số định danh ngân hàng thanh toán (tương tự 6 số đầu của thẻ)
        /// </summary>
        public int AdditionData { get; set; }
        /// <summary>
        /// Trường này cho phép đơn vị kiểm tra bản tin Payment Response có hợp lệ hay không.
        /// </summary>
        public string SecureHash { get; set; }
        /// <summary>
        /// validate Hash, true: hợp lệ, không hợp lệ
        /// </summary>
        public string ValidatedHash { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NotePayment { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class ResPaymentInfoModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        public int Code { get; set; }
        public string Desc { get; set; }
    }
    public class QRCodePaymentModel
    {
        public string accountNo { get; set; }
        public string accountName { get; set; }
        public string acqId { get; set; }
        public string addInfo { get; set; }
        public string amount { get; set; }
        public string template { get; set; }
    }
    public class ResponQRCodePaymentModel
    {
        public string code { get; set; }
        public string desc { get; set; }
        public QRCodeDetailModel data { get; set; }
      
    }
    public class QRCodeDetailModel
    {
        public string qrCode { get; set; }
        public string qrDataURL { get; set; }
    }

    public class DeeplinkInfoAppBank
    {
        public string AppId { get; set; }
        public string AppLogo { get; set; }
        public string AppName { get; set; }
        public string BankName { get; set; }
        public int MonthlyInstall { get; set; }
        public string Deeplink { get; set; }
        public int Autofill { get; set; }
    }
    public class DeeplinkInfoAppBankResponse
    {
        public List<DeeplinkInfoAppBank> Apps { get; set; }
    }
}