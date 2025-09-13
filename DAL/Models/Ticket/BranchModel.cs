using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Ticket
{
    public class BranchModel
    {
        /// <summary>
        /// ID chi nhánh
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// mã chi nhánh
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tên công ty không dấu
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Tên công ty VN
        /// </summary>
        public string NameVN { get; set; }
        /// <summary>
        /// Địa chỉ công ty
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Mã số thuế
        /// </summary>
        public string TaxCode { get; set; }
        /// <summary>
        /// tình trạng xóa: true xóa, false chưa xóa
        /// </summary>
        public string IsDeleted { get; set; }
        /// <summary>
        /// Người tạp
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public string CreatedDate { get; set; }
        /// <summary>
        /// Người cập nhật
        /// </summary>
        public string UpdatedBy { get; set; }
        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public string UpdatedDate { get; set; }
    }
}
