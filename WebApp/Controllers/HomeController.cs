using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using WebApp.Infrastructure.Configuration;

namespace WebApp.Controllers
{
    public class HomeController : AppBaseController
    {
        private IReportService reportService;
        public HomeController(IUserInfoService userInfoService, IReportService reportService)
        {
            this.reportService = reportService;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
               return RedirectToAction("Login","Account");
            }
            else
            {
                //string qrCodeFolder = Path.GetFullPath(AppSettingServices.Get.GeneralSettings.QRCodeFolder);//config["General:RootFolder"];

                //using (QRCodeGenerator QrGenerator = new QRCodeGenerator())
                //{
                //    QRCodeData QrCodeInfo = QrGenerator.CreateQrCode("https://ganhdadia.gamanjsc.com", QRCodeGenerator.ECCLevel.Q);
                //    QRCode QrCode = new QRCode(QrCodeInfo);
                //    using (Bitmap bitMap = QrCode.GetGraphic(20))
                //    {
                //        string fileFullPath = string.Format(qrCodeFolder, "LinkMua");
                //        if (!System.IO.File.Exists(fileFullPath))
                //        {
                //            bitMap.Save(fileFullPath, ImageFormat.Jpeg);
                //        }

                //        //bitMap.Dispose();
                //    }




                //}



                return View();
            }
           
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Privacy()
        {
            var test = User.Identity.Name;
            return View();
        }
        [Authorize(Roles = "Sale")]
        public IActionResult Contact()
        {

            return View();
        }

      



        #region Dashbroad
        public PartialViewResult _PartialStaffSaleCounter(string dateView)
        {
            var res = reportService.ReportStaffSaleCounter(dateView);
            return PartialView(res);
        }

        public PartialViewResult _PartialTicketSaleMisaStatus(string fromDate, string toDate)
        {
            var res = reportService.ReportTicketMisaStatus(fromDate, toDate);
            return PartialView(res);
        }
        #endregion
    }
}
