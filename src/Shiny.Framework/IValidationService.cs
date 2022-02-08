using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;


namespace Shiny
{
    public static class ValidationServiceExtensions
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


        /// <summary>
        /// Monitors an INotifyPropertyChanged interface for changes and returns true if valid - handy for ReactiveCommand in place of WhenAny
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static IObservable<bool> WhenValid(this INotifyPropertyChanged viewModel) 
            => viewModel.WhenAnyProperty().Select(_ => ShinyHost.Resolve<IValidationService>().IsValid(viewModel));
    }


    public interface IValidationService
    {
        /// <summary>
        /// Monitors the viewmodel for changes and sets it's Touched & Errors dictionary as the user changes
        /// </summary>
        /// <param name="viewModel">Your viewmodel that subscribes to IValidationViewModel</param>
        /// <returns></returns>
        IValidationBinding Bind(IValidationViewModel viewModel);


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
