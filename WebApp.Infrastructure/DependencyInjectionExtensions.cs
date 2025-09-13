using WebApp.Infrastructure.HttpClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Infrastructure.Configuration;
using Microsoft.AspNetCore.Hosting;
using WebApp.Infrastructure.Utilities;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System.Net;
using System.Net.Http;
using WebApp.Infrastructure.Utilities;
using WebApp.Infrastructure.Consts;

namespace DAL
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {
            services.AddTransient<InternalClientDelegatingHandler>();
            services.AddTransient<OAuthGamanClientDelegatingHandler>();

            services.AddHttpClient(HttpClientName.OAuthGamanService, client =>
            {
                client.BaseAddress = new Uri(Helper.Settings.DomainSettings.OAuthGamanService);
                client.Timeout = TimeSpan.FromSeconds(20);
            }).AddHttpMessageHandler<OAuthGamanClientDelegatingHandler>()
            .AddPolicyHandler((provider, request) =>
            {
                return HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .Or<TimeoutRejectedException>()
                        .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(1));
            });
            return services;
        }

        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration Configuration)
        {
            //services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
            services.AddHttpClientServices();
            return services;
        }

        public static IApplicationBuilder AddInfrastructureLayer(this IApplicationBuilder app, IHostingEnvironment env)
        {
            AppSettingServices.Services = app.ApplicationServices;
            Helper.ServiceProvider = app.ApplicationServices;
            
            return app;
        }
    }
}
