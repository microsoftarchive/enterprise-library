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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{    
    ///<summary>
    /// A <see cref="PropertyValidator" /> class that validates whether a <see cref="Type"/> can be resolved from the value.
    ///</summary>
    public class TypeValidator : PropertyValidator
    {
        /// <summary>
        /// Validates whether <paramref name="value"/> is can be used to resolve an instance of <see cref="Type"/>.
        /// </summary>
        /// <remarks>
        /// The <paramref name="value"/> is interpreted from within the designers application domain.<br/>
        /// Therefore results cannot be guaranteed to be accurate and warnings are reported, rather than errors.
        /// </remarks>
        /// <param name="property">The Property that declares the <paramref name="value"/>.</param>
        /// <param name="value">Value to validate</param>
        /// <param name="results">The collection to wich any results that occur during the validation can be added.</param>		
        protected override void ValidateCore(ViewModel.Property property, string value, IList<ValidationResult> results)
        {
            if (string.IsNullOrEmpty(value)) return;

            Type type = Type.GetType(value, false, true);
            if (type == null)
            {
                results.Add(new PropertyValidationResult(property, string.Format(CultureInfo.CurrentCulture, Resources.ValidationTypeNotLocatable, value), true));
            }
        }
    }
}
