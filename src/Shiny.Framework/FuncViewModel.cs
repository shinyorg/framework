using Prism.Navigation;
using System;
using System.Threading.Tasks;

namespace Shiny;


public abstract class FuncViewModel : ViewModel
{
    protected FuncViewModel(BaseServices services) : base(services)
    {
    }

    protected Action? Appearing { get; }
    protected Action? Disappearing { get; }
    protected Func<INavigationParameters, Task>? Init { get; }
    protected Func<Task<bool>>? CanNav { get; }
    protected Action<INavigationParameters>? NavTo { get; }
    protected Action<INavigationParameters>? NavFrom { get; }


    public override Task InitializeAsync(INavigationParameters parameters)
    {
        if (this.Init == null)
            return Task.CompletedTask;

        return this.Init.Invoke(parameters);
    }


    public override Task<bool> CanNavigateAsync(INavigationParameters parameters)
    {
        if (this.CanNav == null)
            return Task.FromResult(true);

        return this.CanNav.Invoke();
    }


    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        this.NavTo?.Invoke(parameters);
    }


    public override void OnNavigatedFrom(INavigationParameters parameters)
    {
        base.OnNavigatedFrom(parameters);
        this.NavFrom?.Invoke(parameters);
    }


    public override void OnAppearing()
    {
        base.OnAppearing();
        this.Appearing?.Invoke();
    }


    public override void OnDisappearing()
    {
        base.OnDisappearing();
        this.Disappearing?.Invoke();
    }
}