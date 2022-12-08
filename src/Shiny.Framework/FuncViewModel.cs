using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Prism.Xaml;

namespace Shiny;


public class FuncViewModel : ViewModel
{
    public FuncViewModel(BaseServices services) : base(services) {}


    protected Func<INavigationParameters, CompositeDisposable, Task>? OnStart { get; set; }
    protected Action<INavigationParameters?, CompositeDisposable>? OnReady { get; set; }


    public override async Task InitializeAsync(INavigationParameters parameters)
    {
        if (this.OnStart != null)
            await this.OnStart.Invoke(parameters, this.DestroyWith);

        await base.InitializeAsync(parameters);
    }


    INavigationParameters? navToParams;
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        this.navToParams = parameters;
    }


    public override void OnAppearing()
    {
        base.OnAppearing();
        this.OnReady?.Invoke(this.navToParams, this.DeactivateWith);
    }
}

