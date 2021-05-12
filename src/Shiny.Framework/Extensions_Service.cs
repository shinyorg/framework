using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Impl;


namespace Shiny
{
    public static partial class Extensions
    {
        public static void UseXfMaterialDialogs(this IServiceCollection services)
            => services.TryAddSingleton<IDialogs, XfMaterialDialogs>();

        public static void UseGlobalCommandExceptionHandler(this IServiceCollection services, Action<GlobalExceptionHandlerConfig>? configure = null)
        {
            services.AddSingleton<GlobalExceptionHandler>();
            configure?.Invoke(GlobalExceptionHandlerConfig.Instance);
        }

        public static void UseResxLocalization(this IServiceCollection services, Assembly assembly, string resourceName)
            => services.AddSingleton<ILocalize>(new ResxLocalize(resourceName, assembly));
    }
}
