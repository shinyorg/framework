using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Extensions.Dialogs;
using Shiny.Extensions.Dialogs.XfMaterial;
using Shiny.Extensions.Localization;
using System;


namespace Shiny
{
    public static class ServiceCollectionExtensions
    {
        public static void UseXfMaterialDialogs(this IServiceCollection services, string? localizeSection = null)
        {
            // user will still have to call XF.Material.Forms.Material.Init in App ctor
            services.TryAddSingleton<IDialogs>(sp =>
            {
                ILocalizationSource? localSource = null;

                if (localizeSection != null)
                {
                    localSource = sp.GetRequiredService<ILocalizationManager>().GetSection(localizeSection);
                    if (localSource == null)
                        throw new ArgumentException($"No localization section '{localizeSection}' was found");
                }

                return new XfMaterialDialogs(sp.GetRequiredService<IPlatform>(), localSource);
            });
        }
    }
}
