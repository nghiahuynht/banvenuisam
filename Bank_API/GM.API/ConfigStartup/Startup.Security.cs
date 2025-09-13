using GM.API.Middleware;
using GM.API.Provider;
using GM.API.Provider.Policies;
using GM.MODEL.ViewModel.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GM.API
{
    public static class StartupSecurity
    {
        public static IServiceCollection AddConfigSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            var appSectionKeys = configuration.GetSection("ApplicationKeys");
            var tokenKey = appSectionKeys["TokenKey"];
            var key = Encoding.UTF8.GetBytes(appSectionKeys["TokenKey"]);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero,
                };
                opt.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new ResponseViewModel<string>("Bạn không có quyền truy câp vào nguồn này", StatusCodes.Status403Forbidden));
                        return context.Response.WriteAsync(result);
                    },
                    OnChallenge = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new ResponseViewModel<string>("Bạn không được ủy quyền", StatusCodes.Status401Unauthorized));
                        return context.Response.WriteAsync(result);
                    }
                };
            });

            services.AddAuthorization(opt =>
            {
                opt.DefaultPolicy = Policies.DefaultPolicy();
                opt.AddPolicy(Policies.ApiDataCrawler, policy =>
                {
                    policy.AuthenticationSchemes.Add(ApiCrawlDataProviderOptions.DefaultScheme);
                    policy.RequireAuthenticatedUser();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseConfigSecurity(this IApplicationBuilder app, IConfiguration configuration)
        {
            //var appSectionKeys = configuration.GetSection("ApplicationKeys");
            //var key = Encoding.UTF8.GetBytes(appSectionKeys["TokenKey"]);

            app.UseCors(opt =>
                opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );


            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<TokenProviderMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}