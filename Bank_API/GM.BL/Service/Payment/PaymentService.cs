using GM.DAL;
using GM.MODEL.ViewModel;
using System.Threading.Tasks;

namespace GM.BL.Service.Payment
{
    public interface IPaymentService
    {

        Task<OrderInforViewModel> GetOrderInforByCode(string orderCode);
        Task<CURDResponse> UpdatePaymentStatus(PaymentInforViewModel model, string userName);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IDalService dalService;

        public PaymentService(IDalService dalService)
        {
            this.dalService = dalService;
        }

        public async Task<OrderInforViewModel> GetOrderInforByCode(string orderCode)
        {
            using (IDBContext context = dalService.Connection())
            {
                return await context.UnitOfWork.PaymentRepository.GetOrderInforByCode(orderCode);
            }
        }

        public async Task<CURDResponse> UpdatePaymentStatus(PaymentInforViewModel model, string userName)
        {
            using (IDBContext context = dalService.Connection())
            {
                return await context.UnitOfWork.PaymentRepository.UpdatePaymentStatus(model, userName);
            }
        }
    }
}