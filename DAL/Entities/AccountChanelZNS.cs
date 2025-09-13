using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class AccountChanelZNS : EntityCommonField
    {
        /// <summary>
        /// ID Account
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// Access token dùng để gọi các Official Account API
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Mỗi access token được tạo sẽ có một refresh token đi kèm. 
        /// Refresh token cho phép bạn tạo lại access token mới khi access token hiện tại hết hiệu lực. 
        /// Refresh token chỉ có thể sử dụng 1 lần.
        /// Hiệu lực: 3 tháng
        /// </summary>
        public string RefreshToken { get; set; }
        /// <summary>
        /// Thời hạn của access token (đơn vị tính: giây)
        /// </summary>
        public DateTime? ExpiresIn { get; set; }
    }
}
