using System;
using System.Threading.Tasks;
using Prism.Navigation;


namespace App1.Auth
{
    public class LoginViewModel : Shiny.Scenarios.Auth.LoginViewModel
    {
        readonly IAuthService auth;
        readonly INavigationService navigator;


        public LoginViewModel(IAuthService auth, INavigationService navigator)
        {
            this.auth = auth;
            this.navigator = navigator;
        }


        protected override async Task Process(string id, string pass, string tenant)
        {
        }
    }
}
