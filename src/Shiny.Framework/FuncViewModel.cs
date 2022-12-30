using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Prism.Xaml;

namespace Shiny;


public class FuncViewModel : ViewModel
{
    public FuncViewModel(BaseServices services) : base(services) {}

    protected Func<INavigationParameters?, CompositeDisposable, Task>? OnReady { get; set; }


    INavigationParameters? navToParams;
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        this.navToParams = parameters;
    }


    public override async void OnAppearing()
    {
        base.OnAppearing();
        if (OnReady != null)
        {
            await this.SafeExecuteAsync(
                () => this.OnReady.Invoke(this.navToParams, this.DeactivateWith),
                true
            );
            this.navToParams = null;
        }
    }
}

