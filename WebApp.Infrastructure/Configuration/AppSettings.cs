using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebApp.Infrastructure.Configuration.AppSettingDetail;

namespace WebApp.Infrastructure.Configuration
{
    public class AppSettings
    {
        public DomainSettings DomainSettings { get; set; }
        public ZaloSettings ZaloSettings { get; set; }
        public EmailSettings EmailSettings { get; set; }
        public PathSettings PathSettings { get; set; }
        public GeneralSettings GeneralSettings { get; set; }
        // ddphuong:11/08/2023  add setting
        public GenerateQRCodeSettings GenerateQRCodeSettings { get; set; }
        /// <summary>
        /// Thông tin config ZNS
        /// </summary>
        public ZNSSettings ZNSSettings { get; set; }
        public SecuritySettings SecuritySettings { get; set; }
        public PayooSettings PayooSettings { get; set; }
        public ViettelSettings ViettelSettings { get; set; }
        public DeeplinkBanks DeeplinkBanks { get; set; }
        public VIBSettings VIBSettings { get; set; }

    }
    public class AppSettingDetail
    {
        public class DomainSettings
        {
            /// <summary>
            /// doamain thanh toán onepay
            /// </summary>
            public string OnepayService { get; set; }
            /// <summary>
            /// domain web
            /// </summary>
            public string WebService { get; set; }
            /// <summary>
            /// Domain Zalo
            /// </summary>
            public string ZaloService { get; set; }
            /// <summary>
            /// domain lấy token authen zalo
            /// </summary>
            public string TokenZaloUrl { get; set; }
            /// <summary>
            /// domain Generate QR Code
            /// </summary>
            public string GenerateQRCodePayment { get; set; }
            /// <summary>
            /// domain authenZNS
            /// </summary>
            public string AuthZNSService { get; set; }
            /// <summary>
            /// doamain gủi tin zns
            /// </summary>
            public string ZNSService { get; set; }
            /// <summary>
            /// Auth Gaman
            /// </summary>
            public string OAuthGamanService { get; set; }
            /// <summary>
            /// Auth VIB
            /// </summary>
            public string VIBService { get; set; }
        }

        public class ZaloSettings
        {
            /// <summary>
            /// key
            /// </summary>
            public string APIKey { get; set; }
            /// <summary>
            /// mã bảo mật
            /// </summary>
            public string APISecert { get; set; }
            public string TemplateID { get; set; }
            public string CampainID { get; set; }
            /// <summary>
            /// URL hình qrcode
            /// </summary>
            public string URLQRCode { get; set; }
            /// <summary>
            /// cta code 
            /// </summary>
            public string CTACode { get; set; }
        }

        public class EmailSettings
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string SMTPAddress { get; set; }
            public int SMTPPort { get; set; }
        }
        public class PathSettings
        {
            public string TemplateEmail { get; set; }
        }
        public class GeneralSettings
        {
            public string Domain { get; set; }
            public string RootFolder { get; set; }
            public string QRCodeFolder { get; set; }
            public string privateKeyVIBPath { get; set; }
            public string QRCodeThanhToanFolder { get; set; }
        }

        public class GenerateQRCodeSettings
        {
            /// <summary>
            /// key
            /// </summary>
            public string ApiKey { get; set; }
            /// <summary>
            /// ClientId
            /// </summary>
            public string ClientId { get; set; }
            public string AccountNo { get; set; }
            public string AccountName { get; set; }
            public string AcqId { get; set; }
            public string ServiceCode { get; set; }
            public string Template { get; set; }
        }

        public class ZNSSettings
        {
            /// <summary>
            /// ID OA
            /// </summary>
            public  long OAId { get; set; }
            /// <summary>
            /// ID ứng dụng
            /// </summary>
            public long AppId { get; set; }
            /// <summary>
            /// ID template
            /// </summary>
            public string TemplateID { get; set; }
            /// <summary>
            /// cta code 
            /// </summary>
            public string CTACode { get; set; }
            /// <summary>
            /// Tên đơn vị gửi ZNS
            /// </summary>
            public string CompanyName { get; set; }
        }

        public class SecuritySettings
        {
            public string AuthenticationKey { get; set; }
        }
        public class PayooSettings
        {
            public string CreatePaymentAddress { get; set; }
            public string CheckSumKey { get; set; }
        } 
        public class ViettelSettings
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string APICancelInvoice { get; set; }
            public string TemplateCode { get; set; }
        }

        public class DeeplinkBanks
        {
            public string Android { get; set; }
            public string IOS { get; set; }
        }
        public class VIBSettings
        {
            public string Authorization { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }

    
}
