using Shiny.Applications;
using Shiny.Applications.Maui;
using Shiny.Applications.Maui.Infrastructure;

namespace Shiny;


public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseShinyMvvm(this MauiAppBuilder builder)
    {
#if ANDROID || IOS || MACCATALYST
        builder.UseShiny();
#endif
        builder.Services.AddSingleton<INavigator, ShellNavigator>();
        builder.Services.AddSingleton<IDialogs, MauiDialogs>();
        return builder;
    }


    public static MauiAppBuilder RegisterRoute<TPage, TViewModel>(this MauiAppBuilder builder, string route)
        where TPage : Page
        where TViewModel : class
    {
        ShinyShell.AddRoute(route, typeof(TPage), typeof(TViewModel));
        
        builder.Services.AddTransient<TViewModel>();
        return builder;
    }
}