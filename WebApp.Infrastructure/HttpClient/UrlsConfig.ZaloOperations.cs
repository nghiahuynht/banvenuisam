using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Infrastructure.HttpClient
{
    public partial class UrlsConfig
    {
        public class ZaloOperations
        {
            const string _routePrefix = "/";
            /// <summary>
            /// URL token authen zalo
            /// </summary>
            public const string GetToKen = _routePrefix + "api/token";
            /// <summary>
            /// Gửi Zalo
            /// </summary>
            public const string CreateZNS = _routePrefix + "zns/api/create";

        }
    }
}
