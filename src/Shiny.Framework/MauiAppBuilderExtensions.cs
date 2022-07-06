using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Impl;

namespace Shiny;


public static class MauiAppBuilderExtensions
{
	public static MauiAppBuilder UseShinyFramework<TApp>(this MauiAppBuilder builder, IContainerExtension container, Action<PrismAppBuilder> prismBuilder)
		where TApp : Application
	{
		builder
			.UseShiny()
			.UsePrismApp<TApp>(container, prismBuilder);

		builder.Services.TryAddSingleton(AppInfo.Current);
		builder.Services.TryAddSingleton<IDialogs, NativeDialogs>();
		return builder;
	}
}