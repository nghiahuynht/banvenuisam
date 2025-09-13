using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class ResultCode : BaseEntity
    {
        /// <summary>
        /// Mã lỗi
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Mô tả lỗi tiếng anh
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Mô tả lỗi VN
        /// </summary>
        public string DescriptionVN { get; set; }
    }
}
