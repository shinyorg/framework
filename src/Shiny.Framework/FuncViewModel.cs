using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Prism.Xaml;

namespace Shiny;


public abstract class FuncViewModel : ViewModel
{
    protected FuncViewModel(BaseServices services) : base(services) {}
    protected Func<CompositeDisposable, Task>? OnReady { get; set; }


    protected INavigationParameters? NavParams { get; private set; }
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        this.NavParams = parameters;
    }


    public override async void OnAppearing()
    {
        base.OnAppearing();
        if (OnReady != null)
        {
            await this.SafeExecuteAsync(
                () => this.OnReady.Invoke(this.DeactivateWith),
                true
            );
        }
    }
}

