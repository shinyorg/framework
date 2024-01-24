using Microsoft.Extensions.Logging;
using Prism.Container.DryIoc;
using Shiny;
using Prism.Navigation;

namespace Sample;


public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		try
		{
			var builder = MauiApp
				.CreateBuilder()
				.UseMauiApp<App>()
				.UseShinyFramework(
					new DryIocContainerExtension(),
					prism => prism.OnAppStart(
						"NavigationPage/MainPage",
						ex =>
						{
							Console.WriteLine(ex);
						}
					),
					new(ErrorAlertType.FullError)
				)
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});
#if DEBUG
			builder.Logging.AddDebug();
#endif
			builder.Services.AddLocalization();
			builder.Services.AddDataAnnotationValidation();

			builder.Services.RegisterForNavigation<MainPage, MainViewModel>();
			builder.Services.RegisterForNavigation<DialogsPage, DialogsViewModel>();
			builder.Services.RegisterForNavigation<ValidationPage, ValidationViewModel>();

			return builder.Build();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			throw;
		}
	}
}

