using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class TicketType :EntityCommonField
    {
        /// <summary>
        /// Mã loại vé
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// tình trạng xóa: true xóa, false chưa xóa
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
