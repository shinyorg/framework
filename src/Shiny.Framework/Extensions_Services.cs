using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Extensions.Localization;
using Shiny.Impl;

namespace Shiny;


public static class ServiceExtensions
{
    public static void AddDataAnnotationValidation(this IServiceCollection services)
        => services.TryAddSingleton<IValidationService, DataAnnotationsValidationService>();


    public static void ConfigureLocalization(this IServiceCollection services, Action<LocalizationBuilder> builderAction, string? defaultSection = null)
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


    public static void AddGlobalCommandExceptionHandler(this IServiceCollection services, Action<GlobalExceptionHandlerConfig>? configure = null)
    {
        services.AddShinyService<GlobalExceptionHandler>();
        configure?.Invoke(GlobalExceptionHandlerConfig.Instance);
    }
}
