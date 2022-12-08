#if PLATFORM
using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Impl;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Maui;

namespace Shiny;


public static class MauiExtensions
{
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
        builder.Services.AddConnectivity();

        builder.Services.AddScoped<BaseServices>();
        return builder;
    }
}
#endif
