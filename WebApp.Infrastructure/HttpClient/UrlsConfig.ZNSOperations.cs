using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Infrastructure.HttpClient
{
    public partial class UrlsConfig
    {
        public class ZNSOperations
        {
            const string _routePrefix = "/";
            /// <summary>
            /// URL Get Access token
            /// </summary>
            public const string GetAccessToKen = _routePrefix + "v4/oa/access_token";
            /// <summary>
            /// Gửi tin zns
            /// </summary>
            public const string SendZNS = _routePrefix + "message/template";
        }
    }
}
