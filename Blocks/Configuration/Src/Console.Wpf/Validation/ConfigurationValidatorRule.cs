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
using System.Configuration;

namespace Console.Wpf.Validation
{
    public class ConfigurationValidatorRule : ValidationRule
    {
        readonly ConfigurationValidatorBase validator;

        public ConfigurationValidatorRule(ConfigurationValidatorBase validator)
        {
            this.validator = validator;
            
            ValidationStep = ValidationStep.ConvertedProposedValue;
        }

        public ConfigurationValidatorBase ConfigurationValidator
        {
            get { return validator; }
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (validator.CanValidate(value.GetType()))
            {
                try
                {
                    validator.Validate(value);
                }
                catch(ArgumentException e)
                {
                    return new ValidationResult(false, e.Message);
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
