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
namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
    using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

    internal class DefaultConfigurationPropertyValidator : PropertyValidator
    {
        protected override void ValidateCore(Property property, string value, IList<ValidationResult> results)
        {
            object convertedValue = null;
            if (TryGetConvertedValue(property, value, results, out convertedValue))
            {
                var validators = GetConfigurationPropertyValidators(property)
                    .Union(GetConfigurationValidators(property, convertedValue));

                foreach (var validator in validators)
                {
                    validator.Validate(property, value, results);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static bool TryGetConvertedValue(Property property, string value, IList<ValidationResult> errors, out object convertedValue)
        {
            convertedValue = null;
            try
            {
                convertedValue = property.ConvertFromBindableValue(value);
                return true;
            }
            catch (Exception ex)
            {
                errors.Add(new PropertyValidationResult(property, ex.Message));
            }

            return false;
        }

        private static IEnumerable<Validator> GetConfigurationValidators(Property property, object convertedValue)
        {
            var configurationValidators = property.Attributes
                .OfType<ConfigurationValidatorAttribute>()
                .Select(v => new ConfigurationValidatorWrappingValidator(v.ValidatorInstance, convertedValue));

            return configurationValidators.Cast<Validator>();
        }

        private IEnumerable<Validator> GetConfigurationPropertyValidators(Property property)
        {
            var configurationPropertyAttribute = property.Attributes.OfType<ConfigurationPropertyAttribute>().FirstOrDefault();

            if (configurationPropertyAttribute == null) yield break;

            if (configurationPropertyAttribute.IsRequired)
            {
                yield return new RequiredFieldValidator();
            }
        }

        private class ConfigurationValidatorWrappingValidator : Validator
        {
            private ConfigurationValidatorBase validatorInstance;
            private object convertedValue;

            public ConfigurationValidatorWrappingValidator(ConfigurationValidatorBase validatorInstance, object convertedValue)
            {
                this.validatorInstance = validatorInstance;
                this.convertedValue = convertedValue;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
            protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
            {
                var property = instance as ElementProperty;
                if (property == null) return;

                if (this.validatorInstance.CanValidate(property.PropertyType))
                {
                    try
                    {
                        validatorInstance.Validate(this.convertedValue);
                    }
                    catch (Exception ex)
                    {
                        results.Add(new PropertyValidationResult(property, ex.Message));
                    }
                }
            }
        }
    }
}
