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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Entry point for the container infrastructure for Enterprise Library.
    /// </summary>
    public class EnterpriseLibraryContainer
    {
        private static volatile IServiceLocator currentContainer;
        private static readonly object currentContainerLock = new object();

        /// <summary>
        /// Get or set the current container used to resolve Entlib objects (for use by the
        /// various static factories).
        /// </summary>
        public static IServiceLocator Current
        {
            get
            {
                SetCurrentContainerIfNotSet();
                return currentContainer;
            }

            set
            {
                lock (currentContainerLock)
                {
                    currentContainer = value;
                }
            }
        }

        /// <summary>
        /// Read the current Enterprise Library configuration in the given <paramref name="configSource"/>
        /// and supply the corresponding type information to the <paramref name="configurator"/>.
        /// </summary>
        /// <param name="configurator"><see cref="IContainerConfigurator"/> object used to consume the configuration
        /// information.</param>
        /// <param name="configSource">Configuration information.</param>
        public static void ConfigureContainer(IContainerConfigurator configurator, IConfigurationSource configSource)
        {
            var reconfiguringEventSource = configurator as IContainerReconfiguringEventSource ??
                                           new NullContainerReconfiguringEventSource();


            ConfigureContainer(
                TypeRegistrationsProvider.CreateDefaultProvider(configSource, reconfiguringEventSource),
                configurator,
                configSource);
        }

        /// <summary>
        /// Read the current Enterprise Library configuration in the given <paramref name="configSource"/>
        /// and supply the corresponding type information to the <paramref name="configurator"/>.
        /// </summary>
        /// <param name="locator"><see cref="TypeRegistrationsProvider"/> used to identify what information
        /// to pull from the configuration file.</param>
        /// <param name="configurator"><see cref="IContainerConfigurator"/> object used to consume the configuration
        /// information.</param>
        /// <param name="configSource">Configuration information.</param>
        public static void ConfigureContainer(ITypeRegistrationsProvider locator,
            IContainerConfigurator configurator,
            IConfigurationSource configSource)
        {
            if (configurator == null) throw new ArgumentNullException("configurator");

            configurator.RegisterAll(configSource, locator);

        }

        private static void SetCurrentContainerIfNotSet()
        {
            if (currentContainer == null)
            {
                lock (currentContainerLock)
                {
                    if (currentContainer == null)
                    {
                        currentContainer = CreateDefaultContainer();
                    }
                }
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="IServiceLocator"/> that has been configured
        /// with the information in the default <see cref="IConfigurationSource"/>
        /// </summary>
        /// <returns>The <see cref="IServiceLocator"/> object.</returns>
        public static IServiceLocator CreateDefaultContainer()
        {
            return CreateDefaultContainer(ConfigurationSourceFactory.Create());
        }

        /// <summary>
        /// Create a new instance of <see cref="IServiceLocator"/> that has been configured
        /// with the information in the given <paramref name="configurationSource"/>.
        /// </summary>
        /// <param name="configurationSource"><see cref="IConfigurationSource"/> containing Enterprise Library
        /// configuration information.</param>
        /// <returns>The <see cref="IServiceLocator"/> object.</returns>
        public static IServiceLocator CreateDefaultContainer(IConfigurationSource configurationSource)
        {
            IUnityContainer container = new UnityContainer();
            var configurator = new UnityContainerConfigurator(container);
            ConfigureContainer(configurator, configurationSource);
            return new UnityServiceLocator(container);
        }
    }
}
