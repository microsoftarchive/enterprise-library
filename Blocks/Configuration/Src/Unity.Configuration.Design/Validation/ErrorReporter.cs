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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design.Validation
{
    internal class ErrorReporter
    {
        private readonly Property property;
        private readonly IList<ValidationResult> errors;

        public ErrorReporter(Property property, IList<ValidationResult> errors)
        {
            this.property = property;
            this.errors = errors;
        }

        public void AddWarning(string format, params object[] args)
        {
            var message = String.Format(CultureInfo.CurrentCulture, format, args);
            errors.Add(new PropertyValidationResult(property, message, true));
        }

        public void AddError(string format, params object[] args)
        {
            var message = String.Format(CultureInfo.CurrentCulture, format, args);
            errors.Add(new PropertyValidationResult(property, message));
        }
    }
}
