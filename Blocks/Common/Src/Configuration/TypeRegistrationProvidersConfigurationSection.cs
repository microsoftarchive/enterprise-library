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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Contains settings to determine which <see cref="TypeRegistrationsProvider"/> to configure the <see cref="EnterpriseLibraryContainer"/> with.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "TypeRegistrationProvidersConfigurationSectionDescription")]
    [ResourceDisplayName(typeof(DesignResources), "TypeRegistrationProvidersConfigurationSectionDisplayName")]
    public class TypeRegistrationProvidersConfigurationSection : SerializableConfigurationSection
    {
        /// <summary>The section name under which this configuration section is expected to be found.</summary>
        public const string SectionName = "typeRegistrationProvidersConfiguration";
        
        /// <summary>The Type Registration Provider name for the Caching Application Block</summary>
        public const string CachingTypeRegistrationProviderName = "Caching";

        /// <summary>The Type Registration Provider name for the Cryptography Application Block</summary>
        public const string CryptographyTypeRegistrationProviderName = "Cryptography";
        
        /// <summary>The Type Registration Provider name for the Exception Handling Application Block</summary>
        public const string ExceptionHandlingTypeRegistrationProviderName = "Exception Handling";
        
        /// <summary>The Type Registration Provider name for Instrumentation Configuration</summary>
        public const string InstrumentationTypeRegistrationProviderName = "Instrumentation";
        
        /// <summary>The Type Registration Provider name for the Logging Application Block</summary>
        public const string LoggingTypeRegistrationProviderName = "Logging";
        
        /// <summary>The Type Registration Provider name for the Policy Injection Application Block</summary>
        public const string PolicyInjectionTypeRegistrationProviderName = "Policy Injection";
        
        /// <summary>The Type Registration Provider name for the Security Application Block</summary>
        public const string SecurityTypeRegistrationProviderName = "Security";
        
        /// <summary>The Type Registration Provider name for the Data Access Application Block</summary>
        public const string DataAccessTypeRegistrationProviderName = "Data Access";
        
        /// <summary>The Type Registration Provider name for the Validation Application Block</summary>
        public const string ValidationTypeRegistrationProviderName = "Validation";

        private const string TypeRegistrationProvidersProperty = "";

        /// <summary>
        /// Gets the collection of <see cref="TypeRegistrationProviderElement"/> configured in this section.   
        /// </summary>
        [ConfigurationProperty(TypeRegistrationProvidersProperty, Options=ConfigurationPropertyOptions.IsDefaultCollection)]
        [ResourceDescription(typeof(DesignResources), "TypeRegistrationProvidersConfigurationSectionTypeRegistrationProvidersDescription")]
        [ResourceDisplayName(typeof(DesignResources), "TypeRegistrationProvidersConfigurationSectionTypeRegistrationProvidersDisplayName")]
        public TypeRegistrationProviderElementCollection TypeRegistrationProviders
        {
            get
            {
                return (TypeRegistrationProviderElementCollection)base[TypeRegistrationProvidersProperty];
            }
        }
    }
}
