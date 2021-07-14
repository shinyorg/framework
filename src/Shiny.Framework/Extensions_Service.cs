using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Prism.Common;
using Shiny.Impl;


namespace Shiny
{
    public static partial class Extensions
    {
        internal static bool UsingXfMaterialDialogs { get; private set; }
        public static void UseXfMaterialDialogs(this IServiceCollection services)
        {
            UsingXfMaterialDialogs = true;
            services.TryAddSingleton<IDialogs, XfMaterialDialogs>();
        }


        public static void UseNavigationDelegate(this IServiceCollection services)
        {
            services.AddSingleton<IGlobalNavigationService, GlobalNavigationService>();
            if (!services.Any(x => x.ServiceType == typeof(IApplicationProvider)))
                services.AddSingleton<IApplicationProvider, ApplicationProvider>();
        }


        public static void UseGlobalCommandExceptionHandler(this IServiceCollection services, Action<GlobalExceptionHandlerConfig>? configure = null)
        {
            services.AddSingleton<GlobalExceptionHandler>();
            configure?.Invoke(GlobalExceptionHandlerConfig.Instance);
        }


        public static void UseResxLocalization(this IServiceCollection services, Assembly assembly, string resourceName)
            => services.AddSingleton<ILocalize>(new ResxLocalize(resourceName, assembly));
    }
}
