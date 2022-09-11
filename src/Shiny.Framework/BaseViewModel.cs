using System.Reactive.Disposables;
using System.Reactive.Linq;
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
                this.services
                    .Connectivity
                    .WhenInternetStateChanged()
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(x =>
                    {
                        this.internetAvailable = x;
                        this.RaisePropertyChanged(nameof(this.IsInternetAvailable));
                    })
                    .DisposeWith(this.DestroyWith);
            }
            return this.internetAvailable!.Value;
        }
    }


    IValidationBinding? validationBinding;
    public virtual IValidationBinding? Validation
    {
        get
        {
            if (this.validationBinding == null && this.services.Validation != null)
            {
                this.validationBinding = this.services.Validation.Bind(this);
                this.DestroyWith.Add(this.validationBinding);
            }
            return this.validationBinding;
        }
    }


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
    /// Will trap any errors - log them and display a message to the user
    /// </summary>
    /// <param name="action"></param>
    /// <param name="userMessage"></param>
    /// <param name="title"></param>
    /// <param name="dialogBtn"></param>
    protected virtual async void SafeExecute(Action action, string userMessage, string title = "Error", string dialogBtn = "OK")
    {
        try
        {
            action.Invoke();
        }
        catch (Exception ex)
        {
            this.Logger.LogError(userMessage, ex);
            await this.Dialogs.Alert(userMessage, title, dialogBtn);
        }
    }


    /// <summary>
    /// Will trap any errors - log them and display a message to the user
    /// </summary>
    /// <param name="func"></param>
    /// <param name="userMessage"></param>
    /// <param name="title"></param>
    /// <param name="dialogBtn"></param>
    /// <returns></returns>
    protected virtual async Task SafeExecuteAsync(Func<Task> func, string userMessage, string title = "Error", string dialogBtn = "OK")
    {
        try
        {
            await func.Invoke().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(userMessage, ex);
            await this.Dialogs.Alert(userMessage, title, dialogBtn);
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
