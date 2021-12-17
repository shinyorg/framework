using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace Shiny.Scenarios.Auth
{
    public abstract class SignUpViewModel : ViewModel
    {
        protected IObservable<bool> WhenIdentifierValidates() => this.WhenAny(
            x => x.Identifier,
            x => x.ConfirmIdentifier,
            (identifier, confirm) => this.ValidateIdentifer(
                identifier.GetValue(),
                confirm.GetValue()
            )
        );


        protected virtual bool ValidateIdentifer(string identifier, string confirmIdentifier)
        {
            this.IsIdentifierMatching = true;

            if (identifier.IsEmpty() || !this.IsIdentifierGood(identifier))
                return false;

            if (this.UseConfirmIdentifier)
            {
                if (confirmIdentifier.IsEmpty())
                    return false;

                if (!identifier.Equals(confirmIdentifier))
                {
                    this.IsIdentifierMatching = false;
                    return false;
                }
            }
            return true;
        }


        protected IObservable<bool> WhenPasswordValidates() => this.WhenAny(
            x => x.Password,
            x => x.ConfirmPassword,
            (pass, confirm) => this.ValidatePassword(
                pass.GetValue(),
                confirm.GetValue()
            )
        );


        protected virtual bool ValidatePassword(string newPassword, string confirmNewPassword)
        {
            this.IsPasswordMatching = true;

            if (newPassword.IsEmpty() || !this.IsPasswordComplex(newPassword))
                return false;

            if (confirmNewPassword.IsEmpty())
                return false;

            if (!newPassword.Equals(confirmNewPassword))
            {
                this.IsPasswordMatching = false;
                return false;
            }

            return true;
        }


        protected bool UseConfirmIdentifier { get; set; } = false;
        protected virtual bool IsPasswordComplex(string password) => true;
        protected virtual bool IsIdentifierGood(string identifier) => true;

        [Reactive] public bool IsIdentifierMatching { get; private set; } = true;
        [Reactive] public string Identifier { get; set; }
        [Reactive] public string ConfirmIdentifier { get; set; }

        [Reactive] public bool IsPasswordMatching { get; private set; } = true;
        [Reactive] public string Password { get; set; }
        [Reactive] public string ConfirmPassword { get; set; }

        [Reactive] public string FirstName { get; set; }
        [Reactive] public string LastName { get; set; }
        [Reactive] public string Country { get; set; }
        [Reactive] public string StateProvince { get; set; }
        [Reactive] public string City { get; set; }
        [Reactive] public string Address1 { get; set; }
        [Reactive] public string Address2 { get; set; }
        [Reactive] public string PostalCode { get; set; }
        [Reactive] public string Phone { get; set; }
        [Reactive] public DateTime DateOfBirth { get; set; }
    }
}
