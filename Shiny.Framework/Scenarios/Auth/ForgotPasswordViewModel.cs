using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace Shiny.Scenarios.Auth
{
    public abstract class ForgotPasswordViewModel : ViewModel
    {
        protected ForgotPasswordViewModel()
        {
            this.Send = ReactiveCommand.CreateFromTask(
               () => this.Process(this.Identity),
                this.WhenAny(
                    x => x.Identity,
                    id => this.Validate(id.GetValue())
                )
            );
        }


        protected virtual bool Validate(string identifier) => !identifier.IsEmpty();
        protected abstract Task Process(string id);
        public ICommand Send { get; }
        [Reactive] public string ErrorMessage { get; protected set; }
        [Reactive] public string Identity { get; set; }
    }
}
