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
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Represents a source for getting configuration information.
    /// </summary>
    public partial interface IConfigurationSource : IDisposable
    {
        /// <summary>
        /// Retrieves the specified <see cref="ConfigurationSection"/>.
        /// </summary>
        /// <param name="sectionName">The name of the section to be retrieved.</param>
        /// <returns>The specified <see cref="ConfigurationSection"/>, or <see langword="null"/> (<b>Nothing</b> in Visual Basic)
        /// if a section by that name is not found.</returns>
        ConfigurationSection GetSection(string sectionName);

        /// <summary>
        /// Adds a <see cref="ConfigurationSection"/> to the configuration source and saves the configuration source.
        /// </summary>
        /// <remarks>
        /// If a configuration section with the specified name already exists it will be replaced.
        /// </remarks>
        /// <param name="sectionName">The name by which the <paramref name="configurationSection"/> should be added.</param>
        /// <param name="configurationSection">The configuration section to add.</param>
        void Add(string sectionName, ConfigurationSection configurationSection);

        /// <summary>
        /// Removes a <see cref="ConfigurationSection"/> from the configuration source.
        /// </summary>
        /// <param name="sectionName">The name of the section to remove.</param>
        void Remove(string sectionName);
    }
}
