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
using System.Text;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Configuration section for the configuration sources.
    /// </summary>
	/// <remarks>
	/// This configuration must reside in the application's default configuration file.
	/// </remarks>
    public class ConfigurationSourceSection : SerializableConfigurationSection
    {
        private const string selectedSourceProperty = "selectedSource";
        private const string sourcesProperty = "sources";

        /// <summary>
		/// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
        public const string SectionName = "enterpriseLibrary.ConfigurationSource";        

        /// <summary>
		/// Returns the <see cref="ConfigurationSourceSection"/> from the application's default configuration file.
        /// </summary>
		/// <returns>The section from the configuration file, or <see langword="null"/> (<b>Nothing</b> in Visual Basic) if the section is not present in the configuration file.</returns>
        public static ConfigurationSourceSection GetConfigurationSourceSection()
        {
			return (ConfigurationSourceSection)ConfigurationManager.GetSection(SectionName);
        }

        /// <summary>
        /// Gets or sets the name for the default configuration source.
        /// </summary>
        [ConfigurationProperty(selectedSourceProperty, IsRequired=true)]
        public string SelectedSource
        {
            get
            {
                return (string)this[selectedSourceProperty];
            }
			set
			{
				this[selectedSourceProperty] = value;
			}
        }

        /// <summary>
        /// Gets the collection of defined configuration sources.
        /// </summary>
        [ConfigurationProperty(sourcesProperty, IsRequired = true)]
        public NameTypeConfigurationElementCollection<ConfigurationSourceElement, ConfigurationSourceElement> Sources
        {
            get
            {
                return (NameTypeConfigurationElementCollection<ConfigurationSourceElement, ConfigurationSourceElement>)this[sourcesProperty];
            }           

        }
    }
}
