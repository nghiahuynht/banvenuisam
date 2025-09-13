using GM.API.Middleware;
using GM.API.Provider;
using GM.API.Validator;
using GM.BL.DI;
using GM.DAL;
using GM.DAL.Respository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace GM.API
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddProfileMapper();

           


            var appSectionKeys = Configuration.GetSection("ApplicationKeys");
            var key = Encoding.UTF8.GetBytes(appSectionKeys["TokenKey"]);
            services.AddSingleton<TokenProviderOptions>(provider => new TokenProviderOptions
            {
                Path = appSectionKeys["PathLogin"],
                Expiration = TimeSpan.FromMinutes(Convert.ToInt32(appSectionKeys["ExpiredMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            });

            services.AddBussinessLayer(Configuration);
            services.AddConfigSecurity(Configuration);


            //FluentValidationConfiguration.AddFluentValidationServices(services);
            //services.AddConfigSwagger();
            if (Configuration.GetValue<bool>("EnableSwagger"))
            {
                services.AddConfigSwagger();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();             
            }

            // khangnguyen add to disable swagger on production
            if (Configuration.GetValue<bool>("EnableSwagger") && env.IsDevelopment())
            {             
                app.UseConfigSwagger();
            }
            app.UseRouting();
            app.UseStaticFiles();
            app.UseConfigSecurity(Configuration);
            //app.UseConfigSwagger();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}