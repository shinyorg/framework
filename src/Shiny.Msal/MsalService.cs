using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
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
        JwtSecurityToken ReadJwtFromIdToken();
    }


    [ObjectStoreBinder("secure")]
    public class MsalService : NotifyPropertyChanged,
                               IMsalService,
                               IShinyStartupTask
    {
        readonly MsalConfiguration config;
        readonly IPlatform platform;
        readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        IPublicClientApplication authClient;


        public MsalService(MsalConfiguration config, IPlatform platform)
        {
            this.config = config;
            this.platform = platform;
        }


        public void Start()
        {
            var builder = PublicClientApplicationBuilder
                .Create(this.config.ClientId)
                .WithIosKeychainSecurityGroup(this.config.IosKeychainSecurityGroup ?? "com.microsoft.adalcache")
                .WithAuthority(
                    this.config.Authority ??
                    $"https://login.microsoftonline.com/{this.config.TenantId}" ??
                    "https://login.microsoftonline.com/common"
                );

            if (this.config.UseBroker)
            {
                if (this.platform.IsAndroid() && this.config.SignatureHash.IsEmpty())
                    throw new ArgumentException("UseBroker requires SignatureHash be set on Android");

                var redirectUri = this.platform.IsAndroid()
                    ? $"msauth://{this.platform.AppIdentifier}/{this.config.SignatureHash}"
                    : $"msauth.{this.platform.AppIdentifier}://auth";

                builder = builder
                    .WithBroker()
                    .WithRedirectUri(redirectUri);
            }
            this.authClient = builder.Build();
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
#elif XAMARIN_IOS
                    .WithParentActivityOrWindow(this.GetTopViewController())
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
            if (this.ExpiresOn > DateTimeOffset.UtcNow)
                return;

            try
            {
                // ensure httpclient isn't called for multiple refresh
                await this.semaphore.WaitAsync();
                if (this.ExpiresOn > DateTimeOffset.UtcNow)
                    return;

                var accounts = await this.authClient.GetAccountsAsync();
                var firstAccount = accounts.FirstOrDefault();
                var authResult = await this.authClient
                    .AcquireTokenSilent(this.config.Scopes, firstAccount)
                    .ExecuteAsync();

                this.OnLogin(authResult);
            }
            finally
            {
                this.semaphore.Release();
            }
        }


        public JwtSecurityToken ReadJwtFromIdToken()
        {
            if (this.IdToken.IsEmpty())
                throw new ArgumentException("IdToken is not set");

            var handler = new JwtSecurityTokenHandler();
            var sec = handler.ReadJwtToken(this.IdToken);
            return sec;
        }


        void OnLogin(AuthenticationResult? result)
        {
            this.AccessToken = result?.AccessToken;
            this.IdToken = result?.IdToken;
            this.ExpiresOn = result?.ExpiresOn;
        }


#if XAMARIN_IOS
    protected virtual UIKit.UIWindow GetTopWindow() => UIKit
        .UIApplication
        .SharedApplication
        .Windows
        .Reverse()
        .FirstOrDefault(x =>
            x.WindowLevel == UIKit.UIWindowLevel.Normal &&
            !x.Hidden
        );


    protected virtual UIKit.UIViewController GetTopViewController()
    {
        var viewController = UIKit.UIApplication.SharedApplication.KeyWindow.RootViewController;
        while (viewController.PresentedViewController != null)
            viewController = viewController.PresentedViewController;

        return viewController;
    }

#endif
    }
}
