using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Shiny.Stores;


namespace Shiny.Msal
{
    public interface IMsalService
    {
        string? AccessToken { get; }
        string? IdToken { get; }
        DateTimeOffset? ExpiresOn { get; }

        Task<bool> SignIn();
        Task SignOut();
        Task TryRefresh();
    }


    [ObjectStoreBinder("secure")]
    public class MsalService : NotifyPropertyChanged, IMsalService
    {
        readonly IPublicClientApplication authClient;
        readonly MsalConfiguration config;
        readonly IPlatform platform;


        public MsalService(MsalConfiguration config, IPlatform platform)
        {
            this.config = config;
            this.platform = platform;

            var redirectUri = platform.GetType().FullName.Contains("Android")
                ? "msauth://{AppId}/" + config.SignatureHash
                : "msauth.{AppId}://auth";

            this.authClient = PublicClientApplicationBuilder.Create(config.ClientId)
                .WithIosKeychainSecurityGroup(config.AppId)
                .WithRedirectUri(redirectUri)
                .WithAuthority(config.Authority ?? "https://login.microsoftonline.com/common")
                .Build();
        }


        DateTimeOffset? expiresOn;
        public DateTimeOffset? ExpiresOn
        {
            get => this.expiresOn;
            set => this.Set(ref this.expiresOn, value);
        }


        string accessToken;
        public string? AccessToken
        {
            get => this.accessToken;
            set => this.Set(ref this.idToken, value);
        }


        string? idToken;
        public string? IdToken
        {
            get => this.idToken;
            set => this.Set(ref this.idToken, value);
        }


        public async Task<bool> SignIn()
        {
            var result = true;
            try
            {
                await this.TryRefresh();
            }
            catch (MsalUiRequiredException)
            {
                var authResult = await this.authClient
                    .AcquireTokenInteractive(this.config.Scopes)
                    .WithPrompt(this.config.LoginPrompt ?? Prompt.SelectAccount)
                    .WithUseEmbeddedWebView(this.config.UseEmbeddedWebView ?? true)
#if __ANDROID__
                    .WithParentActivityOrWindow(((IAndroidContext)this.platform).CurrentActivity)
#endif
                    .ExecuteAsync();

                this.OnLogin(authResult);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }


        public async Task SignOut()
        {
            var accounts = await this.authClient.GetAccountsAsync();
            foreach (var account in accounts)
                await this.authClient.RemoveAsync(account);

            this.OnLogin(null);
        }


        public async Task TryRefresh()
        {
            var accounts = await this.authClient.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault();
            var authResult = await this.authClient.AcquireTokenSilent(this.config.Scopes, firstAccount).ExecuteAsync();

            this.OnLogin(authResult);
        }


        void OnLogin(AuthenticationResult? result)
        {
            this.AccessToken = result?.AccessToken;
            this.IdToken = result?.IdToken;
            this.ExpiresOn = result?.ExpiresOn;
        }
    }
}
