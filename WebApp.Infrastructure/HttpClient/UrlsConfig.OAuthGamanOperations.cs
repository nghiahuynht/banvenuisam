using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Infrastructure.HttpClient
{
    public partial class UrlsConfig
    {
        public class OAuthGamanOperations
        {
            const string _routePrefix = "/gm/api/";
            /// <summary>
            /// URL Get Access token
            /// </summary>
            public const string GetAccessToKen = _routePrefix + "v1.0/zns/get-token";
        }
    }
}
