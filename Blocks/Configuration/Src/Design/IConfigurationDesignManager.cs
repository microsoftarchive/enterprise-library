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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents the object that will manage the design of configuration..
    /// </summary>
    public interface IConfigurationDesignManager
    {
        /// <summary>
        /// When implemented by a class, allows the registration of configuration nodes and commands into the configuration tree.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        void Register(IServiceProvider serviceProvider);

        /// <summary>
        /// When implemented by a class, saves the configuration data for the implementer.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        void Save(IServiceProvider serviceProvider);

        /// <summary>
        /// When implemented by a class, opens the configuration for the application.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        void Open(IServiceProvider serviceProvider);

        /// <summary>
        /// When implemented by a class, adds the configuration data for the current implementer to the <see cref="Common.Configuration.DictionaryConfigurationSource"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        /// <param name="dictionaryConfigurationSource">
        /// A <see cref="Common.Configuration.DictionaryConfigurationSource"/> object that will contain the configuration data.</param>
        void BuildConfigurationSource(IServiceProvider serviceProvider, DictionaryConfigurationSource dictionaryConfigurationSource);
    }
}