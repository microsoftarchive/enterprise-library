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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    internal class DefaultPropertyValidator : ValidationAttribute
    {
        protected override void ValidateCore(object instance, IList<ValidationError> errors)
        {
            var property = instance as ElementProperty;
            if (property == null) return;

            object convertedValue = null;
            if (TryGetConvertedValue(property, errors, out convertedValue))
            {
                var validators = GetConfigurationPropertyValidators(property)
                    .Union(GetConfigurationValidators(property));

                foreach (var validator in validators)
                {
                    validator.Validate(property, errors);
                }
            }
        }

        private bool TryGetConvertedValue(ElementProperty property, IList<ValidationError> errors, out object convertedValue)
        {
            convertedValue = null;
            try
            {
                convertedValue = property.ConvertFromBindableValue(property.BindableValue);
                return true;
            }
            catch (Exception ex)
            {
                errors.Add(property.ValidationError(ex.Message));
            }

            return false;
        }

        private IEnumerable<ValidationAttribute> GetConfigurationValidators(Property property)
        {
            var configurationValidators = property.Attributes
                .OfType<ConfigurationValidatorAttribute>()
                .Select(v => new ConfigurationValidatorWrappingValidator(v.ValidatorInstance));

            return configurationValidators.Cast<ValidationAttribute>();
        }

        private IEnumerable<ValidationAttribute> GetConfigurationPropertyValidators(Property property)
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

        private class UniqueCollectionElementValidator : ValidationAttribute
        {
            protected override void ValidateCore(object instance, IList<ValidationError> errors)
            {
                var property = instance as ElementProperty;
                if (property == null) return;

                if (!IsKeyItem(property)) return;

                var containingElement = property.DeclaringElement as CollectionElementViewModel;
                if (containingElement == null) return;

                var parentCollection = containingElement.ParentElement as ElementCollectionViewModel;

                string newItemKey = GetElementKey(containingElement.ConfigurationElement);
                var collection = parentCollection.ConfigurationElement as ConfigurationElementCollection;

                foreach (ConfigurationElement element in collection)
                {
                    if (element == containingElement.ConfigurationElement) return;

                    if (string.Equals(newItemKey, GetElementKey(element), StringComparison.InvariantCulture))
                    {
                        errors.Add(property.ValidationError("Duplicate key value."));
                        return;
                    }
                }
            }

            private bool IsKeyItem(ElementProperty property)
            {
                var configPropertyAttribute = property.Attributes.OfType<ConfigurationPropertyAttribute>().FirstOrDefault();
                return (configPropertyAttribute != null && configPropertyAttribute.IsKey == true);
            }

            private string GetElementKey(ConfigurationElement configurationElement)
            {
                return string.Join(configurationElement.GetType().AssemblyQualifiedName,
                                   configurationElement.ElementInformation
                                       .Properties
                                       .OfType<PropertyInformation>()
                                       .OrderBy(x => x.Name)
                                       .Where(x => x.IsKey)
                                       .Select(x => x.Name + x.Converter.ConvertToInvariantString(x.Value))
                                       .ToArray());
            }
        }

        private class ConfigurationValidatorWrappingValidator : ValidationAttribute
        {
            private ConfigurationValidatorBase validatorInstance;

            public ConfigurationValidatorWrappingValidator(ConfigurationValidatorBase validatorInstance)
            {
                this.validatorInstance = validatorInstance;
            }

            protected override void ValidateCore(object instance, IList<ValidationError> errors)
            {
                var property = instance as ElementProperty;
                if (property == null) return;

                if (validatorInstance.CanValidate(property.PropertyType))
                {
                    try
                    {
                        validatorInstance.Validate(property.BindableValue);
                    }
                    catch (Exception ex)
                    {
                        errors.Add(property.ValidationError(ex.Message));
                    }
                }
            }
        }
    }
}
