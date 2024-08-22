namespace Shiny.Applications;


public abstract class ViewModel : NotifyPropertyChanged, ILifecycle
{
    public Task<bool> CanNavigateAway() => Task.FromResult(true);

    public void OnAppearing()
    {
        Console.WriteLine("ViewModel OnAppearing");
    }

    public void OnDisappearing()
    {
        Console.WriteLine("ViewModel OnDisappearing");
    }

    public virtual void OnDestroy()
    {
        Console.WriteLine("ViewModel OnDestroy");
    }

    public virtual Task OnNavigatedFrom() => Task.CompletedTask;
    public virtual Task OnNavigatedTo() => Task.CompletedTask;
}