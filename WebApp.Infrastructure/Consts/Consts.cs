using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Infrastructure.Consts
{
    public class HttpClientName
    {

        public const string OnepayService = "OnepayService";
        public const string ConDaoService = "ConDaoService";
        public const string ZaloService = "ZaloService";
        public const string ZNSService = "ZNSService";
        public const string GenerateQRCodePayment = "GenerateQRCodePayment";
        public const string OAuthGamanService = "OAuthGamanService";
    }
    public class CoreConsts
    {
        /// <summary>
        ///     API key auth Gaman
        /// </summary>
        public const string HeaderGatewayKey = "gm-gateway-key";
    }
}
