using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shiny.Extensions.Localization;


namespace Shiny.Impl
{
    public class DataAnnotationsValidationService : IValidationService
    {
        readonly ILocalizationManager? localizationManager;
        readonly ILocalizationSource? localizationSource;


        public DataAnnotationsValidationService(ILocalizationSource? localizationSource = null)
            => this.localizationSource = localizationSource;

        public DataAnnotationsValidationService(ILocalizationManager? localizationManager = null)
            => this.localizationManager = localizationManager;


        public bool IsValid(object obj) => Validator.TryValidateObject(
            obj,
            new ValidationContext(obj),
            null
        );


        public IDisposable Subscribe(IValidationViewModel viewModel)
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
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(
                obj,
                new ValidationContext(obj),
                results
            );

            return null;
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