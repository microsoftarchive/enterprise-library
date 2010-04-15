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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigurationSourceBuilder"/> extensions to support creation of type registration provider settings.
    /// </summary>
    public static class TypeRegistrationProviderConfigurationSourceBuilderExtension
    {
        /// <summary>
        /// Main entry point to configure a <see cref="TypeRegistrationProvidersConfigurationSection"/> section.
        /// </summary>
        /// <param name="configurationSourceBuilder">The builder interface to extend.</param>
        /// <returns>A fluent interface that allows to add <see cref="ITypeRegistrationsProvider"/> instances.</returns>
        /// <see cref="ITypeRegistrationsProvider"/>
        /// <see cref="TypeRegistrationProviderElement"/>
        public static IConfigureTypeRegistrations ConfigureTypeRegistrations(this IConfigurationSourceBuilder configurationSourceBuilder)
        {
            return new ConfigureTypeRegistrationBuilder(configurationSourceBuilder, false);
        }

        /// <summary>
        /// Main entry point to configure a <see cref="TypeRegistrationProvidersConfigurationSection"/> section with no 
        /// default type registrations.
        /// </summary>
        /// <param name="configurationSourceBuilder">The builder interface to extend.</param>
        /// <returns>A fluent interface that allows to add <see cref="ITypeRegistrationsProvider"/> instances.</returns>
        /// <see cref="ITypeRegistrationsProvider"/>
        /// <see cref="TypeRegistrationProviderElement"/>
        public static IConfigureTypeRegistrations ConfigureEmptyTypeRegistrations(this IConfigurationSourceBuilder configurationSourceBuilder)
        {
            return new ConfigureTypeRegistrationBuilder(configurationSourceBuilder, true);
        }

        private class ConfigureTypeRegistrationBuilder : IConfigureTypeRegistrations, IConfigureTypeRegistration
        {
            TypeRegistrationProvidersConfigurationSection typeRegistrationsConfigurationSection;
            TypeRegistrationProviderElement currentTypeRegistration;

            public ConfigureTypeRegistrationBuilder(IConfigurationSourceBuilder configurationSourceBuilder, bool empty)
            {
                typeRegistrationsConfigurationSection = new TypeRegistrationProvidersConfigurationSection();
                if (empty)
                {
                    typeRegistrationsConfigurationSection.TypeRegistrationProviders.Clear();
                }

                configurationSourceBuilder.AddSection(TypeRegistrationProvidersConfigurationSection.SectionName, typeRegistrationsConfigurationSection);

            }

            public IConfigureTypeRegistration AddTypeRegistrationsProviderNamed(string typeRegistrationProviderName)
            {
                if (string.IsNullOrEmpty(typeRegistrationProviderName))
                    throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "typeRegistrationProviderName");

                currentTypeRegistration = new TypeRegistrationProviderElement
                {
                    Name = typeRegistrationProviderName
                };

                typeRegistrationsConfigurationSection.TypeRegistrationProviders.Add(currentTypeRegistration);

                return this;
            }

            public IConfigureTypeRegistrations ForType<TTypeRegistrationsProvider>() where TTypeRegistrationsProvider : ITypeRegistrationsProvider
            {
                return ForType(typeof(TTypeRegistrationsProvider));
            }

            public IConfigureTypeRegistrations ForType(Type typeRegistrationsProviderType)
            {
                if (typeRegistrationsProviderType == null) throw new ArgumentNullException("typeRegistrationsProviderType");

                if (!typeof(ITypeRegistrationsProvider).IsAssignableFrom(typeRegistrationsProviderType))
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        Resources.ExceptionTypeMustImplementInterface, typeof(ITypeRegistrationsProvider)), "typeRegistrationsProviderType");

                currentTypeRegistration.ProviderTypeName = typeRegistrationsProviderType.AssemblyQualifiedName;

                return this;
            }

            public IConfigureTypeRegistrations ForSection(string sectionName)
            {
                currentTypeRegistration.SectionName = sectionName;

                return this;
            }
        }
    }

}
