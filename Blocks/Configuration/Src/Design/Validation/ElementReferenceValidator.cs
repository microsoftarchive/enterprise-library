//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// A <see cref="PropertyValidator"/> class that is used to validate <see cref="ElementReferenceProperty"/> values.
    /// </summary>
    /// <seealso cref="ElementReferenceProperty"/>
    public class ElementReferenceValidator : PropertyValidator
    {
        /// <summary>
        /// Validates a <see cref="ElementReferenceProperty"/>.
        /// </summary>
        /// <remarks>
        /// If the <paramref name="property"/> is required and <paramref name="value"/> is empty, the method reports a validation error.<br/>
        /// If the value cannot be resolved, the method reports a validation warning.<br/>
        /// </remarks>
        /// <param name="property">The <see cref="ElementReferenceProperty"/> that should be validated.</param>
        /// <param name="value">The value used for validation.</param>
        /// <param name="results">The collection to wich any results that occur during the validation can be added.</param>		
        protected override void ValidateCore(Property property, string value, IList<ValidationResult> results)
        {
            var referenceProperty = property as ElementReferenceProperty;
            if (referenceProperty == null) return;

            var convertedValue = property.ConvertFromBindableValue(value) as string;

            bool isMissingRequiredReference = string.IsNullOrEmpty(convertedValue) && property.IsRequired;

            if (isMissingRequiredReference || !property.SuggestedValues.Contains(convertedValue))
            {
                results.Add(new
                  PropertyValidationResult(
                      property,
                      GetMissingReferenceMessage(referenceProperty),
                      true));               
            }
        }

        private static string GetMissingReferenceMessage(ElementReferenceProperty referenceProperty)
        {
            if (referenceProperty.ContainingScopeElement != null)
            {
                return string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resources.ValidationElementReferenceMissingWithScope,
                    referenceProperty.ContainingScopeElement.Name);
            }

            return Properties.Resources.ValidationElementReferenceMissing;
        }
    }
}
