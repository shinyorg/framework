using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation;
using System.Reflection;


namespace Shiny
{
    public abstract class FrameworkApplication : PrismApplication
    {
        public static bool FirstRun { get; private set; } = true;


        protected override async void OnInitialized()
            => await FrameworkStartup.Current!.RunApp(NavigationService);


        protected override void Initialize()
        {
            if (FirstRun)
            {
                base.Initialize();
            }
            else
            {
                // use reflection to set _containerExtension from ContainerLocator.Current
                var containerExtension = GetType().GetField("_containerExtension", BindingFlags.Instance | BindingFlags.NonPublic);
                containerExtension.SetValue(this, ContainerLocator.Current);

                var moduleCatalog = GetType().GetField("_moduleCatalog", BindingFlags.Instance | BindingFlags.NonPublic);
                moduleCatalog.SetValue(this, Container.Resolve<IModuleCatalog>());
                NavigationService = Container.Resolve<INavigationService>();
            }
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
            => FrameworkStartup.Current!.ConfigureApp(this, containerRegistry);
    }
}
