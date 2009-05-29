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
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// This class is the event arguments received when a container is being
    /// reconfigured due to a configuration source change. This class is a
    /// collecting argument: new type registrations should be added via the
    /// AddTypeRegistrations method.
    /// </summary>
    public abstract class ContainerReconfiguringEventArgs : EventArgs
    {
        private IConfigurationSource configurationSource;
        private ReadOnlyCollection<string> changedSectionNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerReconfiguringEventArgs"/> class.
        /// </summary>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> that changed,
        /// causing the need to reconfigure the container.</param>
        /// <param name="changedSectionNames">Sequence of changed section names in 
        /// <paramref name="configurationSource"/>.</param>
        protected ContainerReconfiguringEventArgs(IConfigurationSource configurationSource,
            IEnumerable<string> changedSectionNames)
        {
            this.configurationSource = configurationSource;
            this.changedSectionNames = new ReadOnlyCollection<string>(changedSectionNames.ToArray());
        }

        /// <summary>
        /// The updated configuration source.
        /// </summary>
        public IConfigurationSource ConfigurationSource
        {
            get { return configurationSource; }
        }

        /// <summary>
        /// The section names that have changed.
        /// </summary>
        public ReadOnlyCollection<string> ChangedSectionNames
        {
            get { return changedSectionNames; }
        }

        /// <summary>
        /// Called by event receivers to collect the set of type registrations that
        /// must be used to update the container.
        /// </summary>
        /// <param name="newRegistrations">The new set of type registrations.</param>
        public abstract void AddTypeRegistrations(IEnumerable<TypeRegistration> newRegistrations);
    }
}
