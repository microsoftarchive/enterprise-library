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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class EnvironmentOverriddenElementPayload
    {
        string elementPath;
        readonly EnvironmentalOverridesSection environmentSection;
        EnvironmentOverriddenPropertyElement[] lastMergeElements;

        public EnvironmentOverriddenElementPayload(EnvironmentalOverridesSection environmentSection, string elementPath)
        {
            Guard.ArgumentNotNull(environmentSection, "environmentSection");

            this.environmentSection = environmentSection;
            this.elementPath = elementPath;
        }

        private EnvironmentalOverridesElement GetMergeElement(string path)
        {
            return
                environmentSection.MergeElements.Cast<EnvironmentalOverridesElement>().Where(
                    x => x.LogicalParentElementPath == path).FirstOrDefault();
        }

        public string ElementPath
        {
            get { return elementPath; }
        }

        public void UpdateElementPath(string newElementPath)
        {
            var mergeElement = GetMergeElement(elementPath);
            if (mergeElement != null) mergeElement.LogicalParentElementPath = newElementPath;
            elementPath = newElementPath;
        }

        public void CopyInitialValues(IEnumerable<Property> propertiesToCopyInitialValuesFrom)
        {
            var mergeElement = GetMergeElement(elementPath);
            mergeElement.OverriddenProperties.Clear();
            if (lastMergeElements != null)
            {
                foreach (EnvironmentOverriddenPropertyElement lastOverriddenPropertyValue in lastMergeElements)
                {
                    mergeElement.OverriddenProperties.Add(lastOverriddenPropertyValue);
                }
            }
            else
            {
                foreach (var property in propertiesToCopyInitialValuesFrom.Where(x => !typeof(ConfigurationElement).IsAssignableFrom(x.PropertyType)))
                {
                    SetValue(property, property.Value);
                }
            }
        }

        public object GetValue(Property originalProperty)
        {
            var overridableProperty = GetOverridableProperty(originalProperty);
            var mergeElement = GetMergeElement(elementPath);

            if (mergeElement != null)
            {
                var entry = mergeElement.OverriddenProperties.
                    Cast<EnvironmentOverriddenPropertyElement>().
                    Where(x => ConfigurationMergePropertyMatchesProperty(x, overridableProperty)).
                    FirstOrDefault();
                if (entry != null)
                {
                    return overridableProperty.DeltaConfigurationStorageConverter.ConvertTo(originalProperty, CultureInfo.InvariantCulture, entry.OverriddenValue, originalProperty.PropertyType);
                }
            }

            if (lastMergeElements != null)
            {
                var entry = lastMergeElements.
                    Where(x => ConfigurationMergePropertyMatchesProperty(x, overridableProperty)).
                    FirstOrDefault();
                if (entry != null)
                {
                    return overridableProperty.DeltaConfigurationStorageConverter.ConvertTo(originalProperty, CultureInfo.InvariantCulture, entry.OverriddenValue, originalProperty.PropertyType);
                }
            }

            return originalProperty.Value;
        }

        private EnvironmentOverriddenProtectionProviderElement GetOverriddenProtectionProviderElement(SectionViewModel section)
        {
            return environmentSection.OverriddenProtectionProviders.
                Cast<EnvironmentOverriddenProtectionProviderElement>().
                Where(x => x.ContainingSectionXPath == section.Path).
                FirstOrDefault();
        }

        public string GetProtectionProvider(SectionViewModel section, ProtectionProviderProperty originalProperty)
        {
            var overridenValue = GetOverriddenProtectionProviderElement(section);

            return (overridenValue == null) ? (string)originalProperty.Value : overridenValue.ProtectionProvider;

        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "originalProperty")]
        public void SetProtectionProvider(SectionViewModel section, ProtectionProviderProperty originalProperty, string value)
        {
            var overridenValue = GetOverriddenProtectionProviderElement(section);

            if (overridenValue != null)
            {
                overridenValue.ProtectionProvider = value;
            }
            else
            {
                environmentSection.OverriddenProtectionProviders.Add(new EnvironmentOverriddenProtectionProviderElement()
                                                                         {
                                                                             ConfigurationSectionName = section.SectionName,
                                                                             ContainingSectionXPath = section.Path,
                                                                             ProtectionProvider = value
                                                                         });
            }
        }

        public void RemoveProtectionProvider(SectionViewModel section)
        {
            var overridenValue = GetOverriddenProtectionProviderElement(section);

            if (overridenValue != null)
            {
                environmentSection.OverriddenProtectionProviders.Remove(overridenValue);
            }
        }

        public void EnsureProtectionProvider(SectionViewModel section, string value)
        {
            var overridenValue = GetOverriddenProtectionProviderElement(section);

            if (overridenValue != null)
            {
                overridenValue.ProtectionProvider = value;
            }
            else
            {
                environmentSection.OverriddenProtectionProviders.Add(new EnvironmentOverriddenProtectionProviderElement()
                                                                         {
                                                                             ConfigurationSectionName = section.SectionName,
                                                                             ContainingSectionXPath = section.Path,
                                                                             ProtectionProvider = value
                                                                         });
            }
        }

        private static IEnvironmentalOverridesProperty GetOverridableProperty(Property originalProperty)
        {
            IEnvironmentalOverridesProperty overridableProperty = originalProperty as IEnvironmentalOverridesProperty;
            if (overridableProperty == null || !overridableProperty.SupportsOverride)
            {
                throw new InvalidOperationException();
            }
            return overridableProperty;
        }

        public void SetValue(Property originalProperty, object value)
        {
            var overridableProperty = GetOverridableProperty(originalProperty);
            var mergeElement = GetMergeElement(elementPath);

            var propertyAttributeName = overridableProperty.PropertyAttributeName;
            var convertedValue = (string)overridableProperty.DeltaConfigurationStorageConverter.ConvertFrom(originalProperty, CultureInfo.InvariantCulture, value);
            var containingElementXPath = overridableProperty.ContainingElementXPath;
            var containingSectionName = overridableProperty.ConfigurationSectionName;
            var containingSectionXpath = overridableProperty.ContainingSectionXPath;

            var existingEntry = mergeElement.OverriddenProperties.
                Cast<EnvironmentOverriddenPropertyElement>().
                Where(x => ConfigurationMergePropertyMatchesProperty(x, overridableProperty)).
                FirstOrDefault();

            if (existingEntry != null)
            {
                existingEntry.OverriddenValue = convertedValue;
            }
            else
            {
                var newEntry = new EnvironmentOverriddenPropertyElement
                                   {
                                       ConfigurationSectionName = containingSectionName,
                                       ContainingElementXPath = containingElementXPath,
                                       ContainingSectionXPath = containingSectionXpath,
                                       Attribute = propertyAttributeName,
                                       OverriddenValue = convertedValue
                                   };
                mergeElement.OverriddenProperties.Add(newEntry);
            }
        }

        public bool Exists
        {
            get { return GetMergeElement(elementPath) != null; }
        }

        public void Delete()
        {
            if (Exists)
            {
                var mergeElement = GetMergeElement(elementPath);
                lastMergeElements = mergeElement.OverriddenProperties.OfType<EnvironmentOverriddenPropertyElement>().ToArray();
                environmentSection.MergeElements.Remove(mergeElement);
            }
        }

        public void Create()
        {
            if (!Exists)
            {
                environmentSection.MergeElements.Add(new EnvironmentalOverridesElement { LogicalParentElementPath = elementPath });
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool ConfigurationMergePropertyMatchesProperty(EnvironmentOverriddenPropertyElement configurationMergeProperty, IEnvironmentalOverridesProperty runtimeProperty)
        {
            return configurationMergeProperty != null && runtimeProperty != null
                   && (string.Compare(configurationMergeProperty.ConfigurationSectionName, runtimeProperty.ConfigurationSectionName, StringComparison.OrdinalIgnoreCase) == 0
                       && string.Compare(configurationMergeProperty.ContainingElementXPath, runtimeProperty.ContainingElementXPath, StringComparison.OrdinalIgnoreCase) == 0
                       && string.Compare(configurationMergeProperty.Attribute, runtimeProperty.PropertyAttributeName, StringComparison.OrdinalIgnoreCase) == 0);
        }
    }

#pragma warning restore 1591
}

