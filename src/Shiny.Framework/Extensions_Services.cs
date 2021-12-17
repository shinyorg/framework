using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Prism.Common;
using Shiny.Impl;
using System;


namespace Shiny
{
    public static class ServiceExtensions
    {
        public static void UseGlobalNavigation(this IServiceCollection services)
        {
            services.TryAddSingleton<IGlobalNavigationService, GlobalNavigationService>();
            services.TryAddSingleton<IApplicationProvider, ApplicationProvider>();
        }


        public static void UseGlobalCommandExceptionHandler(this IServiceCollection services, Action<GlobalExceptionHandlerConfig>? configure = null)
        {
            services.AddSingleton<GlobalExceptionHandler>();
            configure?.Invoke(GlobalExceptionHandlerConfig.Instance);
        }
    }
}
