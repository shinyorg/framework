using Prism.DryIoc;
using Prism.Ioc;


namespace Shiny
{
    public abstract class FrameworkApplication : PrismApplication
    {
        public static bool FirstRun { get; private set; } = true;


        protected override async void OnInitialized()
            => await FrameworkStartup.Current!.RunApp(NavigationService);


        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            if (FirstRun)
                base.RegisterRequiredTypes(containerRegistry);
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            if (FirstRun)
                FrameworkStartup.Current!.ConfigureApp(this, containerRegistry);
        }
    }
}
