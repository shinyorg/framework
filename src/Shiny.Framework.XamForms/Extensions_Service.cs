using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Prism.Common;
using Shiny.Impl;


namespace Shiny
{
    public static partial class Extensions
    {
        public static void UseGlobalNavigation(this IServiceCollection services)
        {
            services.AddSingleton<IGlobalNavigationService, GlobalNavigationService>();
            if (!services.Any(x => x.ServiceType == typeof(IApplicationProvider)))
                services.AddSingleton<IApplicationProvider, ApplicationProvider>();
        }
    }
}
