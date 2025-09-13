using Microsoft.AspNetCore.Authentication;

namespace GM.API.Provider
{
    public class ApiCrawlDataProviderOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "API Key";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;

        public string ApiKeyCrawlData { get; set; }
        public string ApiRoleCrawlData { get; set; }
    }
}