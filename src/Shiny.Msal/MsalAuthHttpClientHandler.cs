using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Msal
{
    public class MsalAuthHttpClientHandler : HttpClientHandler
    {
        readonly IMsalService msal;
        public MsalAuthHttpClientHandler(IMsalService msal) => this.msal = msal;


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await this.msal.TryRefresh();
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", this.msal.AccessToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
