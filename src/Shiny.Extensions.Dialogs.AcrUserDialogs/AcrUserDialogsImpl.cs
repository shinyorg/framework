using Acr.UserDialogs;
using Shiny.Extensions.Localization;
using System;
using System.Threading.Tasks;


namespace Shiny.Extensions.Dialogs.AcrUserDialogs
{
    public class AcrUserDialogsImpl : IDialogs
    {
        readonly IUserDialogs userDialogs;
        readonly ILocalizationSource? localize;


        public AcrUserDialogsImpl(IUserDialogs userDialogs, ILocalizationSource? localize)
        {
            this.userDialogs = userDialogs;
            this.localize = localize;
        }


        public Task<string?> ActionSheet(string title, string? acceptText = null, string? dismissText = null, params string[] options)
        {
            throw new NotImplementedException();
        }

        public Task Alert(string message, string? title = null, string? dismissText = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Confirm(string message, string? title = null, string? acceptText = null, string? dismissText = null)
        {
            throw new NotImplementedException();
        }

        public Task<string?> Input(string question, string? title = null, string? acceptText = null, string? dismissText = null, string? placeholder = null, int? maxLength = null)
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncDisposable> LoadingDialog(string message)
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncDisposable> LoadingSnackbar(string message)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Snackbar(string message, int durationMillis = 3000, string? actionText = null)
        {
            throw new NotImplementedException();
        }
    }
}