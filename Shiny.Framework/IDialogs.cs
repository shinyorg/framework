using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Shiny
{
    public interface IDialogs
    {
        Task Alert(string message, string title = "Error");
        Task ActionSheet(string title, bool allowCancel, params (string Key, Action Action)[] actions);
        Task ActionSheet(string title, IDictionary<string, Action> actions, bool allowCancel = false);
        Task<bool> Confirm(string message, string title = "Confirm", string okText = "OK", string cancelText = "Cancel");
        Task<string?> Input(string question, string? title = null);
        Task Snackbar(string message);
        Task<T> LoadingTask<T>(Func<Task<T>> task, string message);
        Task LoadingTask(Func<Task> task, string message = "Loading");
    }
}
