using System.Windows.Input;
using ReactiveUI;
using Shiny;


namespace Samples
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel(IDialogs dialogs)
        {
            this.Title = "Framework";
            this.Button = ReactiveCommand.CreateFromTask(async () => await dialogs.Alert("HI"));
        }

        public ICommand Button { get; }
    }
}
