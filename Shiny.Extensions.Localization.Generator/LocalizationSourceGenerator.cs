using System;

namespace Shiny.Extensions.Localization.Generator
{
    public class Class1
    {

    }
}

/*
// this entire file would be source generated
using Microsoft.Extensions.Localization;

namespace Sample;


public static class GeneratedExtensions
{
	
	public static void AddStronglyTypedStrings(this IServiceCollection services)
	{
		services.AddSingleton<MainViewModelLocalization>();
	}
}

public class MainViewModelLocalization
{
	readonly IStringLocalizer localizer;


	public MainViewModelLocalization(IStringLocalizer<MainViewModel> localizer)
	{
		this.localizer = localizer;
	}


	public string LocalizeKey => this.localizer["LocalizeKey"];
}

 */