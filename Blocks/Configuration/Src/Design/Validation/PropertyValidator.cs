using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    public abstract class PropertyValidator : Validator
    {
        protected override void ValidateCore(object instance, string value, IList<ValidationError> errors)
        {
            var property = instance as Property;
            if (property == null) return;

            if (property.ReadOnly) return;

            ValidateCore(property, value, errors);
        }

        protected abstract void ValidateCore(Property property, string value, IList<ValidationError> errors);
    }
}