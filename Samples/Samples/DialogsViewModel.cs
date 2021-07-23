using System;
using System.Windows.Input;
using ReactiveUI;
using Shiny;


namespace Samples
{
    public class DialogsViewModel : TabViewModel
    {
        public DialogsViewModel(IDialogs dialogs)
        {
            this.Button = ReactiveCommand.CreateFromTask(async () => await dialogs.Alert("HI"));
        }


        public ICommand Button { get; }

        protected override void OnActiveChanged(bool active)
        {
            base.OnActiveChanged(active);
            Console.WriteLine("Dialogs ViewModel Active: " + active);
        }
    }
}
