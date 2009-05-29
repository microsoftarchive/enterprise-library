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
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Event arguments describing which sections have changed in a configuration source.
    /// </summary>
    public class ConfigurationSourceChangedEventArgs : EventArgs
    {
        private readonly IConfigurationSource configurationSource;
        private readonly IServiceLocator container;
        private readonly ReadOnlyCollection<string> changedSectionNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSourceChangedEventArgs"/> class.
        /// </summary>
        /// <param name="configurationSource">Configuration source that changed.</param>
        /// <param name="changedSectionNames">Sequence of the section names in <paramref name="configurationSource"/>
        /// that have changed.</param>
        public ConfigurationSourceChangedEventArgs(IConfigurationSource configurationSource, 
            IEnumerable<string> changedSectionNames)
            : this(configurationSource, null, changedSectionNames)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.EventArgs"/> class.
        /// </summary>
        /// <param name="configurationSource">Configuration source that changed.</param>
        /// <param name="container"><see cref="IServiceLocator"/> object that has been configured with the
        /// contents of <paramref name="configurationSource"/>.</param>
        /// <param name="changedSectionNames">Sequence of the section names in <paramref name="configurationSource"/>
        /// that have changed.</param>
        public ConfigurationSourceChangedEventArgs(
            IConfigurationSource configurationSource, 
            IServiceLocator container,
            IEnumerable<string> changedSectionNames)
        {
            this.configurationSource = configurationSource;
            this.container = container;
            this.changedSectionNames = new ReadOnlyCollection<string>(changedSectionNames.ToArray());
        }

        /// <summary>
        /// The configuration source that has changed.
        /// </summary>
        public IConfigurationSource ConfigurationSource
        {
            get { return configurationSource; }
        }

        /// <summary>
        /// The container that has been configured with the new
        /// configuration.
        /// </summary>
        /// <remarks>If this event is received directly from a 
        /// <see cref="IConfigurationSource"/> this property will
        /// be null. Otherwise it will reference a valid container
        /// that has been configured with the contents of the updated
        /// configuration source.</remarks>
        public IServiceLocator Container
        {
            get { return container; }
        }

        /// <summary>
        /// The set of section names that have changed.
        /// </summary>
        public ReadOnlyCollection<string> ChangedSectionNames
        {
            get { return changedSectionNames; }
        }
    }
}
