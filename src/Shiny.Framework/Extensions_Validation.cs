using System;
using Shiny.Hosting;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace Shiny;


public static class ValidationExtensions
{
    /// <summary>
    /// Checks an object property and returns true if valid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="obj"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static bool IsValidProperty<T>(this IValidationService service, T obj, Expression<Func<T, string>> expression)
        => service.IsValid(obj, obj.GetPropertyInfo(expression).Name);


    /// <summary>
    /// Checks an object property to see if it is valid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="obj"></param>
    /// <param name="expression"></param>
    /// <returns></returns>

    public static IEnumerable<string> ValidateProperty<T>(this IValidationService service, T obj, Expression<Func<T, string>> expression)
        => service.ValidateProperty(obj, obj.GetPropertyInfo(expression).Name);
}

