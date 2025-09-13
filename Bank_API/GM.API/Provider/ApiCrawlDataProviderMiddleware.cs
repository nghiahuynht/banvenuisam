using JOS.ApiKeyAuthentication.Web.Features.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GM.API.Provider
{
    public class ApiCrawlDataProviderMiddleware : AuthenticationHandler<ApiCrawlDataProviderOptions>
    {
        private const string ProblemDetailsContentType = "application/problem+json";

        public ApiCrawlDataProviderMiddleware(
            IOptionsMonitor<ApiCrawlDataProviderOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var apiKeyCrawlData = Options.ApiKeyCrawlData;
            var apiRoleCrawlData = Options.ApiRoleCrawlData;

            if (!Request.Headers.TryGetValue(ApiKeyConstants.HeaderName, out var apiKeyHeaderValues))
            {
                return await Task.FromResult(AuthenticateResult.NoResult());
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey))
            {
                return await Task.FromResult(AuthenticateResult.NoResult());
            }

            if (apiKeyCrawlData != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Crawler")
                };

                claims.Add(new Claim(ClaimTypes.Role, apiRoleCrawlData));

                var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
                var identities = new List<ClaimsIdentity> { identity };
                var principal = new ClaimsPrincipal(identities);
                var ticket = new AuthenticationTicket(principal, Options.Scheme);

                return await Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return await Task.FromResult(AuthenticateResult.Fail("Invalid API Key provided."));
        }
    }
}