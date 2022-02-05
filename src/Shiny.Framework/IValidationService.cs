using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Shiny
{
    public static class ValidationServiceExtensions
    {
        public static bool IsValidProperty<T>(this IValidationService service, T obj, Expression<Func<T, string>> expression)
        {
            return service.IsValid(obj, null);
        }


        public static IEnumerable<string> ValidateProperty<T>(this IValidationService service, T obj, Expression<Func<T, string>> expression)
        {
            return service.ValidateProperty(obj, null);
        }
    }


    public interface IValidationService
    {
        /// <summary>
        /// Monitors the viewmodel for changes and sets it's Touched & Errors dictionary as the user changes
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        IDisposable Subscribe(IValidationViewModel viewModel);


        /// <summary>
        /// Returns a dictionary of invalid properties with their corresponding error messages
        /// </summary>
        /// <param name="obj"></param>
        IDictionary<string, IList<string>> Validate(object obj);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        IEnumerable<string> ValidateProperty(object obj, string propertyName);


        /// <summary>
        /// Pass an object to run it through all validations
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool IsValid(object obj, string? propertyName = null);
    }
}
