using Microsoft.AspNetCore.Authentication;
using System;

namespace GM.API.Provider
{
    public static class AuthenticationBuilderExtensionsForDataCrawler
    {
        public static AuthenticationBuilder AddApiKeySupportForDataCrawler(this AuthenticationBuilder authenticationBuilder, Action<ApiCrawlDataProviderOptions> options)
        {
            return authenticationBuilder.AddScheme<ApiCrawlDataProviderOptions, ApiCrawlDataProviderMiddleware>(ApiCrawlDataProviderOptions.DefaultScheme, options);
        }
    }
}