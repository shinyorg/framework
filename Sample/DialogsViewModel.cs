using System;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;

namespace Sample;


public class DialogsViewModel : ViewModel
{
    public DialogsViewModel(BaseServices services) : base(services)
    {
        this.Snackbar = ReactiveCommand.CreateFromTask(async () =>
        {
            this.Message = "Testing Snackbar";
            var clicked = await this.Dialogs.Snackbar("This is a snackbar", 5000, "OK");
            this.Message = clicked ? "The snackbar was tapped" : "The snackbar was not touched";
        });
    }


    public ICommand Snackbar { get; }
    [Reactive] public string Message { get; private set; }
}

