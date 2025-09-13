using GM.BL.Service.Payment;
using GM.BL.Service.Users;
using GM.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.AspNetCore.Antiforgery;
namespace GM.BL.DI
{
    public static class ServicesRegister
    {
        public static IServiceCollection AddBussinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var conneciontString = configuration.GetConnectionString("DefaultConnection");

            services.AddSingleton<IDalService>(sv => new DalService(conneciontString));

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPaymentService, PaymentService>();
           // services.AddSingleton<IAntiforgery>();
            return services;
        }
    }
}