using System;
using System.Collections.Generic;

namespace Shiny
{
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
        IDictionary<string, string> Validate(object obj);

        /// <summary>
        /// Pass an object to run it through all validations
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool IsValid(object obj);
    }
}
