using System;
using System.Threading.Tasks;
using Prism.Navigation;


namespace App1.Auth
{
    public class ResetPasswordViewModel : Shiny.Scenarios.Auth.ResetPasswordViewModel
    {
        readonly IAuthService auth;
        readonly INavigationService navigator;


        public ResetPasswordViewModel(IAuthService auth, INavigationService navigator)
        {
            this.auth = auth;
            this.navigator = navigator;
        }


        protected override async Task Process(string key, string pass)
        {
        }
    }
}
