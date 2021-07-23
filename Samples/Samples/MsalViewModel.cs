using System;
using System.Windows.Input;

using ReactiveUI;

using Shiny;
using Shiny.Msal;


namespace Samples
{
    public class MsalViewModel : TabViewModel
    {
        public MsalViewModel(IDialogs dialogs,  IMsalService? msal = null)
        {
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


        public ICommand MsalSignIn { get; }
        public ICommand MsalSignOut { get; }
        public ICommand MsalRefresh { get; }


        protected override void OnActiveChanged(bool active)
        {
            Console.WriteLine("MSAL ViewModel Active: " + active);
        }
    }
}
