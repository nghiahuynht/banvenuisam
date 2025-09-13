using DAL.Models;
using DAL.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface IPricePolicyService
    {
        SaveResultModel SavePricePolicy(TicketPricePolicyModel model, string userName);
        TicketPricePolicyModel GetPolicyPriceById(int id);
        DataTableResultModel<TicketPricePolicyModel> SearchPricePolicy(PricePolicyFilterModel filter);
        SaveResultModel DeletePricePolicy(int id, string userName);
        List<TicketPricePolicyModel> GetAllPricePloicyForSale(int userId);
    }
}
