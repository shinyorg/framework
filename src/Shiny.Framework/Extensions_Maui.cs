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
using Prism.Ioc;
using Prism;
using ReactiveUI;

namespace Shiny;


public static partial class MauiExtensions
{
    public static MauiAppBuilder UseShinyFramework(this MauiAppBuilder builder, IContainerExtension container, Action<PrismAppBuilder> prismBuilder, GlobalExceptionHandlerConfig? exceptionConfig = null)
    {
        builder
            .UsePrism(container, prismBuilder)
            .UseShiny();

        if (!builder.Services.Any(x => x.ServiceType == typeof(IDialogs)))
        {
            builder.UseMauiCommunityToolkit();
            builder.Services.AddSingleton<IDialogs, NativeDialogs>();
        }
        builder.Services.AddSingleton<IGlobalNavigationService, GlobalNavigationService>();

        builder.Services.TryAddSingleton(AppInfo.Current);
        builder.Services.TryAddSingleton(exceptionConfig ?? new GlobalExceptionHandlerConfig());
        builder.Services.TryAddSingleton<GlobalExceptionAction>();
        builder.Services.AddConnectivity();
        builder.Services.AddScoped<BaseServices>();

        RxApp.DefaultExceptionHandler = new GlobalExceptionHandler();
        return builder;
    }
}
#endif
