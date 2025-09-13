using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class Customer:EntityCommonField
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }   
        /// <summary>
        /// Loại KH hiện tại không có
        /// </summary>
        public string CustomerType { get; set; }
        /// <summary>
        /// Mã số thuế
        /// </summary>
        public string TaxCode { get; set; }
        public int Priority { get; set; }
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string AgencyName { get; set; }

        /// <summary>
        /// Địa chỉ xuất hóa đơn
        /// </summary>
        public string TaxAddress { get; set; }
        /// <summary>
        /// Kênh bán hàng: 1 - Online, 2 - Offline
        /// </summary>
        public int SaleChannelId { get; set; }
    }
}
