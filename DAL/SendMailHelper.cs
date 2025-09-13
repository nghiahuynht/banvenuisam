using DAL.Models.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using WebApp.Infrastructure;
using WebApp.Infrastructure.Configuration;

namespace DAL
{
    public class SendMailHelper
    {
        public void SendMail(EmailNofiModel model, string emailSent, string passEmailSend)
        {
            string smtpAddress = AppSettingServices.Get.EmailSettings.SMTPAddress;
            int smtpPort = AppSettingServices.Get.EmailSettings.SMTPPort;
            MailMessage message = new MailMessage();

            string msg = string.Empty;
            if (!string.IsNullOrEmpty(model.ToList))
            {
                try
                {
                    //MailAddress fromAddress = new MailAddress(model.From,"TTBT DTQG Con Dao");
                    MailAddress fromAddress = new MailAddress(model.From, "Danh Thang Ganh Da Dia");
                    message.From = fromAddress;
                    message.To.Add(model.ToList);
                    if (!string.IsNullOrEmpty(model.CCList))
                    {
                        message.CC.Add(model.CCList);
                    }
                    message.Subject = model.Subject;
                    message.IsBodyHtml = true;
                    message.Body = model.EmailContent;



                    using (SmtpClient smtpClient = new SmtpClient(smtpAddress, smtpPort))
                    {
                        smtpClient.EnableSsl = true;
                        //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        //smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new System.Net.NetworkCredential(emailSent, passEmailSend);
                        smtpClient.Send(message);
                    }

                }
                catch (Exception ex)
                {
                    WriteLog.writeToLogFile($"[SendMail][SendMailHelper] Exception - {ex}");
                }
            }


        }



        public string FormatMailWhenPostIssue(string issueCode, string issueOwerName, string softName, string issuseDescription, int status, string postDate)
        {
            string statusDescription = "";
            switch (status)
            {
                case 0:
                    statusDescription = "<span class='label label-primary'>Chờ xử lý</span>";
                    break;
                case 1:
                    statusDescription = "<span class='label label-warning'>Đã tiếp nhận</span>";
                    break;
                case 2:
                    statusDescription = "<span class='label label-info'>Xử lý thành công</span>";
                    break;
                case 3:
                    statusDescription = "<span class='label label-success'>Xử lý không thành công</span>";
                    break;
                case 4:
                    statusDescription = "<span class='label label-danger'>Xử lý không thành công</span>";
                    break;
            }
            string bodyMail = "<b>Mã yêu cầu: </b>" + issueCode + "<br/>" +
                                "<b>Phần mềm: </b>" + softName + "<br/>" +
                                "<b>Người gửi: </b>" + issueOwerName + "<br/>" +
                                "<b>Tình trạng: </b><p>" + statusDescription + "</p><br/>" +
                                "<b>Chi tiết: </b>" + issuseDescription + "<br/>" +
                                "<b>Ngày gửi: </b>" + postDate + ".<br/>" +
                                "<span>Đăng nhập vào <a href='http://hotrophanmem.com'>HoTroPhanMem.Com</a> để xem chi tiết.</span>";
            return bodyMail;
        }
        public string FormatMailWhenChangeIssueStatus(string issueCode, string issuseDescription, int oldstatus, int newStatus, string userchange, string changeDate)
        {
            string oldStatusDescription = "";
            string newStatusDescription = "";
            switch (oldstatus)
            {
                case 0:
                    oldStatusDescription = "<span class='label label-primary'>Chờ xử lý</span>";
                    break;
                case 1:
                    oldStatusDescription = "<span class='label label-warning'>Đã tiếp nhận</span>";
                    break;
                case 2:
                    oldStatusDescription = "<span class='label label-info'>Xử lý thành công</span>";
                    break;
                case 3:
                    oldStatusDescription = "<span class='label label-success'>Xử lý không thành công</span>";
                    break;
                case 4:
                    oldStatusDescription = "<span class='label label-danger'>Xử lý không thành công</span>";
                    break;
            }
            switch (newStatus)
            {
                case 0:
                    newStatusDescription = "<span class='label label-primary'>Chờ xử lý</span>";
                    break;
                case 1:
                    newStatusDescription = "<span class='label label-warning'>Đã tiếp nhận</span>";
                    break;
                case 2:
                    newStatusDescription = "<span class='label label-info'>Xử lý thành công</span>";
                    break;
                case 3:
                    newStatusDescription = "<span class='label label-success'>Xử lý không thành công</span>";
                    break;
                case 4:
                    newStatusDescription = "<span class='label label-danger'>Xử lý không thành công</span>";
                    break;
            }
            string bodyMail = "<b>Mã yêu cầu: </b>" + issueCode + "<br/>" +
                                "<b>Chi tiết: </b>" + issuseDescription + "<br/>" +
                                "<span>Vừa được chuyển tình trạng từ:<b>" + oldStatusDescription + "</b> sang <b>" + newStatusDescription + "</b></span><br/>" +
                                "<b>Người chuyển: </b>" + userchange + "<br/>" +
                                "<b>Ngày chuyển: </b>" + changeDate + "<br/>." +
                                 "<span>Đăng nhập vào <a href='http://hotrophanmem.com'>HoTroPhanMem.Com</a> để xem chi tiết.</span>";
            return bodyMail;
        }
        public string FormatMailWhenUserCommentInIssue(string issueCode, string comment, string commentName, string commentDate)
        {
            string bodyMail = "Người dùng <b>" + commentName + "</b> vừa bình luận trên yêu cầu hổ trợ: <b>" + issueCode + "</b><br/>" +
                "<b>Nội dung:</b><br/>" +
                "<span>" + comment + "</span>.<br/><br/>" +
                "<span>Đăng nhập vào <a href='http://hotrophanmem.com'>HoTroPhanMem.Com</a> để xem chi tiết.</span>";
            return bodyMail;
        }
    }
}
