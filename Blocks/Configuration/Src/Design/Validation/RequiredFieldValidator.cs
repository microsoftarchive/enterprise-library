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
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    ///<summary>
    /// A <see cref="PropertyValidator" /> class that validates whether a required value is not empty.
    ///</summary>
    public class RequiredFieldValidator : PropertyValidator
    {
        /// <summary>
        /// Validates whether <paramref name="value"/> is not <see langword="null"/> or an empty string.
        /// </summary>
        /// <param name="property">The Property that declares the <paramref name="value"/>.</param>
        /// <param name="value">Value to validate</param>
        /// <param name="results">The collection to wich any results that occur during the validation can be added.</param>		
        protected override void ValidateCore(Property property, string value, IList<ValidationResult> results)
        {
            if (string.IsNullOrEmpty(value))
            {
                results.Add(new PropertyValidationResult(property, string.Format(CultureInfo.CurrentCulture, Resources.ValidationRequiredPropertyValueMissing, property.DisplayName)));
            }
        }
    }
}
