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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Logging
{
    public class LogPriorityMinMaxValidator : PropertyValidator
    {
        protected override void ValidateCore(Property property, string value, IList<ValidationError> errors)
        {
            var elementProperty = property as ElementProperty;
            if (elementProperty == null) return;

            var logPriorityFilter = elementProperty.DeclaringElement;

            var minimumProperty = logPriorityFilter.Property("MinimumPriority");
            var maximumProperty = logPriorityFilter.Property("MaximumPriority");

            if (minimumProperty == null || minimumProperty.BindableProperty.BindableValue == null) return;
            if (maximumProperty == null || minimumProperty.BindableProperty.BindableValue == null) return;

            if (int.Parse(minimumProperty.BindableProperty.BindableValue) >= int.Parse(maximumProperty.BindableProperty.BindableValue))
            {
                errors.Add(
                    new ValidationError(property,
                        Resources.ValidationLoggingPriorityValuesInvalid,
                        false
                        ));
            }
        }
    }
}
