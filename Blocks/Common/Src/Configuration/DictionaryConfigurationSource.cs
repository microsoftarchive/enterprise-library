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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Represents a configuration source that is backed by a dictionary of named objects.
    /// </summary>
    public partial class DictionaryConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// </summary>
        protected internal Dictionary<string, ConfigurationSection> sections;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryConfigurationSource"/> class.
        /// </summary>
        public DictionaryConfigurationSource()
        {
            this.sections = new Dictionary<string, ConfigurationSection>();
        }

        /// <summary>
        /// Retrieves the specified <see cref="ConfigurationSection"/>.
        /// </summary>
        /// <param name="sectionName">The name of the section to be retrieved.</param>
        /// <returns>The specified <see cref="ConfigurationSection"/>, or <see langword="null"/> (<b>Nothing</b> in Visual Basic)
        /// if a section by that name is not found.</returns>
        public ConfigurationSection GetSection(string sectionName)
        {
            if (sections.ContainsKey(sectionName))
            {
                return sections[sectionName];
            }
            return null;
        }

        /// <summary>
        /// Adds a <see cref="ConfigurationSection"/> to the configuration source.
        /// </summary>
        /// <remarks>
        /// If a configuration section with the specified name already exists it will be replaced.
        /// </remarks>
        /// <param name="name">The name by which the <paramref name="section"/> should be added.</param>
        /// <param name="section">The configuration section to add.</param>
        public void Add(string name, ConfigurationSection section)
        {
            sections.Add(name, section);
        }

        /// <summary>
        /// Removes a <see cref="ConfigurationSection"/> from the configuration source.
        /// </summary>
        /// <param name="sectionName">The name of the section to remove.</param>
        public void Remove(string sectionName)
        {
            sections.Remove(sectionName);
        }

        /// <summary>
        /// Determines if a section name exists in the source.
        /// </summary>
        /// <param name="name">The section name to find.</param>
        /// <returns><b>true</b> if the section exists; otherwise, <b>false</b>.</returns>
        public bool Contains(string name)
        {
            return sections.ContainsKey(name);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the method is being called from the <see cref="Dispose()"/> method. <see langword="false"/> if it is being called from within the object finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Releases resources for the <see cref="DictionaryConfigurationSource"/> instance before garbage collection.
        /// </summary>
        ~DictionaryConfigurationSource()
        {
            this.Dispose(false);
        }
    }
}
