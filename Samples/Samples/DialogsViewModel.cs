using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using Shiny;
using Shiny.Extensions.Dialogs;


namespace Samples
{
    public class DialogsViewModel : ViewModel
    {
        public DialogsViewModel(IDialogs dialogs)
        {
            this.ActionSheet = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await dialogs.ActionSheet(
                    "Test Title",
                    "Accept",
                    "Reject",
                    "Option 1",
                    "Option 2",
                    "Option 3"
                );
                await dialogs.Alert("Result: " + result);
            });
            this.Alert = ReactiveCommand.CreateFromTask(async () => await dialogs.Alert("HI"));
            this.Confirm = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await dialogs.Confirm("Testing Confirm", "Test Title", "Yes", "No");
                await dialogs.Alert("Result: " + result);
            });
            this.Input = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await dialogs.Input("This is a question", "Test Title", "Do It", "Don't Do It", "Placeholder", 10);
                await dialogs.Alert("Result: " + result);
            });
            this.Loading = ReactiveCommand.CreateFromTask(async () =>
            {
                await using (await dialogs.LoadingDialog("Doing something"))
                {
                    await Task.Delay(3000);
                }
            });
            this.LoadingSnackbar = ReactiveCommand.CreateFromTask(async () =>
            {
                await using (await dialogs.LoadingSnackbar("Doing something"))
                {
                    await Task.Delay(3000);
                }
            });
            //dialogs.OpenAppSettingsIf
            //dialogs.SnackbarToOpenAppSettings
            this.Snackbar = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await dialogs.Snackbar("Test", 5000, "OK");
                await dialogs.Alert(result ? "You clicked the snackbar" : "You didn't click the snackbar");
            });

            this.WhenAnyValue(x => x.IsActive)
                .Subscribe(active => Console.WriteLine("Dialogs ViewModel Active: " + active))
                .DisposedBy(this.DestroyWith);
        }

        public ICommand ActionSheet { get; }
        public ICommand Alert { get; }
        public ICommand Confirm { get; }
        public ICommand Input { get; }
        public ICommand Loading { get; }
        public ICommand LoadingSnackbar { get; }
        public ICommand Snackbar { get; }
    }
}
