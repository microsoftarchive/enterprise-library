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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    ///<summary>
    /// Validates that a <see cref="Property.Value" /> has a required value.
    ///</summary>
    public class RequiredFieldValidator : PropertyValidator
    {
        protected override void ValidateCore(Property property, string value, IList<ValidationError> errors)
        {
            if (string.IsNullOrEmpty(value))
            {
                errors.Add(new ValidationError(property, string.Format(Resources.ValidationRequiredPropertyValueMissing, property.DisplayName)));
            }
        }
    }
}
