using DAL.Models;
using DAL.Models.Promotion;
using DAL.Models.Ticket;
using DAL.Models.Promotion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface IPromotionService
    {
        //List<PromotionModel> GetAllPromotion();

        //PromotionModel GetPromotionByCode(string promotionCode);
        Task<DataTableResultModel<PromotionGridModel>> SearchPromotion(PromotionFilterModel filter);
        SaveResultModel CreatePromotion(PromotionCreateModel promotibon, string userName);
        SaveResultModel UpdatePromotion(PromotionModel promotion, string userName);
        SaveResultModel DeleteProtmotion(int Id, string userName);


        // ddphuong GetInfo Voucher
        InfoVoucherViewModel GetInfoVoucher(string VoucherCode);
    }
}
