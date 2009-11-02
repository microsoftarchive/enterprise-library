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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Contains settings specific to the registration of a <see cref="TypeRegistrationsProvider"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "TypeRegistrationProviderElementDescription")]
    [ResourceDisplayName(typeof(DesignResources), "TypeRegistrationProviderElementDisplayName")]
    public class TypeRegistrationProviderElement : NamedConfigurationElement
    {
        private const string sectionNameProperty = "sectionName";
        private const string providerTypeNameProperty = "providerType";

        /// <summary>
        /// The section name used to retrieve the <see cref="ITypeRegistrationsProvider"/> if available.
        /// </summary>
        [ConfigurationProperty(sectionNameProperty)]
        [ResourceDescription(typeof(DesignResources), "TypeRegistrationProviderElementSectionNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "TypeRegistrationProviderElementSectionNameDisplayName")]
        public string SectionName
        {
            get
            {
                return (string)base[sectionNameProperty];
            }
            set
            {
                base[sectionNameProperty] = value;
            }
        }

        /// <summary>
        /// The name of the type that implements <see cref="ITypeRegistrationsProvider"/>. 
        /// </summary>
        [ConfigurationProperty(providerTypeNameProperty)]
        [ResourceDescription(typeof(DesignResources), "TypeRegistrationProviderElementProviderTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "TypeRegistrationProviderElementProviderTypeNameDisplayName")]
        public string ProviderTypeName
        {
            get
            {
                return (string)base[providerTypeNameProperty];
            }
            set
            {
                base[providerTypeNameProperty] = value;
            }
        }
    }
}
