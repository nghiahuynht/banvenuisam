using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class AccountZNS : EntityCommonField
    {
        /// <summary>
        /// ID OA
        /// </summary>
        public long OAId { get; set; }
        /// <summary>
        /// ID ứng dụng
        /// </summary>
        public long AppId { get; set; }
        /// <summary>
        /// Chuỗi bảo mật ứng dụng
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// Authorization Code
        /// </summary>
        public string AuthorizationCode { get; set; }
    }
}
