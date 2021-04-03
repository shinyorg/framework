using System;
using System.Threading.Tasks;
using Prism.Navigation;


namespace App1.Auth
{
    public class ForgotPasswordViewModel : Shiny.Scenarios.Auth.ForgotPasswordViewModel
    {
        readonly IAuthService auth;
        readonly INavigationService navigator;


        public ForgotPasswordViewModel(IAuthService auth, INavigationService navigator)
        {
            this.auth = auth;
            this.navigator = navigator;
        }


        protected override async Task Process(string id)
        {
        }
    }
}
