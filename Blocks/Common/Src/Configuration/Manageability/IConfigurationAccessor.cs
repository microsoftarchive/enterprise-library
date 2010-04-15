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
    /// Defines an accessor for configuration.
    /// </summary>
	public interface IConfigurationAccessor
	{
        /// <summary>
        /// Get a configuration section based on name.
        /// </summary>
        /// <param name="sectionName">The name of the configuration section.</param>
        /// <returns>The <see cref="ConfigurationSection"/> for the name.</returns>
		ConfigurationSection GetSection(String sectionName);

        /// <summary>
        /// Remove a configuration section.
        /// </summary>
        /// <param name="sectionName">The name of the section to remove.</param>
		void RemoveSection(String sectionName);

        /// <summary>
        /// Gets the section names for the requested configuration.
        /// </summary>
        /// <returns>
        /// A collection of configuration names.
        /// </returns>
		IEnumerable<String> GetRequestedSectionNames();
	}
}
