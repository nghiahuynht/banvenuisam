using AutoMapper;
using DAL.IService;
using DAL.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Infrastructure.Utilities;

namespace DAL
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSingleton<ICoreHttpClient, CoreHttpClient>();
            services.AddTransient<IUserInfoService, UserInfoService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IInvoiceService, InvoiceService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<ITicketOrderService, TicketOrderService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IResultCodeService, ResultCodeService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IZaloService, ZaloService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IZNSService, ZNSService>();
            services.AddTransient<ISoatVeService, SoatVeService>();
            services.AddTransient<IGatePermissionService, GatePermissionService>();
            services.AddTransient<ITokenMisaService,TokenMisaService>();
            services.AddTransient<IPartnerService, PartnerService>();
            services.AddTransient<IPromotionService, PromotionService>();
            services.AddTransient<IPricePolicyService, PricePolicyService>();
            services.AddTransient<IPayooService, PayooService>();
            services.AddTransient<IVIBService, VIBService>();
            return services;
        }
    }
}
