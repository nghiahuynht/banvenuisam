using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Entities
{
    public class PaymentInfo
    {
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
        /// 
        /// </summary>
        public string NotePayment { get; set; }
        /// <summary>
        /// validate Hash, true: hợp lệ, không hợp lệ
        /// </summary>
        public string ValidatedHash { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
