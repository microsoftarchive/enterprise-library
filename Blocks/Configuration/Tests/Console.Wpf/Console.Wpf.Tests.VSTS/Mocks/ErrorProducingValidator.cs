using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    public class ErrorProducingValidator : Validator
    {
        protected override void ValidateCore(object instance, string value, IList<ValidationError> errors)
        {
            var property = instance as ElementProperty;
            if (property == null)
            {
                throw new ArgumentException("Property was not ElementProperty");
            }

            errors.Add(new ValidationError(property, "Test Validation Error"));
        }
    }
}