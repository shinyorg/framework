using System.Windows.Input;
using ReactiveUI;

namespace Sample;


public class MainViewModel : ViewModel
{
    //public const string LocalizeKey = nameof(LocalizeKey);


    public MainViewModel(BaseServices services) : base(services)
    {
        // TODO: test localization
        this.TestGEH = ReactiveCommand.Create(
            () =>
            {
                throw new ArgumentException("This shouldn't crash your app.  You should be seeing this message in a dialog");
            }
            //this.WhenValid() // this is a hack to ensure prism & shiny are playing nice together
        );

        this.NavToValidate = this.Navigation.Command("ValidationPage");
        this.NavToDialogs = this.Navigation.Command("DialogsPage");
    }


    public ICommand TestGEH { get; }
    public ICommand NavToValidate { get; }
    public ICommand NavToDialogs { get; }
}

