using DAL.IService;
using DAL.Models;
using DAL.Models.Email;
using DAL.Models.Ticket;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Infrastructure;
using WebApp.Infrastructure.Configuration;

namespace DAL.Service
{
    public class EmailService :IEmailService
    {
        public EmailService()
        {
           
        }
        public void SendEMail(ResOrderInfoDto orderDTO)
        {
            var rs = new ResCommon<string>();
            try
            {
                string emailSend = AppSettingServices.Get.EmailSettings.UserName;
                string pass = AppSettingServices.Get.EmailSettings.Password;



                if (string.IsNullOrEmpty(orderDTO.Email))
                {
                    rs.IsSuccess = false;
                    rs.Desc = "Không có thông tin Email để gửi!";
                    return;
                }
                var emailModel = new EmailNofiModel
                {
                    From = emailSend,
                    ToList = orderDTO.Email,
                    Subject = "Thông báo mua vé thăm quan thành công",
                    EmailContent = GetBodyEmail(orderDTO)

                };

                SendMailHelper sendmail = new SendMailHelper();
                sendmail.SendMail(emailModel, emailSend, pass);
            }
            catch(Exception ex)
            {
                rs.IsSuccess = false;
                rs.Desc = "Đã xảy ra lỗi trong quá trình xử lý!";
                WriteLog.writeToLogFile($"[SendMail]Exception - {ex}");
            }
        }

        public string GetBodyEmail(ResOrderInfoDto model)
        {
            string content = string.Empty;
            string templatEmail = System.IO.File.ReadAllText(AppSettingServices.Get.PathSettings.TemplateEmail); 
            var reqEmail = new JavaScriptSerializer().Serialize(model);
            ReplaceTemplate(ref templatEmail, reqEmail);

            return templatEmail;
        }

        public void ReplaceTemplate(ref string template, object obj)
        {
            foreach (var c in JsonConvert.DeserializeObject<Dictionary<string, string>>(obj.ToString()))
                template = template.Replace("{{" + c.Key + "}}", c.Value);
        }




        









    }
}
