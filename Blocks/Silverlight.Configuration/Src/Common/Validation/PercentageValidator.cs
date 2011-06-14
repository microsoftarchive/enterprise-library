//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Validation
{
    /// <summary>
    /// A <see cref="PercentageValidator"/> instance that validates whether the value is between 0 and 100.
    /// </summary>
    public class PercentageValidator : Validator
    {
        /// <summary>
        /// Validates whether <paramref name="value"/> is in the specified range
        /// </summary>
        /// <param name="instance">The instance to validate, this is expected to be a <see cref="Property"/></param>
        /// <param name="value">The value that should be validated.</param>
        /// <param name="results">The collection to add any results that occur during the validation.</param>		
        protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
        {
            var property = instance as Property;

            if (property == null) return;

            int numericValue;

            if (int.TryParse(value, out numericValue))
            {
                if (numericValue < 0 || numericValue > 100)
                {
                    results.Add(new PropertyValidationResult(property, Resources.ValidationPercentage, false));
                }
            }
        }

    }
}
