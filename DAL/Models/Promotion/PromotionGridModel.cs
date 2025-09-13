using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models.Promotion
{
    public class PromotionGridModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public bool? IsPercent { get; set; }
        public int? PercentValue { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

}
