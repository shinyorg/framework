using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Shiny.Scenarios.Auth
{
    public enum ResetPasswordError
    {
        None,
        PasswordNotComplex,
        PasswordsDontMatch
    }


    public abstract class ResetPasswordViewModel : ViewModel
    {
        protected ResetPasswordViewModel()
        {
            Reset = ReactiveCommand.CreateFromTask(
                () => Process(ResetKey, NewPassword),
                this.WhenAny(
                    x => x.ResetKey,
                    x => x.NewPassword,
                    x => x.ConfirmNewPassword,
                    (key, pass, confirm) => Validate(
                        key.GetValue(),
                        pass.GetValue(),
                        confirm.GetValue()
                    )
                )
            );
            BindBusyCommand(Reset);
        }


        protected virtual bool Validate(string key, string newPassword, string confirmNewPassword)
        {
            Error = ResetPasswordError.None;
            if (key.IsEmpty())
                return false;

            if (newPassword.IsEmpty())
                return false;

            if (!IsPasswordComplex(newPassword))
            {
                Error = ResetPasswordError.PasswordNotComplex;
                return false;
            }

            if (confirmNewPassword.IsEmpty())
                return false;

            if (!newPassword.Equals(confirmNewPassword))
            {
                Error = ResetPasswordError.PasswordsDontMatch;
                return false;
            }
            return true;
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
