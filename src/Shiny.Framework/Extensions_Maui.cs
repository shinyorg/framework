#if PLATFORM
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Maui;
using Prism.Ioc;
using Prism;
using Shiny.Impl;

namespace Shiny;


public static partial class MauiExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="container"></param>
    /// <param name="prismBuilder"></param>
    /// <param name="exceptionConfig"></param>
    /// <returns></returns>
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
        builder.Services.TryAddSingleton(AppInfo.Current);
        builder.Services.AddConnectivity();
        builder.Services.AddScoped<BaseServices>();

        // RxApp.DefaultExceptionHandler = new GlobalExceptionHandler();
        return builder;
    }
}
#endif