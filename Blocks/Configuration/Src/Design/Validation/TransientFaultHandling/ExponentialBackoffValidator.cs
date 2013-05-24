#region license
//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Application Block Library
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
#endregion
namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.BlockSpecifics
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
    using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
    using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

    /// <summary>
    /// A validator for exponential backoff bounds.
    /// </summary>
    public class ExponentialBackoffValidator : Validator
    {
        /// <summary>
        /// Validates whether <paramref name="value"/> is in the specified range.
        /// </summary>
        /// <param name="instance">The instance to validate; this is expected to be a <see cref="Property"/>.</param>
        /// <param name="value">The value to validate.</param>
        /// <param name="results">The collection to add any results that occur during the validation.</param>
        protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
        {
            var element = instance as ElementViewModel;

            if (element == null)
            {
                return;
            }

            var minBackoff = (TimeSpan)element.Property("MinBackoff").Value;
            var maxBackoff = (TimeSpan)element.Property("MaxBackoff").Value;

            if (minBackoff >= maxBackoff)
            {
                results.Add(new ElementValidationResult(element, Resources.MaxBackoffMustBeGreaterThanMinBackoff));
            }
        }
    }
}
