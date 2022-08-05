using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Impl;

namespace Shiny;


public static class MauiExtensions
{
    public static MauiAppBuilder UseShinyFramework<TApp>(this MauiAppBuilder builder, IContainerExtension container, Action<PrismAppBuilder> prismBuilder)
            where TApp : Application
    {
        builder
#if PLATFORM
            .UseShiny()
#endif
            .UsePrismApp<TApp>(container, prismBuilder);
#if PLATFORM
        builder.Services.TryAddScoped<IDialogs, NativeDialogs>();
#endif
        builder.Services.TryAddSingleton(AppInfo.Current);
        builder.Services.TryAddSingleton(Connectivity.Current);

        builder.Services.AddScoped<BaseServices>();

        return builder;
    }


    public static IObservable<bool> WhenInternetStateChanged(this IConnectivity connectivity, bool includeConstrained = true) => Observable
        .Create<bool>(ob =>
        {
            var handler = new EventHandler<ConnectivityChangedEventArgs>((sender, args) =>
                ob.OnNext(args.NetworkAccess.IsInternetAvailable(includeConstrained))
            );
            connectivity.ConnectivityChanged += handler;

            ob.OnNext(connectivity.NetworkAccess.IsInternetAvailable());

            return () => connectivity.ConnectivityChanged -= handler;
        });



    public static bool IsInternetAvailable(this NetworkAccess access, bool includeConstrained = true)
        => access == NetworkAccess.Internet || (
            includeConstrained &&
            access == NetworkAccess.ConstrainedInternet
        );
}