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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Entry point for the container infrastructure for Enterprise Library.
    /// </summary>
    public class EnterpriseLibraryContainer
    {
        /// <summary>
        /// Read the current Enterprise Library configuration in the given <paramref name="configSource"/>
        /// and supply the corresponding type information to the <paramref name="configurator"/>.
        /// </summary>
        /// <param name="configurator"><see cref="IContainerConfigurator"/> object used to consume the configuration
        /// information.</param>
        /// <param name="configSource">Configuration information.</param>
        public static void ConfigureContainer(IContainerConfigurator configurator, IConfigurationSource configSource)
        {
            ConfigureContainer(new TypeRegistrationsProviderLocator(), configurator, configSource);
        }

        /// <summary>
        /// Read the current Enterprise Library configuration in the given <paramref name="configSource"/>
        /// and supply the corresponding type information to the <paramref name="configurator"/>.
        /// </summary>
        /// <param name="locator"><see cref="TypeRegistrationsProviderLocator"/> used to identify what information
        /// to pull from the config file.</param>
        /// <param name="configurator"><see cref="IContainerConfigurator"/> object used to consume the configuration
        /// information.</param>
        /// <param name="configSource">Configuration information.</param>
        public static void ConfigureContainer(TypeRegistrationsProviderLocator locator, 
            IContainerConfigurator configurator, 
            IConfigurationSource configSource)
        {
            var registrations = from provider in locator.GetProviders(configSource)
                                from registration in provider.CreateRegistrations()
                                select registration;

            configurator.RegisterAll(registrations);
            
        }
    }
}
