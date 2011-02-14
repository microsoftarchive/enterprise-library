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
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    partial class DictionaryConfigurationSource
    {
        /// <summary>
        /// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// </summary>
        protected internal EventHandlerList eventHandlers = new EventHandlerList();

        /// <summary>
        /// Raised when anything in the source changes.
        /// </summary>
        /// <remarks>
        /// <see cref="DictionaryConfigurationSource"/> does not report any
        /// configuration change events.
        /// </remarks>
        public event EventHandler<ConfigurationSourceChangedEventArgs> SourceChanged;

        /// <summary>
        /// Adds a handler to be called when changes to the section named <paramref name="sectionName"/> are detected.
        /// </summary>
        /// <param name="sectionName">The name of the section to watch for.</param>
        /// <param name="handler">The handler for the change event to add.</param>
        public void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {
            eventHandlers.AddHandler(sectionName, handler);
        }

        /// <summary>
        /// Removes a handler to be called when changes to section <code>sectionName</code> are detected.
        /// </summary>
        /// <param name="sectionName">The name of the watched section.</param>
        /// <param name="handler">The handler for the change event to remove.</param>
        public void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {
            eventHandlers.RemoveHandler(sectionName, handler);
        }

        /// <summary>
        /// Raises the <see cref="SourceChanged"/> event.
        /// </summary>
        /// <param name="args">Event arguments</param>
        protected void OnSourceChangedEvent(ConfigurationSourceChangedEventArgs args)
        {
            var handler = this.SourceChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}
