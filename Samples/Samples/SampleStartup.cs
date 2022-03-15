using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Ioc;
using Prism.Navigation;
using Shiny;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]


namespace Samples
{
    public class SampleStartup : FrameworkStartup
    {
        protected override void Configure(ILoggingBuilder builder, IServiceCollection services)
        {
            services.ConfigureLocalization(x => x
                .AddResource("Samples.Resources.Strings", this.GetType().Assembly, "Strings")
                .AddResource("Samples.Resources.Enums", this.GetType().Assembly, "Enums")
                .AddResource("Samples.Resources.Validation", this.GetType().Assembly, "Validation"),
                "Strings"
            );

            services.UseDataAnnotationValidation();

            // NOTE: IDialogs is automatically registered to NativeDialogs if you do not set a custom one like below
            services.UseXfMaterialDialogs("Strings");

            services.UseGlobalCommandExceptionHandler(options =>
            {
#if DEBUG
                options.AlertType = ErrorAlertType.FullError; // this will show the full error in a dialog during debug
                options.LogError = false;
#else
                // you will need localization to be registered to use this
                options.AlertType = ErrorAlertType.Localize; // you can use NoLocalize which will send a standard english message instead
                options.LogError = true;
                options.IgnoreTokenCancellations = true;
                options.LocalizeErrorTitleKey = "YourLocalizationTitleKey";
                options.LocalizeErrorBodyKey = "YourLocalizationErrorBodyMessageKey";
#endif
            });
        }


        public override Task RunApp(INavigationService navigator)
            => navigator.Navigate("NavigationPage/MainPage");


        public override void ConfigureApp(Application app, IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<TabbedPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<DialogsPage, DialogsViewModel>();
            containerRegistry.RegisterForNavigation<OtherPage, OtherViewModel>();
            containerRegistry.RegisterForNavigation<ValidationPage, ValidationViewModel>();
        }
    }
}
