using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class SaleChannel : BaseEntity
    {
        /// <summary>
        /// Tên kênh đăng ký online/offline
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
    }
}
