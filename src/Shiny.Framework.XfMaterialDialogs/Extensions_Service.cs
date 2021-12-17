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
        public static void UseXfMaterialDialogs(this IServiceCollection services)
            => services.TryAddSingleton<IDialogs, XfMaterialDialogs>();
    }
}
