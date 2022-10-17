using Prism.DryIoc;
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
			.UseShinyFramework(
				new DryIocContainerExtension(),
				prism => prism.OnAppStart("NavigationPage/MainPage")
			)
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddGlobalCommandExceptionHandler();
		builder.Services.AddDataAnnotationValidation();
		// TODO: localization

		builder.Services.RegisterForNavigation<MainPage, MainViewModel>();
		builder.Services.RegisterForNavigation<DialogsPage, DialogsViewModel>();
		builder.Services.RegisterForNavigation<ValidationPage, ValidationViewModel>();

		return builder.Build();
	}
}

