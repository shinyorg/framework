using System.Collections.Generic;

namespace Shiny;


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
