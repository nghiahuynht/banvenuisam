using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class Ticket : EntityCommonField
    {
        /// <summary>
        /// Mã vé
        /// </summary>
        public string Code {get; set;}
        /// <summary>
        /// giá vé
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Tên chi nhánh/ địa điểm
        /// </summary>

        /// <summary>

        /// <summary>
        /// Tình trạng vé: true: xóa, false chưa xóa
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Loại vé: vé lẻ / vé đoàn
        /// </summary>
        public string LoaiIn { get; set; }
        /// <summary>
        /// số hiệu
        /// </summary>
        public string KyHieu { get; set; }
        public string MauSoBienLai { get; set; }
        public decimal? VAT { get; set; }
        public string TicketGroup { get; set; }
        public string Area { get; set; }
    }
}
