using Acr.UserDialogs;
using Microsoft.Extensions.DependencyInjection;
using Shiny.Extensions.Dialogs;
using Shiny.Extensions.Dialogs.AcrUserDialogs;
using Shiny.Extensions.Localization;

using System;

namespace Shiny
{
    public static class ServiceCollectionExtensions
    {
        public static void UseAcrUserDialogs(this IServiceCollection services, string? localizeSection = null)
        {
            services.AddSingleton<IDialogs, AcrUserDialogsImpl>(sp =>
            {
                ILocalizationSource? localSource = null;

                if (localizeSection != null)
                {
                    localSource = sp.GetRequiredService<ILocalizationManager>().GetSection(localizeSection);
                    if (localSource == null)
                        throw new ArgumentException($"No localization section '{localizeSection}' was found");
                }

                return new AcrUserDialogsImpl(UserDialogs.Instance, localSource);
            });
        }
    }
}
