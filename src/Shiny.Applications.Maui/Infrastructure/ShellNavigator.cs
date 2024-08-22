using Microsoft.Extensions.Logging;

namespace Shiny.Applications.Maui.Infrastructure;


public class ShellNavigator : INavigator
{
    readonly ILogger logger;
    readonly IServiceProvider services;

    public ShellNavigator(IServiceProvider services, ILogger<ShellNavigator> logger)
	{
        this.services = services;
        this.logger = logger;
        
	}


    // TODO: nav args
    public Task GoTo(string uri)
        => Shell.Current.GoToAsync(uri, true);


    public IObservable<object> WhenNavigating() => ShinyShell.WhenNav;
        // TODO: where are we leaving from
        // TODO: where are we going to
        // TODO: what are the args?
}