using GM.CORE;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace GM.API.Provider.Policies
{
    public class Policies
    {
        public const string ApiDataCrawler = nameof(ApiDataCrawler);
        public const string Admin = "Admin"; //nameof(Admin);
        public const string User = nameof(User);

        public static AuthorizationPolicy DefaultPolicy()
           => new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();

        //public static AuthorizationPolicy AdminPolicy()
        //    => new AuthorizationPolicyBuilder().RequireClaim(ApiClaimTypes.Role.ClaimType, Admin).Build();
    }
}