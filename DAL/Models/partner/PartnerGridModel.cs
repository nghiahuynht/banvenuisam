using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.partner
{
   public class PartnerGridModel
    {
        public int Id { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string PartnerPhone { get; set; }
        public string PartnerAddress { get; set; }
        public int? Status { get; set; }
        public string Note { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }

    public class InfoPartnerViewModel
    {

        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string PartnerPhone { get; set; }
        public string PartnerAddress { get; set; }
        public string Base64QRCode { get; set; }
        
    }

    public class PartnerViewModel
    {
        public string PartnerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }

        public IFormFile IdFront { get; set; }  // Dùng IFormFile để nhận file upload
        public IFormFile IdBack { get; set; }   // Dùng IFormFile để nhận file upload
    }

    public class PartnerModel
    {
        public int Id { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }

        public string FrontUrl { get; set; } // Đường dẫn ảnh mặt trước
        public string BackUrl { get; set; }  // Đường dẫn ảnh mặt sau
    }

    public class PartnerModelViewModel
    {
        public int Id { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }

        public string FrontUrl { get; set; } // Đường dẫn ảnh mặt trước
        public string BackUrl { get; set; }  // Đường dẫn ảnh mặt sau
        public DateTime CreateDate { get; set; }  
        public DateTime? ApprovalDate { get; set; } 
        public string ApprovalBy { get; set; } 
        public int Status { get; set; } 
        public string Note { get; set; } 
    }

}
