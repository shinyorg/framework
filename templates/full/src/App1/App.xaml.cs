using Prism.Ioc;
using Shiny;
using Xamarin.Forms;


namespace App1
{
    public partial class App : FrameworkApplication
    {
        protected override void Initialize()
        {
            this.InitializeComponent();
            base.Initialize();
            this.NavigationService.Navigate("StartupPage");
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<TabbedPage>();

            containerRegistry.RegisterForNavigation<StartupPage, StartupViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();

            containerRegistry.RegisterForNavigation<Auth.LoginPage, Auth.LoginViewModel>();
            containerRegistry.RegisterForNavigation<Auth.ResetPasswordPage, Auth.ResetPasswordViewModel>();
            containerRegistry.RegisterForNavigation<Auth.ForgotPasswordPage, Auth.ForgotPasswordViewModel>();
        }
    }
}
