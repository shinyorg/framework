using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Prism.Navigation;
using Shiny.Reflection;

namespace Shiny;


public static class PrismExtensions
{
    public static bool IsBack(this INavigationParameters parameters)
        => parameters.GetNavigationMode() == NavigationMode.Back;

    public static bool IsNew(this INavigationParameters parameters)
        => parameters.GetNavigationMode() == NavigationMode.New;

    public static void WhenAnyValueSelected<TViewModel, TRet>(this TViewModel viewModel, Expression<Func<TViewModel, TRet>> expression, Action<TRet> action) where TViewModel : BaseViewModel
    {
        var p = viewModel.GetPropertyInfo(expression);
        if (!p.CanWrite)
            throw new ArgumentException("Cannot write property");

        viewModel
            .WhenAnyProperty(expression)
            .Where(x => x != null)
            .Subscribe(x =>
            {
                p.SetValue(viewModel, null);
                action(x);
            })
            .DisposedBy(viewModel.DestroyWith);
    }


    public static Task Navigate(this INavigationService navigation, string uri, params (string, object)[] parameters)
        => navigation.Navigate(uri, parameters.ToNavParams());


    public static async Task Navigate(this INavigationService navigation, string uri, INavigationParameters parameters)
        => (await navigation.NavigateAsync(uri, parameters)).Assert();


    public static void Assert(this INavigationResult result)
    {
        if (!result.Success)
        {
            Console.WriteLine("[NAV FAIL] " + result.Exception);
            throw new ArgumentException("Failed to navigate", result.Exception);
        }
    }

    public static Task GoBack(this INavigationService navigation, bool toRoot = false, params (string, object)[] parameters)
        => navigation.GoBack(toRoot, parameters.ToNavParams());


    public static async Task GoBack(this INavigationService navigation, bool toRoot = false, INavigationParameters parameters = null)
    {
        parameters = parameters ?? new NavigationParameters();
        var task = toRoot
            ? navigation.GoBackToRootAsync(parameters)
            : navigation.GoBackAsync(parameters);

        var result = await task.ConfigureAwait(false);
        result.Assert();
    }

    public static INavigationParameters Set(this INavigationParameters parameters, string key, object value)
    {
        parameters.Add(key, value);
        return parameters;
    }


    public static INavigationParameters AddRange(this INavigationParameters parameters, params (string Key, object Value)[] args)
    {
        foreach (var arg in args)
            parameters.Add(arg.Key, arg.Value);

        return parameters;
    }


    static NavigationParameters ToNavParams(this (string Key, object Value)[] parameters)
    {
        var navParams = new NavigationParameters();
        if (parameters != null && parameters.Any())
            navParams.AddRange(parameters);

        return navParams;
    }
}
