using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Prism.Xaml;

namespace Shiny;


public abstract class FuncViewModel : ViewModel
{
    protected FuncViewModel(BaseServices services) : base(services) {}
    protected Func<INavigationParameters, CompositeDisposable, Task>? Init { get; set; }
    protected Func<INavigationParameters, CompositeDisposable, Task>? NavTo { get; set; }
    protected Func<INavigationParameters, Task<bool>>? CanNav { get; set; }
    protected Func<Task>? Appearing { get; set; }


    public override async Task<bool> CanNavigateAsync(INavigationParameters parameters)
    {
        if (this.CanNav != null)
        {
            var result = true;
            await this.SafeExecuteAsync(
                async () => result = await this.CanNav.Invoke(parameters),
                true
            );
            return result;
        }
        return await base.CanNavigateAsync(parameters);
    }


    public override async Task InitializeAsync(INavigationParameters parameters)
    {
        await base.InitializeAsync(parameters);
        if (this.Init != null)
        {
            await this.SafeExecuteAsync(
                () => this.Init.Invoke(parameters, this.DestroyWith),
                true
            );
        }
    }


    public override async void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        if (this.NavTo != null)
        {
            await this.SafeExecuteAsync(
                () => this.NavTo.Invoke(parameters, this.DeactivateWith),
                true
            );
        }
    }


    public override async void OnAppearing()
    {
        base.OnAppearing();
        if (this.Appearing != null)
        {
            await this.SafeExecuteAsync(
                () => this.Appearing.Invoke(),
                true
            );
        }
    }
}