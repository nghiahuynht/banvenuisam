using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.ZNS
{
    public class ReqAccessTokenZNS
    {
        /// <summary>
        /// Authorization code 
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// ID của ứng dụng
        /// </summary>
        [JsonProperty("app_id")]
        public long AppId { get; set; }
        /// <summary>
        /// Thuộc tính cho biết thông tin để tạo access token.
        /// authorization_code: tạo access token từ authorization code
        /// </summary>
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
        /// <summary>
        ///  Code verifier được dùng để tạo code challenge
        /// </summary>
        [JsonProperty("code_verifier")]
        public string CodeVerifier { get; set; }
    }

    public class ReqSendZNSModel
    {
        /// <summary>
        /// Tham số cho biết thông báo sẽ được gửi ở chế độ development. Giá trị nhận vào "development"
        /// </summary>
        [JsonProperty("mode")]
        public string Mode { get; set; }
        /// <summary>
        /// SĐT nhận tin ZNS bắt đầu bằng 84
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }
        /// <summary>
        /// ID của template muốn sử dụng.
        /// </summary>
        [JsonProperty("template_id")]
        public string TemplateId { get; set; }
        /// <summary>
        /// Các thuộc tính của template mà đối tác đã đăng ký với Zalo.
        /// </summary>
        [JsonProperty("template_data")]
        public ReqTemplateDataModel Data { get; set; }
        /// <summary>
        /// Mã số đánh dấu lần gọi API của đối tác, do đối tác định nghĩa. 
        /// Đối tác có thể dùng tracking_id để đối soát mà không phụ thuộc vào message_id của Zalo cung cấp.
        /// </summary>
        [JsonProperty("tracking_id")]
        public string TrackingId { get; set; }
    }
    public class ReqTemplateDataModel
    {
        /// <summary>
        /// Tên PM gửi tin/ Cty
        /// </summary>
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }
        /// <summary>
        /// Tên Khánh hàng
        /// </summary>
        [JsonProperty("customer_name")]
        public string CustomerName { get; set; }
        /// <summary>
        /// Mã tra cứu SubOrderCode
        /// </summary>
        [JsonProperty("ticket_code")]
        public string TicketCode { get; set; }
        /// <summary>
        /// Đơn giá
        /// </summary>
        [JsonProperty("price")]
        public string Price { get; set; }
        /// <summary>
        /// Số lượng
        /// </summary>
        [JsonProperty("quantity")]
        public string Quantity { get; set; }
        /// <summary>
        /// Tổng tiền
        /// </summary>
        [JsonProperty("total")]
        public string Total { get; set; }
        /// <summary>
        /// Ngày mua vé
        /// </summary>
        [JsonProperty("order_date")]
        public string CreatedDate { get; set; }
        /// <summary>
        /// Ngày thăm quan
        /// </summary>
        [JsonProperty("visit_date")]
        public string VisitDate { get; set; }
        /// <summary>
        /// mã SubOrderId để gen QRCode
        /// </summary>
        [JsonProperty("qr")]
        public string QRCode { get; set; }
        /// <summary>
        /// ctaCode
        /// </summary>
        [JsonProperty("code")]
        public string CTACode { get; set; }
        /// <summary>
        /// Điểm thăm quan
        /// </summary>
        [JsonProperty("place_visit")]
        public string PlaceVisit { get; set; }
    }
}
