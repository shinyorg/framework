using System;
using System.Windows.Input;
using ReactiveUI;
using Shiny;


namespace Samples
{
    public class DialogsViewModel : ViewModel
    {
        public DialogsViewModel(IDialogs dialogs)
        {
            this.Button = ReactiveCommand.CreateFromTask(async () => await dialogs.Alert("HI"));
            this.WhenAnyValue(x => x.IsActive)
                .Subscribe(active => Console.WriteLine("Dialogs ViewModel Active: " + active))
                .DisposedBy(this.DestroyWith);
        }


        public ICommand Button { get; }
    }
}
