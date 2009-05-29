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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Contains a collection of <see cref="TypeRegistrationProviderSettings"/>.
    /// </summary>
    [ConfigurationElementType(typeof(TypeRegistrationProviderSettings))]
    public class TypeRegistrationProviderSettingsCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new instance of <see cref="TypeRegistrationProviderSettingsCollection"/>.
        /// </summary>
        public TypeRegistrationProviderSettingsCollection()
        {
            BaseAdd(new TypeRegistrationProviderSettings() { Name = TypeRegistrationProvidersConfigurationSection.CachingTypeRegistrationProviderName, SectionName = BlockSectionNames.Caching });
            BaseAdd(new TypeRegistrationProviderSettings() { Name = TypeRegistrationProvidersConfigurationSection.CryptographyTypeRegistrationProviderName, SectionName = BlockSectionNames.Cryptography });
            BaseAdd(new TypeRegistrationProviderSettings() { Name = TypeRegistrationProvidersConfigurationSection.ExceptionHandlingTypeRegistrationProviderName, SectionName = BlockSectionNames.ExceptionHandling });
            BaseAdd(new TypeRegistrationProviderSettings() { Name = TypeRegistrationProvidersConfigurationSection.InstrumentationTypeRegistrationProviderName, SectionName = BlockSectionNames.Instrumentation });
            BaseAdd(new TypeRegistrationProviderSettings() { Name = TypeRegistrationProvidersConfigurationSection.LoggingTypeRegistrationProviderName, SectionName = BlockSectionNames.Logging });
            BaseAdd(new TypeRegistrationProviderSettings() { Name = TypeRegistrationProvidersConfigurationSection.PolicyInjectionTypeRegistrationProviderName, SectionName = BlockSectionNames.PolicyInjection });
            BaseAdd(new TypeRegistrationProviderSettings() { Name = TypeRegistrationProvidersConfigurationSection.SecurityTypeRegistrationProviderName, SectionName = BlockSectionNames.Security });
            BaseAdd(new TypeRegistrationProviderSettings() { Name = TypeRegistrationProvidersConfigurationSection.DataAccessTypeRegistrationProviderName, ProviderTypeName = BlockSectionNames.DataRegistrationProviderLocatorType });
            BaseAdd(new TypeRegistrationProviderSettings() { Name = TypeRegistrationProvidersConfigurationSection.ValidationTypeRegistrationProviderName,  ProviderTypeName = BlockSectionNames.ValidationRegistrationProviderLocatorType });
        }

        /// <value><see langword="true"/></value>
        protected override bool ThrowOnDuplicate
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Returns a new <see cref="TypeRegistrationProviderSettings"/> instance.
        /// </summary>
        /// <returns>A new <see cref="TypeRegistrationProviderSettings"/> instance.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new TypeRegistrationProviderSettings();
        }

        /// <summary>
        /// Public for unit testing purposes.
        /// </summary>
        public void Remove(string elementName)
        {
            base.BaseRemove(elementName);
        }

        /// <summary>
        /// Public for unit testing purposes.
        /// </summary>
        public void Clear()
        {
            base.BaseClear();
        }

        /// <summary>
        /// Public for unit testing purposes.
        /// </summary>
        public void Add(TypeRegistrationProviderSettings element)
        {
            base.BaseAdd(element);
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="TypeRegistrationProviderSettings"/> to return the key for.</param>
        /// <returns>An Object that acts as the key for the specified <see cref="TypeRegistrationProviderSettings"/>. </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            TypeRegistrationProviderSettings typeRegistrationProviderSettings = element as TypeRegistrationProviderSettings;
            if (typeRegistrationProviderSettings != null)
            {
                return typeRegistrationProviderSettings.Name;
            }
            throw new InvalidOperationException();
        }
    }
}
