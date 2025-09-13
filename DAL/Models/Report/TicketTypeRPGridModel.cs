using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace DAL.Models.Report
{
    
    public class TicketTypeRPGridModel
    {
        public Guid Id { get; set; }
        /// <summary>
        /// mã vé
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Loại vé
        /// </summary>
        public string LoaiVe { get; set; }
        /// <summary>
        /// đơn giá
        /// </summary>
        public string Price { get; set; } = "";
        /// <summary>
        /// SL bán
        /// </summary>
        public int? SLSale { get; set; } = 0;
        /// <summary>
        /// Doanh số bán
        /// </summary>
        public string AmountSale { get; set; } = "0";
       
    }
    public class TicketTypeRPGridModel1
    {

        public string Code { get; set; }
        /// <summary>
        /// Loại vé
        /// </summary>
        public string LoaiVe { get; set; }
        /// <summary>
        /// đơn giá
        /// </summary>
        public string Price { get; set; } = "";
        /// <summary>
        /// SL bán
        /// </summary>
        public int? SLSale { get; set; } = 0;
        /// <summary>
        /// Doanh số bán
        /// </summary>
        public string AmountSale { get; set; } = "0";

    }
}
