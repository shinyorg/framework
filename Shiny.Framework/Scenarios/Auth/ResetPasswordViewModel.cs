using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace Shiny.Scenarios.Auth
{
    public enum ResetPasswordError
    {
        PasswordNotComplex,
        PasswordsDontMatch
    }


    public abstract class ResetPasswordViewModel : ViewModel
    {
        protected ResetPasswordViewModel()
        {
            this.Reset = ReactiveCommand.CreateFromTask(
                () => this.Process(this.ResetKey, this.NewPassword),
                this.WhenAny(
                    x => x.ResetKey,
                    x => x.NewPassword,
                    x => x.ConfirmNewPassword,
                    (key, pass, confirm) =>
                    {
                        if (key.GetValue().IsEmpty())
                            return false;

                        var pwd = pass.GetValue();
                        if (pwd.IsEmpty())
                            return false;

                        if (!this.IsPasswordComplex(pwd))
                        {
                            this.Error = ResetPasswordError.PasswordNotComplex;
                            return false;
                        }

                        var c = confirm.GetValue();
                        if (c.IsEmpty())
                            return false;

                        if (pwd != c)
                        {
                            this.Error = ResetPasswordError.PasswordsDontMatch;
                            return false;
                        }
                        return true;
                    }
                )
            );
            this.BindBusyCommand(this.Reset);
        }


        protected virtual bool IsPasswordComplex(string pass) => true;
        protected abstract Task Process(string key, string pass);

        public ICommand Reset { get; }
        [Reactive] public string ErrorMessage { get; protected set; }
        [Reactive] public ResetPasswordError Error { get; protected set; }
        [Reactive] public string ResetKey { get; set; }
        [Reactive] public string NewPassword { get; set; }
        [Reactive] public string ConfirmNewPassword { get; set; }
    }
}
