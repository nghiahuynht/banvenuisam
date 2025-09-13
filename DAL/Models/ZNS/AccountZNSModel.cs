using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.ZNS
{
    public class AccountZNSModel
    {
        public int Id { get; set; }
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
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
