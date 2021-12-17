using System;
using System.Threading.Tasks;
using Prism;
using Prism.Common;
using Prism.Ioc;
using Prism.Navigation;


namespace Shiny.Impl
{
    class GlobalNavigationService : IGlobalNavigationService
    {
        private const string NavigationServiceName = "PageNavigationService";
        readonly IContainerExtension container;
        readonly IApplicationProvider provider;


        public GlobalNavigationService(IContainerExtension container, IApplicationProvider applicationProvider)
        {
            this.container = container;
            this.provider = applicationProvider;
        }


        Task<INavigationResult> INavigationService.GoBackAsync()
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.GoBackAsync();
        }


        Task<INavigationResult> INavigationService.GoBackAsync(INavigationParameters parameters)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.GoBackAsync(parameters);
        }


        Task<INavigationResult> INavigationService.NavigateAsync(Uri uri)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(uri);
        }


        Task<INavigationResult> INavigationService.NavigateAsync(Uri uri, INavigationParameters parameters)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(uri, parameters);
        }


        Task<INavigationResult> INavigationService.NavigateAsync(string name)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(name);
        }


        Task<INavigationResult> INavigationService.NavigateAsync(string name, INavigationParameters parameters)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(name, parameters);
        }


        Task<INavigationResult> INavigationService.GoBackAsync(INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.GoBackAsync(parameters, useModalNavigation, animated);
        }


        Task<INavigationResult> INavigationService.GoBackToRootAsync(INavigationParameters parameters)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.GoBackToRootAsync(parameters);
        }


        Task<INavigationResult> INavigationService.NavigateAsync(string name, INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(name, parameters, useModalNavigation, animated);
        }


        Task<INavigationResult> INavigationService.NavigateAsync(Uri uri, INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(uri, parameters, useModalNavigation, animated);
        }


        INavigationService? GetNavigationService()
        {
            if (PrismApplicationBase.Current is null)
                return null;

            var navService = ((IContainerProvider)this.container).IsRegistered<INavigationService>(NavigationServiceName)
                ? container.Resolve<INavigationService>(NavigationServiceName)
                : container.Resolve<INavigationService>();

            if (navService is IPageAware pa)
            {
                pa.Page = PageUtilities.GetCurrentPage(provider.MainPage);
            }
            return navService;
        }


        Task<INavigationResult> PrismNotInitialized() => Task.FromResult<INavigationResult>(new NavigationResult
        {
            Success = false,
            Exception = new NavigationException("No Prism Application Exists", null)
        });
    }
}