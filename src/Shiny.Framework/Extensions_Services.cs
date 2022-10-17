using Microsoft.Extensions.DependencyInjection.Extensions;
using ReactiveUI;
using Shiny.Extensions.Localization;
using Shiny.Impl;

namespace Shiny;


public static class ServiceExtensions
{
    public static void AddDataAnnotationValidation(this IServiceCollection services)
        => services.TryAddSingleton<IValidationService, DataAnnotationsValidationService>();


    public static void AddLocalization(this IServiceCollection services, Action<LocalizationBuilder> builderAction, string? defaultSection = null)
    {
        var builder = new LocalizationBuilder();
        builderAction(builder);
        var localizationManager = builder.Build();

        services.AddSingleton(localizationManager);
        if (defaultSection != null)
        {
            var section = localizationManager.GetSection(defaultSection);
            if (section == null)
                throw new InvalidOperationException($"Invalid Default Section Name: " + defaultSection);

            services.AddSingleton(section);
        }
    }


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
