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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using System.Reflection;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{

#pragma warning disable 1591
    /// <summary>
    /// This type supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
    /// be used directly from your code.
    /// Encapsulates the logic to retrieve the attributes for registered manageability providers from assembly dll files.
    /// </summary>
    /// <seealso cref="ConfigurationSectionManageabilityProviderAttribute"/>
    /// <seealso cref="ConfigurationElementManageabilityProviderAttribute"/>
    public class ConfigurationManageabilityProviderAttributeRetriever
    {
        private AssemblyLocator assemblyLocator;
        private ICollection<ConfigurationSectionManageabilityProviderAttribute> sectionManageabilityProviderAttributes;
        private ICollection<ConfigurationElementManageabilityProviderAttribute> elementManageabilityProviderAttributes;

        /// <summary>
        /// This type supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
        /// be used directly from your code.
        /// Initializes a new instance of the <see cref="ConfigurationManageabilityProviderAttributeRetriever"/> class that
        /// will retrieve attributes from the assemblies located in the running application's base directory.
        /// </summary>
        public ConfigurationManageabilityProviderAttributeRetriever(AssemblyLocator assemblyLocator)
        {
            this.assemblyLocator = assemblyLocator;

            sectionManageabilityProviderAttributes
                = new List<ConfigurationSectionManageabilityProviderAttribute>();
            elementManageabilityProviderAttributes
                = new List<ConfigurationElementManageabilityProviderAttribute>();

            LoadRegisteredManageabilityProviders(sectionManageabilityProviderAttributes, elementManageabilityProviderAttributes);
        }


        /// <summary>
        /// This type supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
        /// be used directly from your code.
        /// Gets the retrieved <see cref="ConfigurationSectionManageabilityProviderAttribute"/> instances.
        /// </summary>
        public IEnumerable<ConfigurationSectionManageabilityProviderAttribute> SectionManageabilityProviderAttributes
        {
            get { return sectionManageabilityProviderAttributes; }
        }

        /// <summary>
        /// This type supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
        /// be used directly from your code.
        /// Gets the retrieved <see cref="ConfigurationElementManageabilityProviderAttribute"/> instances.
        /// </summary>
        public IEnumerable<ConfigurationElementManageabilityProviderAttribute> ElementManageabilityProviderAttributes
        {
            get { return elementManageabilityProviderAttributes; }
        }

        /// <devdoc>
        /// Loads the assemblies specified by the assembly names and retrieves the manageability provider attributes from them.
        /// The attributes are stored for later use.
        /// </devdoc>
        private void LoadRegisteredManageabilityProviders(ICollection<ConfigurationSectionManageabilityProviderAttribute> sectionManageabilityProviderAttributes,
            ICollection<ConfigurationElementManageabilityProviderAttribute> elementManageabilityProviderAttributes)
        {
            foreach (Assembly assembly in assemblyLocator.Assemblies)
            {
                LoadAttributes<ConfigurationSectionManageabilityProviderAttribute>(assembly, sectionManageabilityProviderAttributes);
                LoadAttributes<ConfigurationElementManageabilityProviderAttribute>(assembly, elementManageabilityProviderAttributes);
            }
        }

        /// <devdoc>
        /// Retrieves the attributes of type <typeparamref name="T"/> from the given assembly.
        /// </devdoc>
        private static void LoadAttributes<T>(Assembly assembly, ICollection<T> manageabilityProviderAttributes)
        {
            foreach (T attribute in assembly.GetCustomAttributes(typeof(T), false))
            {
                manageabilityProviderAttributes.Add(attribute);
            }
        }
    }

#pragma warning restore 1591
}
