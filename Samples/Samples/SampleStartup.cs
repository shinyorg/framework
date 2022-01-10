using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Ioc;
using Prism.Navigation;
using Shiny;
using Shiny.Extensions.Localization;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]


namespace Samples
{
    public class SampleStartup : FrameworkStartup
    {
        protected override void Configure(ILoggingBuilder builder, IServiceCollection services)
        {
            var manager = new LocalizationBuilder()
                .AddResource("Samples.Resources.Strings", this.GetType().Assembly, "Strings")
                .AddResource("Samples.Resources.Enums", this.GetType().Assembly, "Enums")
                .Build();

            services.AddSingleton(manager);
            services.UseXfMaterialDialogs();
            services.UseGlobalCommandExceptionHandler();
        }


        public override Task RunApp(INavigationService navigator)
            => navigator.Navigate("NavigationPage/MainPage");


        public override void ConfigureApp(Application app, IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<TabbedPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<DialogsPage, DialogsViewModel>();
        }
    }
}
