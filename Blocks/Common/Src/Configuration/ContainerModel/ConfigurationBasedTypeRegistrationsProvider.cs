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
    /// A <see cref="TypeRegistrationsProvider"/> that can be configured through a <see cref="TypeRegistrationProviderElement"/>.
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
            return CreateProvider(new DictionaryConfigurationSource(), new NullContainerReconfiguringEventSource());
        }

        ///// <summary>
        ///// Create a <see cref="ITypeRegistrationsProvider"/> that contains all the default registration
        ///// providers, honoring any configuration overrides of locators.
        ///// </summary>
        ///// <returns>The <see cref="ITypeRegistrationsProvider"/> that will return all registrations.</returns>
        //public static ITypeRegistrationsProvider CreateProvider(IConfigurationSource configurationSource)
        //{
        //    return CreateProvider(configurationSource, new NullContainerReconfiguringEventSource());
        //}

        /// <summary>
        /// Create a <see cref="ITypeRegistrationsProvider"/> that contains all the default registration
        /// providers, honoring any configuration overrides of locators.
        /// </summary>
        /// <param name="configurationSource">The configuration source to use when creating <see cref="TypeRegistrationsProvider"/>s</param>
        /// <param name="reconfiguringEventSource">The <see cref="IContainerReconfiguringEventSource"/> responsible for raising container reconfiguration events.</param>
        /// <returns>The <see cref="ITypeRegistrationsProvider"/> that will return all registrations.</returns>
        public static ITypeRegistrationsProvider CreateProvider(IConfigurationSource configurationSource, IContainerReconfiguringEventSource reconfiguringEventSource)
        {
            var locators = CreateTypeRegistrationsProviderLocators(configurationSource, reconfiguringEventSource);
            return new CompositeTypeRegistrationsProviderLocator(locators);
        }

        /// <summary>
        /// public for unittesting purposes.
        /// </summary>
        /// <param name="configurationSource"></param>
        /// <param name="reconfiguringEventSource"></param>
        /// <returns></returns>
        public static IEnumerable<ITypeRegistrationsProvider> CreateTypeRegistrationsProviderLocators(IConfigurationSource configurationSource, IContainerReconfiguringEventSource reconfiguringEventSource)
        {
            TypeRegistrationProvidersConfigurationSection section = configurationSource.GetSection(TypeRegistrationProvidersConfigurationSection.SectionName) as TypeRegistrationProvidersConfigurationSection;
            if (section == null)
            {
                section = new TypeRegistrationProvidersConfigurationSection();
            }

            foreach (TypeRegistrationProviderElement typeRegistrationProviderElement in section.TypeRegistrationProviders)
            {
                if (!string.IsNullOrEmpty(typeRegistrationProviderElement.SectionName) &&
                    !string.IsNullOrEmpty(typeRegistrationProviderElement.ProviderTypeName))
                {
                    throw new ConfigurationErrorsException(
                        string.Format("Type Registration Provider Settings '{0}' cannot declare both sectionName and providerType attributes",
                        typeRegistrationProviderElement.Name));
                }
                if (!string.IsNullOrEmpty(typeRegistrationProviderElement.SectionName))
                {
                    yield return new ConfigSectionLocator(typeRegistrationProviderElement.SectionName, reconfiguringEventSource);
                }
                else if (!string.IsNullOrEmpty(typeRegistrationProviderElement.ProviderTypeName))
                {
                    yield return new TypeLoadingLocator(typeRegistrationProviderElement.ProviderTypeName, reconfiguringEventSource);
                }
            }
        }
    }
}
