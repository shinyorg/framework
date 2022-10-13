using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive.Linq;
using Shiny.Extensions.Localization;


namespace Shiny.Impl
{
    public class DataAnnotationsValidationService : IValidationService
    {
        readonly ILocalizationManager? localizationManager;


        public DataAnnotationsValidationService(ILocalizationManager? localizationManager = null)
            => this.localizationManager = localizationManager;


        public bool IsValid(object obj, string? propertyName = null)
        {
            if (propertyName == null)
            {
                var context = new ValidationContext(obj);
                var results = new List<ValidationResult>();
                var result = Validator.TryValidateObject(
                    obj,
                    context,
                    results,
                    true
                );
                return result;
            }
            return !this.ValidateProperty(obj, propertyName).Any();
        }


        public IValidationBinding Bind(IValidationViewModel viewModel)
            => new ValidationBinding(this, viewModel);


        public IDictionary<string, IList<string>> Validate(object obj)
        {
            var values = new Dictionary<string, IList<string>>();
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(
                obj,
                new ValidationContext(obj),
                results
            );

            foreach (var result in results)
            {
                foreach (var member in result.MemberNames)
                {
                    if (!values.ContainsKey(member))
                        values.Add(member, new List<string>());

                    var errMsg = this.GetErrorMessage(result);
                    values[member].Add(errMsg);
                }
            }
            return values;
        }


        public IEnumerable<string> ValidateProperty(object obj, string propertyName)
        {
            var results = new List<ValidationResult>();
            var value = GetValue(obj, propertyName);

            Validator.TryValidateProperty(
                value,
                new ValidationContext(obj) { MemberName = propertyName },
                results
            );
            foreach (var result in results)
            {
                if (result.MemberNames.Contains(propertyName))
                {
                    yield return this.GetErrorMessage(result);
                }
            }
        }


        protected virtual string GetErrorMessage(ValidationResult result)
        {
            if (result.ErrorMessage.StartsWith("localize:"))
            {
                if (this.localizationManager == null)
                    throw new ArgumentException("Localization has not been put into your startup");

                var key = result.ErrorMessage.Replace("localize:", String.Empty);
                return this.localizationManager[key] ?? key;
            }
            return result.ErrorMessage!;
        }


        protected static object? GetValue(object obj, string propertyName)
            => obj.GetType().GetProperty(propertyName).GetValue(obj, null);
    }
}