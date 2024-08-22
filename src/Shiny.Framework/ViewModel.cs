using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using Shiny.Net;

namespace Shiny;


public abstract partial class ViewModel(BaseServices services) : ObservableObject, IDestructible
{
    protected BaseServices Services => services;
    protected INavigationService Navigation => this.Services.Navigation;



    [RelayCommand]
    async Task Navigate(string uri)
    {
        await this.Navigation.Navigate(uri);
    }

    [ObservableProperty] bool isBusy;
    [ObservableProperty] string? title;
    
    
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
                        // this.RaisePropertyChanged(nameof(this.IsInternetAvailable));
                    });
                // TODO: dispose
            }
            return this.internetAvailable!.Value;
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
    protected ILogger Logger => this.logger ??= this.Services.LoggerFactory.CreateLogger(this.GetType());
    
    /// <summary>
    /// Dialog service from the service provider
    /// </summary>
    public IDialogs Dialogs => this.Services.Dialogs;

    IStringLocalizer? localizer;
    /// <summary>
    /// The localization source for this instance - will attempt to use the default section (if registered)
    /// </summary>
    public IStringLocalizer? Localize => this.localizer ??= this.Services.StringLocalizationFactory?.Create(this.GetType());


    /// <summary>
    /// Shiny Connectivity Service 
    /// </summary>
    public IConnectivity Connectivity => this.Services.Connectivity;
    
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
    /// Reads localization key from localization service
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual string this[string key]
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