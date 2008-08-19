//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration class that handles the collection of
    /// InjectorData items in configuration.
    /// </summary>
    public class InjectorDataCollection : NameTypeConfigurationElementCollection< InjectorData, CustomInjectorData>
    {
        private const string defaultInjectorPropertyName = "defaultInjector";

        /// <summary>
        /// Gets or sets the default injector name to use, if configured.
        /// </summary>
        [ConfigurationProperty(defaultInjectorPropertyName)]
        public string DefaultInjector
        {
            get { return (string)base[defaultInjectorPropertyName]; }
            set { base[defaultInjectorPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the injector data for the given name.
        /// </summary>
        /// <param name="name">Name of injector data to get.</param>
        /// <returns>The injector data.</returns>
        public new InjectorData this[string name]
        {
            get { return (InjectorData)(BaseGet(name)); }
            set { base[name] = value; }
        }

        /// <summary>
        /// Get the injector data at the given index.
        /// </summary>
        /// <param name="index">Index to retrieve injector data from.</param>
        /// <returns>The injector data.</returns>
        public InjectorData this[int index]
        {
            get { return (InjectorData)( BaseGet(index) ); }
        }
    }
}
