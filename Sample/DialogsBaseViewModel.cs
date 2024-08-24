using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Sample;


public partial class DialogsBaseViewModel(BaseServices services) : ViewModel(services)
{
    public ICommand Snackbar => Commands.CreateFromTask(async () =>
    {

//         this.Message = "Testing Snackbar";
//         var clicked = await this.Dialogs.Snackbar("This is a snackbar", 5000, "OK");
//         this.Message = clicked ? "The snackbar was tapped" : "The snackbar was not touched";
    });
    [ObservableProperty] string message;
}