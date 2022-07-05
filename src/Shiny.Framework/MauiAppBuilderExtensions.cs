using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Impl;

namespace Shiny;


public static class MauiAppBuilderExtensions
{
	public static MauiAppBuilder UseShinyFramework<TApp>(this MauiAppBuilder builder, Action<PrismAppBuilder> prismBuilder)
		where TApp : Application, IContainerExtension
	{
		builder
			.UseShiny()
			.UsePrismApp<TApp>(null, prismBuilder);

		builder.Services.TryAddSingleton(AppInfo.Current);
		builder.Services.TryAddSingleton<IDialogs, NativeDialogs>();
		return builder;
	}
}