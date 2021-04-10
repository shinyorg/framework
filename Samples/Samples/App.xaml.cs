using System;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Navigation;
using Shiny;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]


namespace Samples
{
    public partial class App : FrameworkApplication
    {
        protected override Task Run(INavigationService navigator)
        {
            this.InitializeComponent();
            return navigator.Navigate("NavigationPage/MainPage");
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();
        }
    }
}
