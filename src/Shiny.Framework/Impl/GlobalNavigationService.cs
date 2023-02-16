#if PLATFORM
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Prism.Common;
using Prism.Navigation.Xaml;
using Shiny;

namespace Shiny.Impl;


public class GlobalNavigationService : IGlobalNavigationService
{
    readonly IApplication application;
    readonly IPlatform platform;


    public GlobalNavigationService(IApplication application, IPlatform platform)
    {
        this.application = application;
        this.platform = platform;
    }


    public Task<INavigationResult> GoBackAsync(INavigationParameters parameters)
        => this.Run(nav => nav.GoBackAsync(parameters));

    public Task<INavigationResult> GoBackToAsync(string name, INavigationParameters parameters)
        => this.Run(nav => nav.NavigateAsync(name, parameters));

    public Task<INavigationResult> GoBackToRootAsync(INavigationParameters parameters)
        => this.Run(nav => nav.GoBackAsync(parameters));

    public Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameters)
        => this.Run(nav => nav.NavigateAsync(uri, parameters));


    async Task<INavigationResult> Run(Func<INavigationService, Task<INavigationResult>> func)
    {
        var window = this.application.Windows.OfType<Window>().First();
        var currentPage = MvvmHelpers.GetCurrentPage(window.Page);
        var container = currentPage.GetContainerProvider();

        var navService = container.Resolve<INavigationService>();
        var result = await this.platform.InvokeTaskOnMainThread(() => func(navService));
        return result;
    }
}
#endif