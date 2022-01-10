using ReactiveUI;

using Shiny;

using System.Windows.Input;

namespace Samples
{
    public class OtherViewModel : ViewModel
    {
        public OtherViewModel()
        {
            this.GlobalCommandExceptionHandle = ReactiveCommand.Create(() => throw new System.Exception("BOOM"));
        }

        public ICommand GlobalCommandExceptionHandle { get; }
    }
}
