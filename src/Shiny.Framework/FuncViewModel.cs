using Prism.Navigation;
using System;
using System.Threading.Tasks;

namespace Shiny;


public abstract class FuncViewModel : ViewModel
{
    protected FuncViewModel(BaseServices services) : base(services)
    {
    }

    protected Action? Appearing { get; set; }
    protected Func<Task>? AppearingTask { get; set; }

    protected Action? BeforeDestroy { get; set; }
    protected Action? Disappearing { get; set; }
    protected Func<INavigationParameters, Task>? Init { get; set; }
    protected Func<Task<bool>>? CanNav { get; set; }
    protected Action<INavigationParameters>? NavTo { get; set; }
    protected Func<INavigationParameters, Task>? NavToTask { get; set; }
    protected Action<INavigationParameters>? NavFrom { get; set; }

    protected Func<IDisposable[]>? WithDisappear { get; set; }
    protected Func<IDisposable[]>? WithDestroy { get; set; }

    public override Task InitializeAsync(INavigationParameters parameters)
    {
        if (this.Init == null)
            return Task.CompletedTask;

        if (this.WithDestroy != null)
        {
            var en = this.WithDestroy.Invoke();
            this.DestroyWith.AddRange(en);
        }

        return this.Init.Invoke(parameters);
    }


    public override Task<bool> CanNavigateAsync(INavigationParameters parameters)
    {
        if (this.CanNav == null)
            return Task.FromResult(true);

        return this.CanNav.Invoke();
    }


    public override async void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        this.NavTo?.Invoke(parameters);

        if (this.NavToTask != null)
            await this.SafeExecuteAsync(() => this.NavToTask.Invoke(parameters));
    }


    public override void OnNavigatedFrom(INavigationParameters parameters)
    {
        base.OnNavigatedFrom(parameters);
        this.NavFrom?.Invoke(parameters);
    }


    public override async void OnAppearing()
    {
        base.OnAppearing();
        this.Appearing?.Invoke();
        if (this.AppearingTask != null)
            await this.SafeExecuteAsync(this.AppearingTask);

        if (this.WithDisappear != null)
        {
            var en = this.WithDisappear.Invoke();
            this.DeactivateWith.AddRange(en);
        }
    }


    public override void OnDisappearing()
    {
        base.OnDisappearing();
        this.Disappearing?.Invoke();
    }


    public override void Destroy()
    {
        base.Destroy();
        this.BeforeDestroy?.Invoke();
    }
}