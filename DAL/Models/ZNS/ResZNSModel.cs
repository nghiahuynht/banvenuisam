using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.ZNS
{
    public class ResAccessTokenZNS
    {
        /// <summary>
        /// Access token dùng để gọi các Official Account API
        /// Hiệu lực: 25 giờ
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        /// <summary>
        /// Mỗi access token được tạo sẽ có một refresh token đi kèm. 
        /// Refresh token cho phép bạn tạo lại access token mới khi access token hiện tại hết hiệu lực. 
        /// Refresh token chỉ có thể sử dụng 1 lần.
        /// Hiệu lực: 3 tháng
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        /// <summary>
        /// Thời hạn của access token (đơn vị tính: giây)
        /// </summary>
        [JsonProperty("expires_in")]
        public string ExpireIn { get; set; }
    }
    public class ResSendZNSModel 
    {
        /// <summary>
        /// Mã code 0 thành c
        /// </summary>
        [JsonProperty("error")]
        public int Error { get; set; }
        /// <summary>
        /// Thông báo
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("data")]
        public ResDataZNSModel Data { get; set; }
    }
    public class ResDataZNSModel
    {
        /// <summary>
        /// Thời gian gửi thông báo ZNS (định dạng timestamp).
        /// </summary>
        [JsonProperty("sent_time")]
        public string SentTime { get; set; }
        /// <summary>
        /// Bao gồm 1 trong 2 giá trị:
        /// 1: tin ZNS được gửi theo cơ chế thông thường.
        /// 2: tin ZNS được gửi theo cơ chế dựa trên tương tác người dùng (user interaction-base)
        /// </summary>
        [JsonProperty("sending_mode")]
        public string SendingMode { get; set; }
        /// <summary>
        /// Thông tin quota thông báo ZNS của OA.
        /// </summary>
        [JsonProperty("quota")]
        public QuotaZNSModel Quota { get; set; }
        /// <summary>
        /// ID của thông báo ZNS.
        /// </summary>
        [JsonProperty("msg_id")]
        public string MessageId { get; set; }

    }
    public class QuotaZNSModel
    {
        /// <summary>
        /// Số thông báo ZNS OA được gửi trong 1 ngày.
        /// </summary>
        [JsonProperty("dailyQuota")]
        public string DailyQuota { get; set; }
        /// <summary>
        /// Số thông báo ZNS OA được gửi trong ngày còn lại.
        /// </summary>
        [JsonProperty("remainingQuota")]
        public string RemainingQuota { get; set; }
    }
    public class AccessTokenZNSDto
    {
        /// <summary>
        /// token authen
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// thời gian hết hạn tính theo giây
        /// </summary>
        public long ExpireIn { get; set; }
        /// <summary>
        /// Ngày hết hạn GMT + 7:00
        /// </summary>
        public string ExpireDate { get; set; }
    }
}
