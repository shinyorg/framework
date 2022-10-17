using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Impl;
#if PLATFORM
using CommunityToolkit.Maui;
#endif

namespace Shiny;


public static class MauiExtensions
{
    #if PLATFORM

    public static MauiAppBuilder UseShinyFramework(this MauiAppBuilder builder, IContainerExtension container, Action<PrismAppBuilder> prismBuilder)
    {
        builder
            .UsePrism(container, prismBuilder)
            .UseShiny();

        if (!builder.Services.Any(x => x.ServiceType == typeof(IDialogs)))
        {
            builder.UseMauiCommunityToolkit();
            builder.Services.AddSingleton<IDialogs, NativeDialogs>();
        }
        builder.Services.AddGlobalCommandExceptionAction();
        builder.Services.TryAddSingleton(AppInfo.Current);
        builder.Services.TryAddSingleton(Connectivity.Current);

        builder.Services.AddScoped<BaseServices>();
        return builder;
    }

    #endif


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