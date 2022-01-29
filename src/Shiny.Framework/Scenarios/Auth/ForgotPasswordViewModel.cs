using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Shiny.Scenarios.Auth
{
    public abstract class ForgotPasswordViewModel : ViewModel
    {
        protected ForgotPasswordViewModel()
        {
            Send = ReactiveCommand.CreateFromTask(
               () => Process(Identifier),
                this.WhenAny(
                    x => x.Identifier,
                    id => Validate(id.GetValue())
                )
            );
        }


        protected virtual bool Validate(string identifier) => !identifier.IsEmpty();
        protected abstract Task Process(string id);
        public ICommand Send { get; }
        [Reactive] public string ErrorMessage { get; protected set; }
        [Reactive] public string Identifier { get; set; }
    }
}
