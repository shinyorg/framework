using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            var context = new ValidationContext(obj);
            if (propertyName != null)
                context.MemberName = propertyName;

            return Validator.TryValidateObject(
                obj,
                context,
                null
            );
        }


        public IDisposable Subscribe(IValidationViewModel viewModel, bool setFirstError)
        {
            var results = new List<ValidationResult>();
            return viewModel
                .WhenAnyProperty()
                .SubOnMainThread(x =>
                {
                    if (!viewModel.Touched.ContainsKey(x.Value))
                        viewModel.Touched[x.Value] = true;

                    results.Clear();
                    var result = Validator.TryValidateObject(
                        viewModel,
                        new ValidationContext(viewModel)
                        {
                            MemberName = x.Value
                        },
                        results
                    );

                    viewModel.Errors.Remove(x.Value);
                    if (!result)
                    {
                        // TODO: take first issue or all?  Make it configurable?
                        viewModel.Errors[x.Value] = this.GetErrorMessage(results[0]!);
                    }
                    // TODO: fire change notifications for the dictionary and dictionary value?  possible?
                    //viewModel.RaisePropertyChanged()
                });
        }


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
            Validator.TryValidateObject(
                obj,
                new ValidationContext(obj) { MemberName = propertyName },
                results
            );
            return results
                .Select(x => this.GetErrorMessage(x))
                .ToArray();
        }


        protected virtual string GetErrorMessage(ValidationResult result)
        {
            if (!result.ErrorMessage.IsEmpty())
                return result.ErrorMessage!;

            // TODO: localization stuff - use objecttype?
            return String.Empty;
        }
    }
}