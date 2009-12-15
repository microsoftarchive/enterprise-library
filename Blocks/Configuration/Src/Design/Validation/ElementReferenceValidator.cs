using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    public class ElementReferenceValidator : PropertyValidator
    {
        protected override void ValidateCore(Property property, string value, IList<ValidationError> errors)
        {
            var referenceProperty = property as ElementReferenceProperty;
            if (referenceProperty == null) return;

            var convertedValue = property.ConvertFromBindableValue(value);

            bool isMissingRequiredReference = string.IsNullOrEmpty(convertedValue.ToString()) && property.IsRequired;

            if (isMissingRequiredReference || !property.SuggestedValues.Contains(convertedValue))
            {
                errors.Add(new
                  ValidationError(
                      property,
                      GetMissingReferenceMessage(referenceProperty),
                      true));               
            }
        }

        private string GetMissingReferenceMessage(ElementReferenceProperty referenceProperty)
        {
                                  
            if (referenceProperty.ContainingScopeElement != null)
            {
                return string.Format(
                    CultureInfo.CurrentUICulture,
                    Properties.Resources.ValidationElementReferenceMissingWithScope,
                    referenceProperty.ContainingScopeElement.Name);
            }

            return Properties.Resources.ValidationElementReferenceMissing;
        }
    }
}
