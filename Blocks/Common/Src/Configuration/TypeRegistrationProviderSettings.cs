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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Contains settings specific to the registration of a <see cref="TypeRegistrationsProvider"/>.
    /// </summary>
    public class TypeRegistrationProviderSettings : ConfigurationElement
    {
        private const string nameProperty = "name";
        private const string sectionNameProperty = "sectionName";
        private const string providerTypeNameProperty = "providerType";

        /// <summary>
        /// The name used to uniquely identify the <see cref="TypeRegistrationsProvider"/> in configuration.
        /// </summary>
        [ConfigurationProperty(nameProperty, IsKey = true, IsRequired=true)]
        public string Name
        {
            get
            {
                return (string)base[nameProperty];
            }
            set
            {
                base[nameProperty] = value;
            }
        }

        /// <summary>
        /// The section name used to retrieve the <see cref="ITypeRegistrationsProvider"/> if available.
        /// </summary>
        [ConfigurationProperty(sectionNameProperty)]
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
        /// The name of the type that implements <see cref="ITypeRegistrationsProvider"/> if available. 
        /// </summary>
        [ConfigurationProperty(providerTypeNameProperty)]
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
