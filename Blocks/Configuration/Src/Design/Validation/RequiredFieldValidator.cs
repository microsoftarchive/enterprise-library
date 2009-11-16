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
    public class RequiredFieldValidator : ValidationAttribute
    {
        protected override void ValidateCore(object instance, IList<ValidationError> errors)
        {
            var property = instance as ElementProperty;

            if (property == null) return;

            bool isMissing = (property.BindableValue == null) ||
                             (property.PropertyType == typeof (string) && string.IsNullOrEmpty((string) property.Value));

            if (isMissing)
            {
                errors.Add(property.ValidationError(string.Format(Resources.ValidationRequiredPropertyValueMissing, property.DisplayName)));
            }
        }
    }
}
