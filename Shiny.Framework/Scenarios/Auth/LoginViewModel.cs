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
                    (tenant, id, pass) =>
                    {
                        if (this.UseTenant && tenant.GetValue().IsEmpty())
                            return false;

                        if (id.GetValue().IsEmpty())
                            return false;

                        if (pass.GetValue().IsEmpty())
                            return false;

                        return true;
                    }
                )
            );
            this.BindBusyCommand(this.Login);
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
