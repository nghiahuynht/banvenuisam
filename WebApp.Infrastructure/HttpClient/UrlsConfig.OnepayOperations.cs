using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Infrastructure.HttpClient
{
    public partial class UrlsConfig
    {
        public class OnepayOperations
        {
            const string _routePrefix = "/";
            public const string GetURLPayment = _routePrefix + "onecomm-pay/vpc.op";

        }
    }
}
