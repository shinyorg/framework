using System;

namespace Shiny;


public class FuncViewModel : ViewModel
{
    public FuncViewModel(BaseServices services) : base(services) {}


    protected Func<IDisposable, Task>? OnStart { get; set; }
    protected Action<INavigationParameters, IDisposable>? NavTo { get; set; }


    public override async Task InitializeAsync(INavigationParameters parameters)
    {
        if (this.OnStart != null)
            await this.OnStart.Invoke(this.DestroyWith);

        await base.InitializeAsync(parameters);
    }


    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        this.NavTo?.Invoke(parameters, this.DeactivateWith);
    }
}

