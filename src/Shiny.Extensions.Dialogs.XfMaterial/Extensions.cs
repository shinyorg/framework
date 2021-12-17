using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Extensions.Dialogs;
using Shiny.Extensions.Dialogs.XfMaterial;


namespace Shiny
{
    public static class ServiceCollectionExtensions
    {
        public static void UseXfMaterialDialogs(this IServiceCollection services)
        {
            // user will still have to call XF.Material.Forms.Material.Init in App ctor
            services.TryAddSingleton<IDialogs, XfMaterialDialogs>();
        }
    }
}
