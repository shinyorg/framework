using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Prism.Xaml;

namespace Shiny;


public abstract class FuncViewModel : ViewModel
{
    protected FuncViewModel(BaseServices services) : base(services) {}
    protected Func<INavigationParameters, CompositeDisposable, Task>? NavTo { get; set; }
    protected Func<Task>? Appearing { get; }


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