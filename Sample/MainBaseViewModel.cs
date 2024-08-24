using System.Windows.Input;

namespace Sample;


public class MainBaseViewModel(BaseServices services) : ViewModel(services)
{
    public ICommand TestGEH => Commands.Create(() =>
        throw new ArgumentException("This shouldn't crash your app.  You should be seeing this message in a dialog"));
}

