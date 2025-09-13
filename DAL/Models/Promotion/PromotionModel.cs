using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace DAL.Models.Promotion
{
    public class PromotionModel:BaseEntity
    {
        public int Id { get; set; }
        public string PromotionCode { get; set; }
        public decimal PromotionValue { get; set; }
        public DateTime? EffectFrom { get; set; }
        public DateTime? EffectTo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool isDelete { get; set; }
        public decimal? PercentValue { get; set; }

    }

    public class PromotionCreateModel
    {
        public int Id { get; set; }
        public string PromotionCode { get; set; }
        public decimal? PromotionValue { get; set; }
        public int? PercentValue { get; set; }
        public bool IsPercent { get; set; }
    }
}
