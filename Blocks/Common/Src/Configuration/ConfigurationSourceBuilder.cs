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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using System.Configuration;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Entry point that is used for programatically building up a configution source.
    /// </summary>
    public class ConfigurationSourceBuilder : IConfigurationSourceBuilder, IFluentInterface
    {
        readonly DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

        /// <summary>
        /// Adds a <see cref="ConfigurationSection"/> to the builder.
        /// </summary>
        /// <param name="sectionName">Name of section to add.</param>
        /// <param name="configurationSection">Configuration section to add.</param>
        /// <returns></returns>
        public IConfigurationSourceBuilder AddSection(string sectionName, ConfigurationSection configurationSection)
        {
            configurationSource.Add(sectionName, configurationSection);
            return this;
        }

        /// <summary>
        /// Determines if a section name is contained in the builder.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns>True if contained in the builder, false otherwise.</returns>
        public bool Contains(string sectionName)
        {
            return configurationSource.Contains(sectionName);
        }

        /// <summary>
        /// Updates a configuration source replacing any existing sections with those 
        /// built up with the builder.
        /// </summary>
        /// <param name="source"></param>
        public void UpdateConfigurationWithReplace(IConfigurationSource source)
        {
            foreach (var section in configurationSource.sections)
            {
                source.Remove(null, section.Key);
                source.Add(null, section.Key, section.Value);
            }
        }
    }


    /// <summary>
    /// Defines a configuration source builder.
    /// </summary>
    public interface IConfigurationSourceBuilder : IFluentInterface
    {
        /// <summary>
        /// Adds a <see cref="ConfigurationSection"/> to the builder.
        /// </summary>
        /// <param name="sectionName">Name of section to add.</param>
        /// <param name="section">Configuration section to add.</param>
        /// <returns></returns>
        IConfigurationSourceBuilder AddSection(string sectionName, ConfigurationSection section);

        /// <summary>
        /// Determines if a section name is contained in the builder.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns>True if contained in the builder, false otherwise.</returns>
        bool Contains(string sectionName);
        
        /// <summary>
        /// Updates a configuration source replacing any existing sections with those 
        /// built up with the builder.
        /// </summary>
        /// <param name="source"></param>
        void UpdateConfigurationWithReplace(IConfigurationSource source);
    }
}
