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
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    internal class DefaultPropertyValidator : PropertyValidator
    {
        protected override void ValidateCore(Property property, string value, IList<ValidationError> errors)
        {
            object convertedValue = null;
            if (TryGetConvertedValue(property, value, errors, out convertedValue))
            {
                var validators = GetConfigurationPropertyValidators(property)
                    .Union(GetConfigurationValidators(property));

                foreach (var validator in validators)
                {
                    validator.Validate(property, value, errors);
                }
            }
        }

        private static bool TryGetConvertedValue(Property property, string value, IList<ValidationError> errors, out object convertedValue)
        {
            convertedValue = null;
            try
            {
                convertedValue = property.ConvertFromBindableValue(value);
                return true;
            }
            catch (Exception ex)
            {
                errors.Add(new ValidationError(property, ex.Message));
            }

            return false;
        }

        private IEnumerable<Validator> GetConfigurationValidators(Property property)
        {
            var configurationValidators = property.Attributes
                .OfType<ConfigurationValidatorAttribute>()
                .Select(v => new ConfigurationValidatorWrappingValidator(v.ValidatorInstance));

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

            if (configurationPropertyAttribute.IsKey)
            {
                yield return new UniqueCollectionElementValidator();
            }
        }

        private class UniqueCollectionElementValidator : Validator
        {
            protected override void ValidateCore(object instance, string value, IList<ValidationError> errors)
            {
                var property = instance as ElementProperty;
                if (property == null) return;

                if (!IsKeyItem(property)) return;

                var containingElement = property.DeclaringElement as CollectionElementViewModel;
                if (containingElement == null) return;

                var parentCollection = containingElement.ParentElement as ElementCollectionViewModel;

                var newItemComparer = CreateMatchKeyPredicate(property, containingElement.ConfigurationElement);

                var collection = parentCollection.ConfigurationElement as ConfigurationElementCollection;

                foreach (ConfigurationElement element in collection)
                {
                    if (element == containingElement.ConfigurationElement) continue;

                    if (newItemComparer(element))
                    {
                        errors.Add(new ValidationError(property, "Duplicate key value."));
                        return;
                    }
                }
            }

            private bool IsKeyItem(ElementProperty property)
            {
                var configPropertyAttribute = property.Attributes.OfType<ConfigurationPropertyAttribute>().FirstOrDefault();
                return (configPropertyAttribute != null && configPropertyAttribute.IsKey == true);
            }

            private Func<ConfigurationElement, bool> CreateMatchKeyPredicate(ElementProperty property, ConfigurationElement other)
            {
                string[] keyPropertyNames = other.ElementInformation.Properties.Cast<PropertyInformation>().Where(x => x.IsKey).Select(x => x.Name).ToArray();

                return x =>
                {
                    int keyCount = other.ElementInformation.Properties.Keys.Count;
                    foreach (string keyProperty in keyPropertyNames)
                    {
                        object originalValue;
                        originalValue = keyProperty == property.ConfigurationName ? 
                                property.ConvertFromBindableValue(property.BindableProperty.BindableValue) 
                                : other.ElementInformation.Properties[keyProperty].Value;

                        if (!object.Equals(originalValue, x.ElementInformation.Properties[keyProperty].Value))
                        {
                            return false;
                        }
                    }
                    return true;
                };
            }
        }

        private class ConfigurationValidatorWrappingValidator : Validator
        {
            private ConfigurationValidatorBase validatorInstance;

            public ConfigurationValidatorWrappingValidator(ConfigurationValidatorBase validatorInstance)
            {
                this.validatorInstance = validatorInstance;
            }

            protected override void ValidateCore(object instance, string value, IList<ValidationError> errors)
            {
                var property = instance as ElementProperty;
                if (property == null) return;

                if (validatorInstance.CanValidate(property.PropertyType))
                {
                    try
                    {
                        validatorInstance.Validate(value);
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new ValidationError(property, ex.Message));
                    }
                }
            }
        }
    }
}
