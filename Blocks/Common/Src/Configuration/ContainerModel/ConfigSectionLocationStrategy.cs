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

using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// A <see cref="TypeRegistrationsProviderLocationStrategy"/> implementation that looks up
    /// a provider by looking for the named configuration section in the given <see cref="IConfigurationSource"/>.
    /// If found, tries to cast the config section to <see cref="ITypeRegistrationsProvider"/>.
    /// </summary>
    public class ConfigSectionLocationStrategy : TypeRegistrationsProviderLocationStrategy
    {
        private readonly string sectionName;

        /// <summary>
        /// Construct an instance of <see cref="ConfigSectionLocationStrategy"/> that will
        /// look for the given <paramref name="sectionName"/>.
        /// </summary>
        /// <param name="sectionName">Section name in configuration to look for.</param>
        public ConfigSectionLocationStrategy(string sectionName)
        {
            this.sectionName = sectionName;
        }

        /// <summary>
        /// Name that identifies this particular strategy object.
        /// </summary>
        public override string Name
        {
            get { return sectionName; }
        }

        /// <summary>
        /// Find a single <see cref="ITypeRegistrationsProvider"/> object.
        /// </summary>
        /// <param name="configurationSource"></param>
        /// <returns></returns>
        public override ITypeRegistrationsProvider GetProvider(IConfigurationSource configurationSource)
        {
            ConfigurationSection section = configurationSource.GetSection(sectionName);
            if (section != null)
            {
                return section as ITypeRegistrationsProvider;
            }
            return null;
        }
    }
}
