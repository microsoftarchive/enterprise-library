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
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Represents a configuration accessor for system configuration (.NET configuration).
    /// </summary>
    public class ConfigurationInstanceConfigurationAccessor : IConfigurationAccessor
    {
        readonly System.Configuration.Configuration configuration;
        readonly IDictionary<string, bool> requestedSections;

        /// <summary>
        /// Initialize a new instance of the <see cref="ConfigurationInstanceConfigurationAccessor"/> with the configuration to access.
        /// </summary>
        /// <param name="configuration">
        /// The configuration to access.
        /// </param>
        public ConfigurationInstanceConfigurationAccessor(System.Configuration.Configuration configuration)
        {
            this.configuration = configuration;
            requestedSections = new Dictionary<string, bool>();
        }

        /// <summary>
        /// Gets the section names for the requested configuration.
        /// </summary>
        /// <returns>
        /// A collection of configuration names.
        /// </returns>
        public IEnumerable<string> GetRequestedSectionNames()
        {
            String[] requestedSectionNames = new String[requestedSections.Keys.Count];
            requestedSections.Keys.CopyTo(requestedSectionNames, 0);

            return requestedSectionNames;
        }

        /// <summary>
        /// Get a configuration section based on name.
        /// </summary>
        /// <param name="sectionName">The name of the configuration section.</param>
        /// <returns>The <see cref="ConfigurationSection"/> for the name.</returns>
        public ConfigurationSection GetSection(string sectionName)
        {
            ConfigurationSection section = configuration.GetSection(sectionName);
            requestedSections[sectionName] = section != null;

            return section;
        }

        /// <summary>
        /// Remove a configuration section.
        /// </summary>
        /// <param name="sectionName">The name of the section to remove.</param>
        public void RemoveSection(string sectionName)
        {
            configuration.Sections.Remove(sectionName);
        }
    }
}
