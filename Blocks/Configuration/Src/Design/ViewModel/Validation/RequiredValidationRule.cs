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
using System.Windows.Controls;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Console.Wpf.Validation
{
    public class RequiredValidationRule : ValidationRule
    {
        public RequiredValidationRule()
        {
            ValidationStep = ValidationStep.ConvertedProposedValue;
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string valueString = value as String;

            if (valueString != null)
            {
                if (string.IsNullOrEmpty(valueString))
                {
                    return new ValidationResult(false, "Value can not be an empty string.");
                }
            }
            else if (!(value is ValueType))
            {
                if (value == null)
                {
                    return new ValidationResult(false, "Value can not be empty.");
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
