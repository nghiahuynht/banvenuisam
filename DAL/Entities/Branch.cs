using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    /// <summary>
    /// Chi nhánh / địa điểm
    /// </summary>
    public class Branch: EntityCommonField
    {
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
        public bool IsDeleted { get; set; }
    }
}
