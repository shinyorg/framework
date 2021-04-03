using System;
using Prism.Navigation;
using Shiny;


namespace App1
{
    public class StartupViewModel : ViewModel
    {
        readonly IAuthService auth;
        readonly INavigationService navigator;


        public StartupViewModel(IAuthService auth, INavigationService navigator)
        {
            this.auth = auth;
            this.navigator = navigator;
        }


        public override async void OnAppearing()
        {
            base.OnAppearing();
            if (this.auth.IsAuthenticated)
            {
                // could refresh tokens and stuff here
                await this.navigator.Navigate("//NavigationPage/MainPage");
            }
            else
            {
                await this.navigator.Navigate("//NavigationPage/LoginPage");
            }
        }
    }
}
