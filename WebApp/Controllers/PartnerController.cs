using DAL;
using DAL.IService;
using DAL.Models;
using DAL.Models.partner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class PartnerController : AppBaseController
    {
        private readonly IPartnerService _partnerService;
        private IUserInfoService _userService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PartnerController(IPartnerService partnerService, IUserInfoService userService, IHostingEnvironment hostingEnvironment)
        {
            _partnerService = partnerService;
            _userService = userService;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Search partner
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public DataTableResultModel<PartnerGridModel> SearchPartner(PartnerFilterModel filter)
        {
            var res = _partnerService.SearchTicket(filter);
            return res;
        }

        /// <summary>
        /// Get info partner  InfoPartnerViewModel
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateAntiForgeryToken]
        public JsonResult GetInfoPartner( int Id)
        {
            var resJson = new ResCommon<InfoPartnerViewModel>();
            var res = _partnerService.GetInfoPartner(Id);
            using (QRCodeGenerator QrGenerator = new QRCodeGenerator())
            {
                string domain = $"{Request.Scheme}://{Request.Host}";
                string urlPartner = string.Format(@"{0}?partner={1}",domain,res.PartnerCode);
                QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(urlPartner, QRCodeGenerator.ECCLevel.Q);
                QRCode QrCode = new QRCode(QrCodeInfo);

                using (Bitmap bitMap = QrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // Lưu bitmap vào MemoryStream dưới định dạng PNG
                        bitMap.Save(ms, ImageFormat.Png);

                        // Chuyển đổi nội dung MemoryStream thành chuỗi base64
                        string base64String = Convert.ToBase64String(ms.ToArray());

                        // Gán chuỗi base64 vào thuộc tính Base64QRCode
                        res.Base64QRCode = base64String;
                    }
                }
            }

            resJson.Data = res;
            return Json(resJson);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public IActionResult PartnerDatail(int? id)
        {
            PartnerModelViewModel partnerDetailVM = new PartnerModelViewModel();
            try
            {
                if (id.HasValue)
                {
                    partnerDetailVM = _partnerService.GetPartnerById((int)id);

                }
            }
            catch (Exception ex)
            {
                partnerDetailVM = new PartnerModelViewModel();
            }
           




            return View(partnerDetailVM);
        }

        /// <summary>
        /// Get info partner  InfoPartnerViewModel
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
      
        public JsonResult ApprovalPartner( int Id)
        {
            var resJson = new ResCommon<int>();
            var infoPartner = _partnerService.GetInfoPartner(Id);
            // B1 Tạo user
            DAL.Entities.UserInfo modelUser = new DAL.Entities.UserInfo()
            {
                Email = "",
                FullName = infoPartner.PartnerName,
                Phone = infoPartner.PartnerPhone,
                UserName = infoPartner.PartnerPhone,
                Pass = "GM123",
                RoleCode = "Partner",
                IsPartner = true,
                PartnerCode=infoPartner.PartnerCode,
                IsActive=true
            };
           var  resCreateUser = _userService.CreateNewUser(modelUser, AuthenInfo().UserName);
            if(resCreateUser.IsSuccess)
            {
                // Cập nhập trạng thái parner
                var resApproval = _partnerService.ApprovalPartner(Id, AuthenInfo().UserName);
                if(resApproval == true)
                {
                    resJson.IsSuccess = true;
                    resJson.Desc = "Thành công";
                }    
                else
                {
                    resJson.IsSuccess = false;
                    resJson.Desc = "Lỗi duyệt đối tác";
                }    
            }   
            else
            {
                resJson.IsSuccess = false;
                resJson.Desc = "Thêm user thất bại";
            }    
            return Json(resJson);
        }

        /// <summary>
        /// RegisterPartner
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegisterPartner([FromForm] PartnerViewModel model)
        {
            ResultModel rs = new ResultModel();
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostingEnvironment.WebRootPath; // Đường dẫn tới thư mục wwwroot
                string uploadsFolder = Path.Combine(wwwRootPath, "uploadsPartner"); // Thư mục lưu ảnh

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder); // Tạo thư mục nếu chưa có
                }

                // Xử lý ảnh mặt trước
                string frontFileName = null;
                if (model.IdFront != null)
                {
                    string fileExtension = Path.GetExtension(model.IdFront.FileName);
                    frontFileName = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("HHmmss")+ fileExtension;
                    string frontFilePath = Path.Combine(uploadsFolder, frontFileName);

                    using (var stream = new FileStream(frontFilePath, FileMode.Create))
                    {
                         model.IdFront.CopyTo(stream);
                    }
                }

                // Xử lý ảnh mặt sau
                string backFileName = null;
                if (model.IdBack != null)
                {
                    string fileExtension = Path.GetExtension(model.IdBack.FileName);
                    backFileName = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("HHmmss") + fileExtension;
                    string backFilePath = Path.Combine(uploadsFolder, backFileName);

                    using (var stream = new FileStream(backFilePath, FileMode.Create))
                    {
                         model.IdBack.CopyTo(stream);
                    }
                }

                // Lưu thông tin đối tác vào cơ sở dữ liệu
                PartnerModel partner = new PartnerModel
                {
                    PartnerCode= Helper.GenPartnerCode(model.PartnerName).ToLower(),
                    PartnerName = model.PartnerName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    BankAccount = model.BankAccount,
                    BankName = model.BankName,
                    FrontUrl = "/uploadsPartner/" + frontFileName, // Đường dẫn tương đối đến ảnh mặt trước
                    BackUrl = "/uploadsPartner/" + backFileName    // Đường dẫn tương đối đến ảnh mặt sau
                };

                 rs = _partnerService.InsertUpdatePartner(partner);
                //await _context.SaveChangesAsync();
                return Json(rs);

            }
            rs.IsSuccess = false;
            rs.ErrorMessage = "Dữ liệu không hợp lệ";
            return Json(rs);
        }

        /// <summary>
        /// CancelApproval
        /// </summary>
        /// <param name="CancelApproval"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CancelApproval(int id,string note)
        {
            ResultModel rs = new ResultModel();
            var resCancelApproval = _partnerService.CancelApproval(id,note, AuthenInfo().UserName);
            if(resCancelApproval ==true)
            {
                rs.IsSuccess = true;
                rs.ErrorMessage = "Cập nhập thành công";
            }    
            else
            {
                rs.IsSuccess = false;
                rs.ErrorMessage = "Cập nhập thất bại";
            }    
            return Json(rs);
        }

        /// <summary>
        /// Get info partner  InfoPartnerViewModel
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateAntiForgeryToken]
        public JsonResult DeletePartner(int Id)
        {
            var resJson = new ResCommon<int>();
            // Cập nhập trạng thái parner
            var resApproval = _partnerService.DeletePartner(Id, AuthenInfo().UserName);
            if (resApproval == true)
            {
                resJson.IsSuccess = true;
                resJson.Desc = "Thành công";
            }
            else
            {
                resJson.IsSuccess = false;
                resJson.Desc = "Lỗi xóa đối tác";
            }
            return Json(resJson);
        }

    }
}
