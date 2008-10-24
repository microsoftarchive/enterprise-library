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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents the information about a configuration section. This is used by a <see cref="IConfigurationDesignManager"/>.
	/// </summary>
	public class ConfigurationSectionInfo
	{
		private readonly ConfigurationNode node;
		private readonly ConfigurationSection section;
		private readonly string sectionName;
        private readonly string protectionProviderName;

        /// <summary>
        /// Initialize a new instance of the <see cref="ConfigurationSectionInfo"/> class with the node representing the root of the configuration, the ConfigurationSection and the section name.
        /// </summary>
        /// <param name="node">The root node of the configuration.</param>
        /// <param name="section">The configuration section.</param>
        /// <param name="sectionName">The name of the section.</param>
        public ConfigurationSectionInfo(ConfigurationNode node, ConfigurationSection section, string sectionName)
            :this(node, section, sectionName, string.Empty)
        {
        }
		
        /// <summary>
		/// Initialize a new instance of the <see cref="ConfigurationSectionInfo"/> class with the node representing the root of the configuration, the ConfigurationSection and the section name.
		/// </summary>
		/// <param name="node">The root node of the configuration.</param>
		/// <param name="section">The configuration section.</param>
		/// <param name="sectionName">The name of the section.</param>
        /// <param name="protectionProviderName">TODO: add comment</param>
		public ConfigurationSectionInfo(ConfigurationNode node, ConfigurationSection section, string sectionName, string protectionProviderName)
		{
			this.node = node;
			this.section = section;
			this.sectionName = sectionName;
            this.protectionProviderName = protectionProviderName;
		}

		/// <summary>
		/// Gets the root node of the configuration.
		/// </summary>
		/// <value>
		/// The root node of the configuration.
		/// </value>
		public ConfigurationNode Node
		{
			get { return node; }
		}

		/// <summary>
		/// Gets the configuration section.
		/// </summary>
		/// <value>
		/// The configuration section.
		/// </value>
		public ConfigurationSection Section
		{
			get { return section; }
		}

		/// <summary>
		/// Gets the configuration section name.
		/// </summary>
		/// <value>
		/// The configuration section name.
		/// </value>
		public string SectionName
		{
			get { return sectionName; }
		}

        /// <summary>
        /// TODO: add comment
        /// </summary>
        /// <value>
        /// TODO: add comment
        /// </value>
        public string ProtectionProviderName
        {
            get { return protectionProviderName; }
        }
	}
}
