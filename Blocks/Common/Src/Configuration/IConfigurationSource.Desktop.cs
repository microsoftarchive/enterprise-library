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
    partial interface IConfigurationSource
    {
        /// <summary>
        /// Event raised when any section in this configuration source changes.
        /// </summary>
        event EventHandler<ConfigurationSourceChangedEventArgs> SourceChanged;

        /// <summary>
        /// Adds a handler to be called when changes to the section named <paramref name="sectionName"/> are detected.
        /// </summary>
        /// <param name="sectionName">The name of the section to watch for.</param>
        /// <param name="handler">The handler for the change event to add.</param>
        void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler);

        /// <summary>
        /// Removes a handler to be called when changes to section <code>sectionName</code> are detected.
        /// </summary>
        /// <param name="sectionName">The name of the watched section.</param>
        /// <param name="handler">The handler for the change event to remove.</param>
        void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler);
    }
}
