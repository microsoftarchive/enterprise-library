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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    /// <summary>
    /// Service class used to get the names of configuration sections available in the designer.
    /// </summary>
    /// <remarks>
    /// In order to get an instance of this class, declare it as a constructor argument on the consuming component or use the <see cref="IUnityContainer"/> to obtain an instance from code.
    /// </remarks>
    /// <see cref="HandlesSectionAttribute"/>
    public abstract class ConfigurationSectionLocator
    {
        /// <summary>
        /// Gets the list of known section names.
        /// </summary>
        /// <remarks>
        ///  If the configuration file contains a section with a known section name, the configuration designer will attempt to open it.
        /// </remarks>
        /// <value>
        /// The list of known section names.
        /// </value>
        public abstract IEnumerable<string> ConfigurationSectionNames { get; }

        /// <summary>
        /// Gets the list of section names that are written to by the configuration designer when saving and therefore should be removed from the target configuration file, prior to saving.
        /// </summary>
        /// <value>
        /// The list of section names that are written to by the configuration designer when saving and therefore should be removed from the target configuration file, prior to saving.
        /// </value>
        public abstract IEnumerable<string> ClearableConfigurationSectionNames{ get; }
    }
}
