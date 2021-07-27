using System;
using System.Reactive;


namespace Shiny.UserDialogs
{
    public interface IUserDialogs
    {
        IObservable<Unit> Alert(AlertOptions options);
        IObservable<string?> ActionSheet(ActionSheetOptions options);
        IObservable<bool> Confirm(ConfirmOptions options);
        IObservable<PromptResult> Prompt(PromptOptions options);
    }
}
