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
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class LogPriorityMinMaxValidator : Validator
    {
        protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
        {
            var logPriorityFilter = instance as ElementViewModel;
            if (logPriorityFilter == null) return;

            var minimumProperty = logPriorityFilter.Property("MinimumPriority");
            var maximumProperty = logPriorityFilter.Property("MaximumPriority");

            if (minimumProperty == null || minimumProperty.BindableProperty.BindableValue == null) return;
            if (maximumProperty == null || minimumProperty.BindableProperty.BindableValue == null) return;

            int maximum, minimum;

            // bypass invalid int values, do not log error because the range validator will log it
            // and if we also log it we'll get a duplicate message

            if (!int.TryParse(maximumProperty.BindableProperty.BindableValue, out maximum)
                || !int.TryParse(minimumProperty.BindableProperty.BindableValue, out minimum))
            {
                return;
            }

            if (minimum >= maximum)
            {
                results.Add(
                    new ElementValidationResult(logPriorityFilter,
                                                Resources.ValidationLoggingPriorityValuesInvalid,
                                                false
                        ));
            }
        }
    }
#pragma warning restore 1591
}
