using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Shiny.Net;
using Shiny.Stores;

namespace Shiny;


public abstract class BaseViewModel : ReactiveObject, IDestructible, IValidationViewModel
{
    protected BaseServices Services { get; }
    protected BaseViewModel(BaseServices services) => this.Services = services;


    protected INavigationService Navigation => this.Services.Navigation;


    ICommand? navigateCommand;
    public ICommand Navigate
    {
        get => this.navigateCommand ??= this.Navigation.GeneralNavigateCommand();
    }


    bool isBusy;
    public bool IsBusy
    {
        get => this.isBusy;
        set => this.RaiseAndSetIfChanged(ref this.isBusy, value);
    }


    string? title;
    public string? Title
    {
        get => this.title;
        protected set => this.RaiseAndSetIfChanged(ref this.title, value);
    }


    bool? internetAvailable;
    public virtual bool IsInternetAvailable
    {
        get
        {            
            if (this.internetAvailable == null)
            {
                this.internetAvailable = this.Services.Connectivity.IsInternetAvailable();

                this.Services
                    .Connectivity
                    .WhenInternetStatusChanged()
                    .SubOnMainThread(x =>
                    {
                        this.internetAvailable = x;
                        this.RaisePropertyChanged(nameof(this.IsInternetAvailable));
                    })
                    .DisposeWith(this.DestroyWith);
            }
            return this.internetAvailable!.Value;
        }
    }


    protected virtual void BindValidation()
    {        
        if (this.Validation == null && this.Services.Validation != null)
        {
            this.Validation = this.Services.Validation.Bind(this);
            this.DestroyWith.Add(this.Validation);
        }
    }



    public virtual IValidationBinding Validation { get; private set; } = null!;


    CompositeDisposable? deactivateWith;
    protected internal CompositeDisposable DeactivateWith => this.deactivateWith ??= new CompositeDisposable();


    CompositeDisposable? destroyWith;
    protected internal CompositeDisposable DestroyWith => this.destroyWith ??= new CompositeDisposable();


    CancellationTokenSource? deactiveToken;
    /// <summary>
    /// The destroy cancellation token - called when your model is deactivated
    /// </summary>
    protected CancellationToken DeactivateToken
    {
        get
        {
            this.deactiveToken ??= new CancellationTokenSource();
            return this.deactiveToken.Token;
        }
    }


    CancellationTokenSource? destroyToken;
    /// <summary>
    /// The destroy cancellation token - called when your model is destroyed
    /// </summary>
    protected CancellationToken DestroyToken
    {
        get
        {
            this.destroyToken ??= new CancellationTokenSource();
            return this.destroyToken.Token;
        }
    }


    ILogger? logger;
    /// <summary>
    /// A lazy loader logger instance for this viewmodel instance
    /// </summary>
    protected ILogger Logger => this.logger ??= this.Services.LoggerFactory.CreateLogger(this.GetType());


#if PLATFORM
    /// <summary>
    /// Access to platform services
    /// </summary>
    public IPlatform Platform => this.Services.Platform;
#endif

    /// <summary>
    /// Dialog service from the service provider
    /// </summary>
    public IDialogs Dialogs => this.Services.Dialogs;

    IStringLocalizer? localizer;
    /// <summary>
    /// The localization source for this instance - will attempt to use the default section (if registered)
    /// </summary>
    public IStringLocalizer? Localize => this.localizer ??= this.Services.StringLocalizationFactory!.Create(this.GetType());


    /// <summary>
    /// Shiny Connectivity Service 
    /// </summary>
    public IConnectivity Connectivity => this.Services.Connectivity;

    /// <summary>
    /// Store binder
    /// </summary>
    public IObjectStoreBinder StoreBinder => this.Services.ObjectBinder;

    /// <summary>
    /// Monitors for viewmodel changes and returns true if valid - handy for ReactiveCommand in place of WhenAny
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public IObservable<bool> WhenValid()
    {
        if (this.Services.Validation == null)
            throw new InvalidOperationException("Validation service is not registered");

        return this.WhenAnyProperty().Select(_ => this.Services.Validation.IsValid(this));
    }


    /// <summary>
    /// Will trap any errors - log them and display a message to the user
    /// </summary>
    /// <param name="action"></param>
    protected virtual async void SafeExecute(Action action)
    {
        try
        {
            action.Invoke();
        }
        catch (Exception ex)
        {
            await this.Services.ErrorHandler.Process(ex);
        }
    }


    /// <summary>
    /// Will trap any errors - log them and display a message to the user
    /// </summary>
    /// <param name="func"></param>
    /// <param name="markBusy"></param>
    /// <returns></returns>
    protected virtual async Task SafeExecuteAsync(Func<Task> func, bool markBusy = false)
    {
        try
        {
            if (markBusy)
                this.IsBusy = true;

            await func.Invoke().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await this.Services.ErrorHandler.Process(ex);
        }
        finally
        {
            this.IsBusy = false;
        }
    }


    /// <summary>
    /// This can be called manually, generally used when your viewmodel is going to the background in the nav stack
    /// </summary>
    protected virtual void Deactivate()
    {
        this.deactiveToken?.Cancel();
        this.deactiveToken?.Dispose();
        this.deactiveToken = null;

        this.deactivateWith?.Dispose();
        this.deactivateWith = null;
    }


    /// <summary>
    /// Called when the viewmodel is being destroyed (not in nav stack any longer)
    /// </summary>
    public virtual void Destroy()
    {
        this.destroyToken?.Cancel();
        this.destroyToken?.Dispose();

        this.Deactivate();
        this.destroyWith?.Dispose();
    }


    /// <summary>
    /// Binds to IsBusy while your command works
    /// </summary>
    /// <param name="command"></param>
    protected void BindBusyCommand(ICommand command)
        => this.BindBusyCommand((IReactiveCommand)command);


    /// <summary>
    /// Binds to IsBusy while your command works
    /// </summary>
    /// <param name="command"></param>
    protected void BindBusyCommand(IReactiveCommand command) =>
        command.IsExecuting.Subscribe(
            x => this.IsBusy = x,
            _ => this.IsBusy = false,
            () => this.IsBusy = false
        )
        .DisposeWith(this.DeactivateWith);


    /// <summary>
    /// Records the state of this model type for all get/set properties
    /// </summary>
    protected virtual void RememberUserState()
    {
        this.Services.ObjectBinder.Bind(this);
        this.DestroyWith.Add(Disposable.Create(() =>
            this.Services.ObjectBinder.UnBind(this)
        ));
    }


    /// <summary>
    /// Reads localization key from localization service
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual string? this[string key]
    {
        get
        {
            if (this.Localize == null)
                throw new InvalidOperationException("Localization has not been initialized in your DI container");

            var res = this.Localize[key];
            if (res.ResourceNotFound)
                return $"KEY '{key}' NOT FOUND";

            return res.Value;
        }
    }
}
