using DAL.IService;
using DAL.Models.Ticket;
using DAL.Models;
using DAL.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DAL.Models.Promotion;
using DAL.Models.ConDao;
using DAL.Models;
using DAL.Models.Promotion;
using Microsoft.AspNetCore.Authorization;
using System;

namespace WebApp.Controllers
{
    public class PromotionController : AppBaseController
    {

        private IPromotionService promotionService;



        public PromotionController(IPromotionService promotionService)
        {
            this.promotionService = promotionService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
  
        public async Task<DataTableResultModel<PromotionGridModel>> SearchPromotion(PromotionFilterModel filter)
        {
            var res = await promotionService.SearchPromotion(filter);
            return res;
        }
        [HttpGet]
        public async Task<JsonResult> DeletePromotion(int id)
        {
            var res =  promotionService.DeleteProtmotion(id, User.Identity.Name);
            return Json(res);
        }

        /// <summary>
        /// RegisterPartner
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreatePromotion([FromBody] PromotionCreateModel model)
        {
            SaveResultModel rs = new SaveResultModel();
           try
            {
                 rs = promotionService.CreatePromotion(model, AuthenInfo().UserName);

                return Json(rs);
            }
            catch(Exception ex)
            {
                rs.IsSuccess = false;
                rs.ErrorMessage = ex.Message;
                return Json(rs);
            }
        }

        #region ddphuong
        /// <summary>
        /// Get info voucher
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateAntiForgeryToken]
        public JsonResult GetInfoVoucher(string VoucherCode)
        {
            var resJson = new ResCommon<InfoVoucherViewModel>();
            var res = promotionService.GetInfoVoucher(VoucherCode);
            resJson.Data = res;
            return Json(resJson);
        }
        #endregion
    }
}
