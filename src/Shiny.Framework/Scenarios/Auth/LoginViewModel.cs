using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace Shiny.Scenarios.Auth
{
    public abstract class LoginViewModel : ViewModel
    {
        protected LoginViewModel()
        {
            this.Login = ReactiveCommand.CreateFromTask(
                () => this.Process(this.Identifier, this.Password, this.Tenant),
                this.WhenAny(
                    x => x.Tenant,
                    x => x.Identifier,
                    x => x.Password,
                    (tenant, id, pass) => this.Validate(
                        id.GetValue(),
                        pass.GetValue(),
                        tenant.GetValue()
                    )
                )
            );
            this.BindBusyCommand(this.Login);
        }


        protected virtual bool Validate(string identifier, string password, string tenant)
        {
            if (this.UseTenant && tenant.IsEmpty())
                return false;

            if (identifier.IsEmpty())
                return false;

            if (password.IsEmpty())
                return false;

            return true;
        }


        protected abstract Task Process(string id, string pass, string tenant);
        public ICommand Login { get; }
        protected bool UseTenant { get; }
        [Reactive] public string ErrorMessage { get; protected set; }
        [Reactive] public string Tenant { get; set; }
        [Reactive] public string Identifier { get; set; }
        [Reactive] public string Password { get; set; }
        [Reactive] public bool RememberMe { get; set; }
    }
}
