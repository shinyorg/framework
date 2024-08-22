using Microsoft.Extensions.Logging;
using Shiny.Hosting;

namespace Shiny.Applications.Maui;


public class ShinyShell : Shell
{
    //https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation?view=net-maui-8.0
    static readonly Dictionary<string, Type> pageRoutes = new();
    static readonly Dictionary<Type, Type> pageToViewModelMap = new();
    internal static ShinySubject<object> WhenNav { get; } = new();

    readonly IServiceProvider services;
    readonly ILogger logger;


    public ShinyShell()
    {
        this.services = Host.ServiceProvider;
        this.logger = services.GetService<ILogger<ShinyShell>>()!;

        foreach (var route in pageRoutes)
        {
            Routing.RegisterRoute(route.Key, route.Value);
        }
    }


    protected override async void OnNavigating(ShellNavigatingEventArgs args)
    {
        if (this.CurrentPage == null)
            return;

        var page = this.CurrentPage!;
        var lc = page?.BindingContext as ILifecycle;
        this.logger.LogDebug($"OnNavigating: {page.GetType().FullName} ({page.BindingContext?.GetType()}) - {args.Source}");
        this.logger.LogDebug($"Current: {args.Current.Location} - Target: {args.Target.Location}");

        //if (lc != null)
        //{
        //    var token = args.GetDeferral();
        //    var result = await lc.CanNavigateAway();
        //    if (result)
        //    {
        //        token.Complete();
        //        args.Cancel();
        //    }
        //    //args.CanCancel
        //}

        //e.Current.Location
        //e.Target.Location

        switch (args.Source)
        {
            case ShellNavigationSource.Unknown:
                break;

            case ShellNavigationSource.Push:
                // TODO: lifecycle naving away
                break;

            case ShellNavigationSource.Pop:
                // can nav?
                // disappearing?
                lc?.OnDestroy();
                break;

            case ShellNavigationSource.PopToRoot:
                // disappearing?
                lc?.OnDestroy();
                // TODO: Check lifecycle cannavigateaway
                // TODO: fire destory lifecycle here?
                break;

            case ShellNavigationSource.Remove:
                lc?.OnDestroy();
                break;


            case ShellNavigationSource.ShellItemChanged:
                break;

            case ShellNavigationSource.ShellSectionChanged:
                break;

            case ShellNavigationSource.ShellContentChanged:
                break;
        }

        //e.CanCancel
        //e.Current.Location
        //e.Source == ShellNavigationSource.Insert
        //e.Target.Location
        base.OnNavigating(args);
    }


    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        // where we are now - we may need to stick the viewmodel on this now or is it too late?
        var page = this.CurrentPage;

        this.logger.LogDebug($"OnShellNavigated: {page.GetType().FullName} - {args.Source}");
        var vmType = pageToViewModelMap[page.GetType()];
        if (vmType == null)
            throw new InvalidOperationException("No viewmodel registered");

        if (page.BindingContext == null)
        {
            var viewModel = this.services.GetRequiredService(vmType);    
            page.BindingContext = viewModel;
        }
        var lifecycle = page.BindingContext as ILifecycle;

        //e.Current.Location
        switch (args.Source)
        {
            case ShellNavigationSource.Push:
                // async
                lifecycle?.OnNavigatedTo();
                break;

            case ShellNavigationSource.Pop:
                // async
                lifecycle?.OnNavigatedFrom();
                lifecycle?.OnDestroy();
                break;

            case ShellNavigationSource.PopToRoot:
                lifecycle?.OnNavigatedFrom();
                lifecycle?.OnDestroy();
                break;

            case ShellNavigationSource.Remove:
                //lifecycle.OnNavigatedFrom();
                lifecycle?.OnDestroy();
                break;

            // tabs/flyout changess
            case ShellNavigationSource.ShellItemChanged:
                break;

            case ShellNavigationSource.ShellSectionChanged:
                break;

            case ShellNavigationSource.ShellContentChanged:
                break;
        }
        //e.Previous.Location
        //e.Source == ShellNavigationSource.Insert
        //e.Current.Location
    }


    protected virtual object? CreateViewModelForRoute(string route)
    {
        if (!pageRoutes.ContainsKey(route))
            return null;

        var vType = pageRoutes[route];
        if (vType == null)
            return null;

        return this.services.GetService(vType);
    }


    internal static void AddRoute(string route, Type pageType, Type? viewModelType = null)
    {
        pageRoutes.Add(route, pageType);

        if (viewModelType != null)
            pageToViewModelMap.Add(pageType, viewModelType);
    }
}