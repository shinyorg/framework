using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using Shiny.Reflection;

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
    public static bool IsValidProperty<T>(this IValidationService service, T obj, Expression<Func<T, string>> expression) where T : notnull
        => service.IsValid(obj, obj.GetPropertyInfo(expression)?.Name);


    /// <summary>
    /// Checks an object property to see if it is valid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="obj"></param>
    /// <param name="expression"></param>
    /// <returns></returns>

    public static IEnumerable<string> ValidateProperty<T>(this IValidationService service, T obj, Expression<Func<T, string>> expression) where T : notnull
        => service.ValidateProperty(obj, obj.GetPropertyInfo(expression).Name);
}

