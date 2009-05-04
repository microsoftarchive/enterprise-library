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
using System.IO;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Manageability
{
    /// <summary>
    /// This type supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
    /// be used directly from your code.
    /// Encapsulates the logic to retrieve the attributes for registered manageability providers from assembly dll files.
    /// </summary>
    /// <seealso cref="ConfigurationSectionManageabilityProviderAttribute"/>
    /// <seealso cref="ConfigurationElementManageabilityProviderAttribute"/>
    public class ConfigurationManageabilityProviderAttributeRetriever
    {
        private ICollection<ConfigurationSectionManageabilityProviderAttribute> sectionManageabilityProviderAttributes;
        private ICollection<ConfigurationElementManageabilityProviderAttribute> elementManageabilityProviderAttributes;

        /// <summary>
        /// This type supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
        /// be used directly from your code.
        /// Initializes a new instance of the <see cref="ConfigurationManageabilityProviderAttributeRetriever"/> class that
        /// will retrieve attributes from the assemblies located in the running application's base directory.
        /// </summary>
        public ConfigurationManageabilityProviderAttributeRetriever()
            : this(AppDomain.CurrentDomain.BaseDirectory)
        { }

        /// <summary>
        /// This type supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
        /// be used directly from your code.
        /// Initializes a new instance of the <see cref="ConfigurationManageabilityProviderAttributeRetriever"/> class that
        /// will retrieve attributes from the assemblies located in the given directory.
        /// </summary>
        /// <param name="baseDirectory">The directory where to look for assembly files.</param>
        public ConfigurationManageabilityProviderAttributeRetriever(String baseDirectory)
            : this(GetAssemblyNames(baseDirectory))
        { }

        internal static ConfigurationManageabilityProviderAttributeRetriever CreateInstance(
            IServiceProvider serviceProvider)
        {
            IPluginDirectoryProvider directoryProvider
                = serviceProvider != null
                    ? (IPluginDirectoryProvider)serviceProvider.GetService(typeof(IPluginDirectoryProvider))
                    : null;

            if (directoryProvider != null)
                return new ConfigurationManageabilityProviderAttributeRetriever(directoryProvider.PluginDirectory);

            return new ConfigurationManageabilityProviderAttributeRetriever();
        }

        /// <summary>
        /// This type supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
        /// be used directly from your code.
        /// Initializes a new instance of the <see cref="ConfigurationManageabilityProviderAttributeRetriever"/> class that
        /// will retrieve attributes from the given assembly names.
        /// </summary>
        /// <param name="fileNames">The name of the assemblies where to look for attributes.</param>
        public ConfigurationManageabilityProviderAttributeRetriever(IEnumerable<String> fileNames)
        {
            sectionManageabilityProviderAttributes
                = new List<ConfigurationSectionManageabilityProviderAttribute>();
            elementManageabilityProviderAttributes
                = new List<ConfigurationElementManageabilityProviderAttribute>();

            LoadRegisteredManageabilityProviders(fileNames, sectionManageabilityProviderAttributes, elementManageabilityProviderAttributes);
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
        private static void LoadRegisteredManageabilityProviders(IEnumerable<string> fileNames,
            ICollection<ConfigurationSectionManageabilityProviderAttribute> sectionManageabilityProviderAttributes,
            ICollection<ConfigurationElementManageabilityProviderAttribute> elementManageabilityProviderAttributes)
        {
            foreach (string file in fileNames)
            {
                Assembly assembly = null;

                try
                {
                    AssemblyName assemblyName = AssemblyName.GetAssemblyName(file);
                    assembly = Assembly.Load(assemblyName);
                }
                catch (BadImageFormatException)
                {
                    // not an assembly dll, no need to scan it for registered providers.
                }
                if (assembly != null)
                {
                    LoadAttributes<ConfigurationSectionManageabilityProviderAttribute>(assembly, sectionManageabilityProviderAttributes);
                    LoadAttributes<ConfigurationElementManageabilityProviderAttribute>(assembly, elementManageabilityProviderAttributes);
                }
            }
        }

        private static IEnumerable<string> GetAssemblyNames(String baseDirectory)
        {
            return Directory.GetFiles(baseDirectory, "*.dll");
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
}
