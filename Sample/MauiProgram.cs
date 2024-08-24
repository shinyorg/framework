using Prism.Container.DryIoc;
using Shiny;
using Prism.Navigation;

namespace Sample;


public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp
			.CreateBuilder()
			.UseMauiApp<App>()
			// .UseShinyMvvm()
			.UseShinyFramework(
				new DryIocContainerExtension(),
				prism => prism.CreateWindow(
					"NavigationPage/MainPage",
					ex =>
					{
						Console.WriteLine(ex);
					}
				)
            )
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddLocalization();
		builder.Services.RegisterForNavigation<MainPage, MainBaseViewModel>();
		builder.Services.RegisterForNavigation<DialogsPage, DialogsBaseViewModel>();
		
		var app = builder.Build();
		return app;
	}
}

