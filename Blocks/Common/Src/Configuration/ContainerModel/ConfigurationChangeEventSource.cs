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
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// An object that can be injected into various entlib objects
    /// that supplies events that indicate when the current configuration
    /// source has changed. This provides some isolation from the actual
    /// configuration source.
    /// </summary>
    public abstract class ConfigurationChangeEventSource
    {
        /// <summary>
        /// Event raised when the underlying configuration source has changed
        /// any section.
        /// </summary>
        public event EventHandler<ConfigurationSourceChangedEventArgs> SourceChanged;

        /// <summary>
        /// A helper interface used as the return type of the GetSection method.
        /// </summary>
        /// <typeparam name="TSection"></typeparam>
        public interface ISourceChangeEventSource<TSection> where TSection: ConfigurationSection
        {
            /// <summary>
            /// The event raised when the section of type TSection is
            /// changed.
            /// </summary>
            event EventHandler<SectionChangedEventArgs<TSection>> SectionChanged;
        }

        /// <summary>
        /// Used to get an object that lets you register a change handler for a
        /// particular configuration section.
        /// </summary>
        /// <typeparam name="TSection">Type of the configuration section to register against.</typeparam>
        /// <returns>The object that implements the SectionChanged event.</returns>
        public abstract ISourceChangeEventSource<TSection> GetSection<TSection>() where TSection : ConfigurationSection;

        /// <summary>
        /// Utility function to raise the <see cref="SourceChanged"/> event.
        /// </summary>
        /// <param name="configurationSource">Configuration source that changed.</param>
        /// <param name="container"><see cref="IServiceLocator"/> object that has been configured with the
        /// contents of <paramref name="configurationSource"/>.</param>
        /// <param name="changedSectionNames">Sequence of the section names in <paramref name="configurationSource"/>
        /// that have changed.</param>
        protected virtual void OnSourceChanged(IConfigurationSource configurationSource, 
            IServiceLocator container, 
            IEnumerable<string> changedSectionNames)
        {
            EventHandler<ConfigurationSourceChangedEventArgs> handler = SourceChanged;
            if(handler != null)
            {
                var eventArgs = new ConfigurationSourceChangedEventArgs(configurationSource, container, changedSectionNames);
                handler(this, eventArgs);
            }
        }
    }
}
