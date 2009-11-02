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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Contains a collection of <see cref="TypeRegistrationProviderElement"/>.
    /// </summary>
    [ConfigurationElementType(typeof(TypeRegistrationProviderElement))]
    [ResourceDescription(typeof(DesignResources), "TypeRegistrationProviderElementCollectionDescription")]
    [ResourceDisplayName(typeof(DesignResources), "TypeRegistrationProviderElementCollectionDisplayName")]
    public class TypeRegistrationProviderElementCollection : NamedElementCollection<TypeRegistrationProviderElement>
    {
        /// <summary>
        /// Creates a new instance of <see cref="TypeRegistrationProviderElementCollection"/>.
        /// </summary>
        public TypeRegistrationProviderElementCollection()
        {
            BaseAdd(new TypeRegistrationProviderElement() { Name = TypeRegistrationProvidersConfigurationSection.CachingTypeRegistrationProviderName, SectionName = BlockSectionNames.Caching });
            BaseAdd(new TypeRegistrationProviderElement() { Name = TypeRegistrationProvidersConfigurationSection.CryptographyTypeRegistrationProviderName, SectionName = BlockSectionNames.Cryptography });
            BaseAdd(new TypeRegistrationProviderElement() { Name = TypeRegistrationProvidersConfigurationSection.ExceptionHandlingTypeRegistrationProviderName, SectionName = BlockSectionNames.ExceptionHandling });
            BaseAdd(new TypeRegistrationProviderElement() { Name = TypeRegistrationProvidersConfigurationSection.InstrumentationTypeRegistrationProviderName, SectionName = BlockSectionNames.Instrumentation });
            BaseAdd(new TypeRegistrationProviderElement() { Name = TypeRegistrationProvidersConfigurationSection.LoggingTypeRegistrationProviderName, SectionName = BlockSectionNames.Logging });
            BaseAdd(new TypeRegistrationProviderElement() { Name = TypeRegistrationProvidersConfigurationSection.PolicyInjectionTypeRegistrationProviderName, SectionName = BlockSectionNames.PolicyInjection });
            BaseAdd(new TypeRegistrationProviderElement() { Name = TypeRegistrationProvidersConfigurationSection.SecurityTypeRegistrationProviderName, SectionName = BlockSectionNames.Security });
            BaseAdd(new TypeRegistrationProviderElement() { Name = TypeRegistrationProvidersConfigurationSection.DataAccessTypeRegistrationProviderName, ProviderTypeName = BlockSectionNames.DataRegistrationProviderLocatorType });
            BaseAdd(new TypeRegistrationProviderElement() { Name = TypeRegistrationProvidersConfigurationSection.ValidationTypeRegistrationProviderName,  ProviderTypeName = BlockSectionNames.ValidationRegistrationProviderLocatorType });
        }
    }
}
