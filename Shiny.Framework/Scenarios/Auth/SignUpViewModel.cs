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
            (identifier, confirm) =>
            {
                this.IsIdentifierMatching = true;
                var id = identifier.GetValue();

                if (id.IsEmpty())
                    return false;

                if (!this.IsIdentifierGood(id))
                    return false;

                if (this.UseConfirmIdentifier)
                {
                    var con = confirm.GetValue();
                    if (con.IsEmpty())
                        return false;

                    if (!id.Equals(con))
                    {
                        this.IsIdentifierMatching = false;
                        return false;
                    }
                }
                return true;
            }
        );


        protected IObservable<bool> WhenPasswordValidates() => this.WhenAny(
            x => x.Password,
            x => x.ConfirmPassword,
            (pass, confirm) =>
            {
                this.IsPasswordMatching = true;
                var p = pass.GetValue();
                var c = pass.GetValue();

                if (p.IsEmpty())
                    return false;

                if (!this.IsPasswordComplex(p))
                   return false;

                if (c.IsEmpty())
                    return false;

                if (!c.Equals(p))
                {
                    this.IsPasswordMatching = false;
                    return false;
                }
                 
                return true;
             }
        );


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
