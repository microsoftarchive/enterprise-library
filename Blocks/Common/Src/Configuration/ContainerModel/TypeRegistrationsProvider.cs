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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// This class encapsulates the logic used to find the type registration providers
    /// in the current application.
    /// </summary>
    public abstract class TypeRegistrationsProvider : ITypeRegistrationsProvider
    {
        private readonly string name;

        /// <summary>
        /// Create a new <see cref="TypeRegistrationsProvider"/> instance
        /// without a name.
        /// </summary>
        protected TypeRegistrationsProvider()
        {
            
        }

        /// <summary>
        /// Create a new <see cref="TypeRegistrationsProvider"/> instance.
        /// </summary>
        protected TypeRegistrationsProvider(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Every locator has a name associated with it so that it can be added and removed
        /// from composites. This property returns that name.
        /// </summary>
        public virtual string Name { get { return name; } }

        /// <summary>
        /// Return the <see cref="TypeRegistration"/> objects needed to configure
        /// the container.
        /// </summary>
        /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
        public abstract IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource);

        /// <summary>
        /// Return the <see cref="TypeRegistration"/> objects needed to reconfigure
        /// the container after a configuration source has changed.
        /// </summary>
        /// <remarks>If there are no reregistrations, return an empty sequence.</remarks>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> containing
        /// the configuration information.</param>
        /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
        public virtual IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
        {
            return Enumerable.Empty<TypeRegistration>();
        }

        /// <summary>
        /// Creates a new <see cref="TypeRegistrationsProvider"/> that will return all the
        /// configuration for entlib blocks.
        /// </summary>
        /// <returns>The locator.</returns>
        public static ITypeRegistrationsProvider CreateDefaultProvider()
        {
            return ConfigurationBasedTypeRegistrationsProviderFactory.CreateProvider();
        }

        /// <summary>
        /// Creates a new <see cref="TypeRegistrationsProvider"/> that will return all the
        /// configuration for entlib blocks.
        /// </summary>
        /// <param name="configurationSource">Configuration source containing any customizations
        /// to the locator list.</param>
        /// <param name="reconfiguringEventSource">Event source notifying of container reconfiguration events.</param>
        /// <returns>The locator.</returns>
        public static ITypeRegistrationsProvider CreateDefaultProvider(IConfigurationSource configurationSource, IContainerReconfiguringEventSource reconfiguringEventSource)
        {
            return ConfigurationBasedTypeRegistrationsProviderFactory.CreateProvider(configurationSource, reconfiguringEventSource);
        }
    }

    /// <summary>
    /// A <see cref="TypeRegistrationsProvider"/> that provides a composite
    /// over a collection of individual <see cref="TypeRegistrationsProvider"/>s.
    /// </summary>
    public class CompositeTypeRegistrationsProviderLocator : TypeRegistrationsProvider
    {
        private readonly List<ITypeRegistrationsProvider> locators;

        /// <summary>
        /// Create the composite with the list of locators to use.
        /// </summary>
        /// <param name="locators">The locators.</param>
        public CompositeTypeRegistrationsProviderLocator(IEnumerable<ITypeRegistrationsProvider> locators)
            : base("composite")
        {
            this.locators = new List<ITypeRegistrationsProvider>(locators);
        }

        /// <summary>
        /// Create the composite with the list of locators to use.
        /// </summary>
        /// <param name="locators">The locators.</param>
        public CompositeTypeRegistrationsProviderLocator(params ITypeRegistrationsProvider[] locators)
            : this((IEnumerable<ITypeRegistrationsProvider>)locators)
        {
        }


        /// <summary>
        /// Return the <see cref="TypeRegistration"/> objects needed to configure
        /// the container.
        /// </summary>
        /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            return GetRegistrationsInternal(configurationSource, (l, cs) => l.GetRegistrations(cs));
        }

        /// <summary>
        /// Return the <see cref="TypeRegistration"/> objects needed to reconfigure
        /// the container after a configuration source has changed.
        /// </summary>
        /// <remarks>If there are no reregistrations, return an empty sequence.</remarks>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> containing
        /// the configuration information.</param>
        /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
        public override IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
        {
            return GetRegistrationsInternal(configurationSource, (l, cs) => l.GetUpdatedRegistrations(cs));
        }

        private IEnumerable<TypeRegistration> GetRegistrationsInternal(IConfigurationSource configurationSource,
            Func<ITypeRegistrationsProvider, IConfigurationSource, IEnumerable<TypeRegistration>> registrationsAccessor)
        {
            return locators.SelectMany(l => registrationsAccessor(l, configurationSource));
        }
    }
}
