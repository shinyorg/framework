using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Shiny.Extensions.Localization;
using Shiny.Hosting;
using Shiny.Stores;

namespace Shiny;


public abstract class BaseViewModel : ReactiveObject, IDestructible, IValidationViewModel
{
    readonly BaseServices services;
    protected BaseViewModel(BaseServices services) => this.services = services;


    protected INavigationService Navigation => this.services.Navigation;


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
                this.internetAvailable = this.services.Connectivity.IsInternetAvailable();

                this.services
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
        if (this.Validation == null && this.services.Validation != null)
        {
            this.Validation = this.services.Validation.Bind(this);
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
    protected ILogger Logger
    {
        get
        {
            this.logger ??= this.services.LoggerFactory.CreateLogger(this.GetType().AssemblyQualifiedName!);
            return this.logger;
        }
        set => this.logger = value;
    }


#if PLATFORM
    /// <summary>
    /// Access to platform services
    /// </summary>
    public IPlatform Platform => this.services.Platform;
#endif

    /// <summary>
    /// Dialog service from the service provider
    /// </summary>
    public IDialogs Dialogs => this.services.Dialogs;


    /// <summary>
    /// Localization manager from the service provider
    /// </summary>
    public ILocalizationManager? LocalizationManager => this.services.localizeManager;


    /// <summary>
    /// The localization source for this instance - will attempt to use the default section (if registered)
    /// </summary>
    public ILocalizationSource? Localize => this.services.Localize;


    /// <summary>
    /// Monitors for viewmodel changes and returns true if valid - handy for ReactiveCommand in place of WhenAny
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public IObservable<bool> WhenValid()
        => this.WhenAnyProperty().Select(_ => {
            if (this.services.Validation == null)
                throw new InvalidOperationException("Validation service is not registered");

            var result = this.services.Validation.IsValid(this);
            return result;
        });


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
            await this.services.ErrorHandler.Process(ex);
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
            await this.services.ErrorHandler.Process(ex);
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
        this.services.ObjectBinder.Bind(this);
        this.DestroyWith.Add(Disposable.Create(() =>
            this.services.ObjectBinder.UnBind(this)
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
            if (this.LocalizationManager == null)
                throw new InvalidOperationException("Localization has not been initialized in your DI container");

            if (key.Contains(":") || this.Localize == null)
                return this.LocalizationManager[key];

            return this.Localize[key];
        }
    }
}
