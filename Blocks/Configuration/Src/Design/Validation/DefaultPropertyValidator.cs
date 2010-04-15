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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    internal class DefaultPropertyValidator : PropertyValidator
    {
        protected override void ValidateCore(Property property, string value, IList<ValidationResult> results)
        {
            object convertedValue = null;
            if (TryGetConvertedValue(property, value, results, out convertedValue))
            {
                var validators = GetConfigurationPropertyValidators(property)
                    .Union(GetConfigurationValidators(property));

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

        private static IEnumerable<Validator> GetConfigurationValidators(Property property)
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
            protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
            {
                var property = instance as ElementProperty;
                if (property == null) return;

                if (!IsKeyItem(property)) return;

                var containingElement = property.DeclaringElement as CollectionElementViewModel;
                if (containingElement == null) return;

                var parentCollection = containingElement.ParentElement as ElementCollectionViewModel;

                var newItemComparer = CreateMatchKeyPredicate(containingElement, property);

                if ((typeof(KeyValueConfigurationElement) == containingElement.ConfigurationElement.GetType()))
                {
                    
                    int itemCount = 0;
                    foreach (var element in parentCollection.ChildElements)
                    {
                        if (string.Compare(value, (string)element.NameProperty.Value, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            itemCount++;
                        }
                    }

                    if (itemCount > 1)
                    {
                        results.Add(new PropertyValidationResult(property, Resources.ValidationErrorDuplicateKeyValue));
                        return;
                    }
                }
                else
                {

                    foreach (var element in parentCollection.ChildElements.OfType<CollectionElementViewModel>())
                    {
                        if (element.ElementId == containingElement.ElementId) { continue; }

                        if (newItemComparer(element))
                        {
                            results.Add(new PropertyValidationResult(property, Resources.ValidationErrorDuplicateKeyValue));
                            return;
                        }
                    }
                }
            }

            private static bool IsKeyItem(ElementProperty property)
            {
                var configPropertyAttribute = property.Attributes.OfType<ConfigurationPropertyAttribute>().FirstOrDefault();
                return (configPropertyAttribute != null && configPropertyAttribute.IsKey == true);
            }

            private static Func<ElementViewModel, bool> CreateMatchKeyPredicate(ElementViewModel elementBeingValidated, Property propertyBeingValidated)
            {
                string[] keyPropertyNames = elementBeingValidated.ConfigurationElement.
                                            ElementInformation.Properties.
                                            Cast<PropertyInformation>().
                                            Where(x => x.IsKey).
                                            Select(x => x.Name).ToArray();

                return otherElementInKeyComparison =>
                {
                    foreach (string keyProperty in keyPropertyNames)
                    {
                        BindableProperty otherElementInKeyComparisonBindableProperty = otherElementInKeyComparison.Properties.OfType<ElementProperty>().Where(y => y.ConfigurationName == keyProperty).Single().BindableProperty;
                        BindableProperty propertyOnElementBeingValidatedBindable = elementBeingValidated.Properties.OfType<ElementProperty>().Where(y=>y.ConfigurationName == keyProperty).Single().BindableProperty;
                        if (!otherElementInKeyComparisonBindableProperty.IsBindableValueCommitted)
                        {
                            return false;
                        }
                        if (propertyOnElementBeingValidatedBindable.Property != propertyBeingValidated && !propertyOnElementBeingValidatedBindable.IsBindableValueCommitted)
                        {
                            return false;
                        }
                        if (!string.Equals(otherElementInKeyComparisonBindableProperty.BindableValue, propertyOnElementBeingValidatedBindable.BindableValue, StringComparison.OrdinalIgnoreCase))
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

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
            protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
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
                        results.Add(new PropertyValidationResult(property, ex.Message));
                    }
                }
            }
        }
    }
}
