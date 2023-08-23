using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReactiveUI;
using Shiny.Impl;

namespace Shiny;


public static class ServiceExtensions
{
    public static void AddDataAnnotationValidation(this IServiceCollection services)
        => services.TryAddSingleton<IValidationService, DataAnnotationsValidationService>();


    public static void AddGlobalCommandExceptionHandler(this IServiceCollection services, GlobalExceptionHandlerConfig? config = null)
    {
        RxApp.DefaultExceptionHandler = new GlobalExceptionHandler();
        services.AddGlobalCommandExceptionAction();
    }


    internal static void AddGlobalCommandExceptionAction(this IServiceCollection services, GlobalExceptionHandlerConfig? config = null)
    {
        services.TryAddSingleton(config ?? new GlobalExceptionHandlerConfig());
        services.TryAddSingleton<GlobalExceptionAction>();
    }
}
