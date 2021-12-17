using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Extensions.Dialogs.XfMaterial;

namespace Shiny
{
    public static class ServiceCollectionExtensions
    {
        public static void UseXfMaterialDialogs(this IServiceCollection services)
            => services.TryAddSingleton<IDialogs, XfMaterialDialogs>();
    }
}
