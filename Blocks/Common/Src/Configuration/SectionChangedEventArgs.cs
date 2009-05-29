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
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Event argument passed when a configuration section signals that it has changed.
    /// </summary>
    /// <typeparam name="TSection">Type of the configuration section class that changed.
    /// </typeparam>
    public class SectionChangedEventArgs<TSection> : EventArgs
        where TSection : ConfigurationSection
    {
        private readonly TSection section;
        private readonly IServiceLocator container;

        /// <summary>
        /// Create an instance of the <see cref="SectionChangedEventArgs{TSection}"/> class
        /// that wraps the given section.
        /// </summary>
        /// <param name="section">Configuration section that changed.</param>
        /// <param name="container"><see cref="IServiceLocator"/> that's been configured with
        /// the contents of the <paramref name="section"/>.</param>
        public SectionChangedEventArgs(TSection section, IServiceLocator container)
        {
            this.section = section;
            this.container = container;
        }

        /// <summary>
        /// The configuration section that changed.
        /// </summary>
        public TSection Section
        {
            get { return section; }
        }

        /// <summary>
        /// Container that can be used to resolve newly configured objects.
        /// </summary>
        public IServiceLocator Container
        {
            get { return container; }
        }
    }
}
