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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// A <see cref="TypeRegistrationsProvider"/> implementation that
    /// loads a type by name, and returns an instance of that type as the provider.
    /// </summary>
    /// <remarks>
    /// This is primarily used to support the Data Access Application Block's configuration provider, which
    /// has to pull stuff from several spots. Also, we load by name rather than
    /// using a type object directly to avoid a compile time dependency from Common on the
    /// Data assembly.
    /// </remarks>
    public class TypeLoadingLocator : TypeRegistrationsProvider
    {
        /// <summary>
        /// Construct a <see cref="TypeLoadingLocator"/> that will use the
        /// type named in <paramref name="typeName"/> as the provider.
        /// </summary>
        /// <param name="typeName">type to construct as a provider. This type must have a single argument
        /// constructor that takes an <see cref="IConfigurationSource"/> parameter.</param>
        public TypeLoadingLocator(string typeName)
            : this(typeName, new NullContainerReconfiguringEventSource())
        {
        }

        /// <summary>
        /// Construct a <see cref="TypeLoadingLocator"/> that will use the
        /// type named in <paramref name="typeName"/> as the provider.
        /// </summary>
        /// <param name="typeName">type to construct as a provider. This type must have a single argument
        /// constructor that takes an <see cref="IConfigurationSource"/> parameter.</param>
        /// <param name="reconfiguringEventSource">The event source containing events raised when the configuration source is changed.</param>
        public TypeLoadingLocator(string typeName, IContainerReconfiguringEventSource reconfiguringEventSource)
            : base(typeName)
        {
            if (reconfiguringEventSource == null) throw new ArgumentNullException("reconfiguringEventSource");

            reconfiguringEventSource.ContainerReconfiguring += OnContainerReconfiguring;
        }

        private void OnContainerReconfiguring(object sender, ContainerReconfiguringEventArgs e)
        {
            e.AddTypeRegistrations(GetUpdatedRegistrations(e.ConfigurationSource));   
        }

        /// <summary>
        /// Return the <see cref="TypeRegistration"/> objects needed to configure
        /// the container.
        /// </summary>
        /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            return GetRegistrationsInternal(configurationSource, (p, cs) => p.GetRegistrations(cs));
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
            return GetRegistrationsInternal(configurationSource, (p, cs) => p.GetUpdatedRegistrations(cs));
        }

        private IEnumerable<TypeRegistration> GetRegistrationsInternal(IConfigurationSource configurationSource,
            Func<ITypeRegistrationsProvider, IConfigurationSource, IEnumerable<TypeRegistration>> registrationAccessor)
        {
            Type providerType = Type.GetType(Name);
            if (providerType == null) return new TypeRegistration[0];

            var provider = (ITypeRegistrationsProvider)Activator.CreateInstance(providerType);
            return registrationAccessor(provider, configurationSource);
        }
    }
}
