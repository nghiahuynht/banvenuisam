using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Infrastructure.HttpClient
{
    public partial class UrlsConfig
    {
        public class PaymentOperations
        {
            const string _routePrefix = "/";
            public const string GetURLReturnPayment = _routePrefix + "/condao/finish";
            public const string GetAgainLinkPayment = _routePrefix + "/condao/TicketOrder";
            public const string GetURIQRCodePayment = _routePrefix + "v2/generate";
        }
    }
}
