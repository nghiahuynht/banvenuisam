using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WebApp.Infrastructure.Utilities
{
    public static partial class CoreUtility
    {
        private static IHttpClientFactory _httpClientFactory;
        public static void ConfigureHttpClientFactory(IHttpClientFactory httpClient)
        {
            _httpClientFactory = httpClient;
        }
        public static IHttpClientFactory HttpClientFactory()
        {
            return _httpClientFactory;
        }
    }
}
