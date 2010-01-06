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
    public class TypeValidator : PropertyValidator
    {
        protected override void ValidateCore(ViewModel.Property property, string value, IList<ValidationError> errors)
        {
            if (string.IsNullOrEmpty(value)) return;

            Type type = Type.GetType(value, false, true);
            if (type == null)
            {
                errors.Add(new ValidationError(property, string.Format(CultureInfo.CurrentUICulture, Resources.ValidationTypeNotLocatable, value), true));
            }
        }
    }
}
