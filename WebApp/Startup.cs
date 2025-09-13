using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using DAL;
using DAL.Entities;
using DAL.IService;
using DAL.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rotativa.AspNetCore;
using WebApp.Extensions;
using WebApp.Infrastructure.Configuration;
using WebApp.Infrastructure.Utilities;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddInfrastructureLayer(Configuration);
            services.AddApplicationLayer();
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddHttpServices();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddDbContext<EntityDataContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // services.AddHostedService<TokenRefreshService>();
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.AddInfrastructureLayer(env);
            CoreUtility.ConfigureHttpClientFactory(app.ApplicationServices.GetRequiredService<IHttpClientFactory>());
            ConfigureContext.ConfigureContextAccessor(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "system_route",
                   template: "system/{action=index}/{id?}/{id2?}",
                   defaults: new { controller = "condao" }
               );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=condao}/{action=index}/{id?}/{id2?}");
            });

           

            RotativaConfiguration.Setup((IHostingEnvironment)env);
        }
    }
}
