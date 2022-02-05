using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Shiny.Impl
{
    public class DataAnnotationsValidationService : IValidationService
    {
        public bool IsValid(object obj)
        {
            //Validator.TryValidateObject(obj)
            var vc = new ValidationContext(obj);
            
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IValidationViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, string> Validate(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
//protected virtual string OnValidate(string propertyName)
//{
//    if (string.IsNullOrEmpty(propertyName))
//    {
//        throw new ArgumentException("Invalid property name", propertyName);
//    }

//    string error = string.Empty;
//    var value = GetValue(propertyName);
//    var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>(1);
//    var result = Validator.TryValidateProperty(
//        value,
//        new ValidationContext(this, null, null)
//        {
//            MemberName = propertyName
//        },
//        results);

//    if (!result)
//    {
//        var validationResult = results.First();
//        error = validationResult.ErrorMessage;
//    }

//    return error;
//}