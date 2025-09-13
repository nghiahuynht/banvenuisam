using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Infrastructure.Configuration;
using WebApp.Infrastructure.Consts;

namespace WebApp.Infrastructure.HttpClient
{
    public class OAuthGamanClientDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
           HttpRequestMessage request,
           CancellationToken cancellationToken)
        {
            request.Headers.Add(CoreConsts.HeaderGatewayKey, AppSettingServices.Get.SecuritySettings.AuthenticationKey);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
