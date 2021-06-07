using System.Windows.Input;
using ReactiveUI;
using Shiny;
using Shiny.Msal;

namespace Samples
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel(IDialogs dialogs, IPlatform platform, IMsalService? msal = null)
        {
            this.Title = "Framework";
            this.Button = ReactiveCommand.CreateFromTask(async () => await dialogs.Alert("HI"));

            this.MsalSignIn = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await msal.SignIn();
                await dialogs.Snackbar("Sign-In Result: " + result);
            });

            this.MsalSignOut = ReactiveCommand.CreateFromTask(async () =>
            {
                await msal.SignOut();
                await dialogs.Snackbar("Sign-Out success");
            });

            this.MsalRefresh = ReactiveCommand.CreateFromTask(async () =>
            {
                await msal.TryRefresh();
                await dialogs.Snackbar("Refresh success");
            });
        }


        public ICommand Button { get; }
        public ICommand MsalSignIn { get; }
        public ICommand MsalSignOut { get; }
        public ICommand MsalRefresh { get; }
    }
}
