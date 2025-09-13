using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Infrastructure.Configuration;

namespace WebApp.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddHttpServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptionsMonitor<AppSettings>>().CurrentValue);

            return services;
        }

    }
}
