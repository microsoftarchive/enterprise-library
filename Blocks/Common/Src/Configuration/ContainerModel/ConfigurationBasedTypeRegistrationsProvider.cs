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
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// A <see cref="TypeRegistrationsProvider"/> that can be configured through a <see cref="TypeRegistrationProviderSettings"/>.
    /// </summary>
    public static class ConfigurationBasedTypeRegistrationsProviderFactory
    {
        /// <summary>
        /// Create a <see cref="ITypeRegistrationsProvider"/> that contains all the default registration
        /// providers, ignoring any configuration section.
        /// </summary>
        /// <returns>The <see cref="ITypeRegistrationsProvider"/> that will return all registrations.</returns>
        public static ITypeRegistrationsProvider CreateProvider()
        {
            var locators = CreateTypeRegistrationsProviderLocators(new DictionaryConfigurationSource());
            return new CompositeTypeRegistrationsProviderLocator(locators);
        }

        /// <summary>
        /// Create a <see cref="ITypeRegistrationsProvider"/> that contains all the default registration
        /// providers, honoring any configuration overrides of locators.
        /// </summary>
        /// <returns>The <see cref="ITypeRegistrationsProvider"/> that will return all registrations.</returns>
        public static ITypeRegistrationsProvider CreateProvider(IConfigurationSource configurationSource)
        {
            var locators = CreateTypeRegistrationsProviderLocators(configurationSource);
            return new CompositeTypeRegistrationsProviderLocator(locators);
        }

        /// <summary>
        /// public for unittesting purposes.
        /// </summary>
        /// <param name="configurationSource"></param>
        /// <returns></returns>
        public static IEnumerable<ITypeRegistrationsProvider> CreateTypeRegistrationsProviderLocators(IConfigurationSource configurationSource)
        {
            TypeRegistrationProvidersConfigurationSection section = configurationSource.GetSection(TypeRegistrationProvidersConfigurationSection.SectionName) as TypeRegistrationProvidersConfigurationSection;
            if (section == null)
            {
                section = new TypeRegistrationProvidersConfigurationSection();
            }

            foreach (TypeRegistrationProviderSettings typeRegistrationProviderSettings in section.TypeRegistrationProviders)
            {
                if (!string.IsNullOrEmpty(typeRegistrationProviderSettings.SectionName) &&
                    !string.IsNullOrEmpty(typeRegistrationProviderSettings.ProviderTypeName))
                {
                    throw new ConfigurationErrorsException(
                        string.Format("Type Registration Provider Settings '{0}' cannot declare both sectionName and providerType attributes",
                        typeRegistrationProviderSettings.Name));
                }
                if (!string.IsNullOrEmpty(typeRegistrationProviderSettings.SectionName))
                {
                    yield return new ConfigSectionLocator(typeRegistrationProviderSettings.SectionName);
                }
                else if (!string.IsNullOrEmpty(typeRegistrationProviderSettings.ProviderTypeName))
                {
                    yield return new TypeLoadingLocator(typeRegistrationProviderSettings.ProviderTypeName);
                }
            }
        }
    }
}
