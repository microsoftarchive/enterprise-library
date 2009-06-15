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
using System.Configuration;
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// A <see cref="TypeRegistrationsProvider"/> implementation that looks up
    /// a provider by looking for the named configuration section in the given <see cref="IConfigurationSource"/>.
    /// If found, tries to cast the config section to <see cref="ITypeRegistrationsProvider"/>.
    /// </summary>
    public class ConfigSectionLocator : TypeRegistrationsProvider
    {
        /// <summary>
        /// Construct an instance of <see cref="ConfigSectionLocator"/> that will
        /// look for the given <paramref name="sectionName"/>.
        /// </summary>
        /// <param name="sectionName">Section name in configuration to look for.</param>
        public ConfigSectionLocator(string sectionName)
            : this(sectionName, new NullContainerReconfiguringEventSource())
        {
        }

        /// <summary>
        /// Construct an instance of <see cref="ConfigSectionLocator"/> that
        /// will look for the given <paramref name="sectionName"/>. It also
        /// registers for the <see cref="IContainerReconfiguringEventSource.ContainerReconfiguring"/>
        /// event, and will request updated type registrations from the section
        /// at that time.
        /// </summary>
        /// <param name="sectionName">Section name in configuration to look for.</param>
        /// <param name="reconfiguringEventSource">Event source to signal when reconfiguration is needed.</param>
        public ConfigSectionLocator(string sectionName, IContainerReconfiguringEventSource reconfiguringEventSource)
            : base(sectionName)
        {
            if(reconfiguringEventSource == null) throw new ArgumentNullException("reconfiguringEventSource");

            reconfiguringEventSource.ContainerReconfiguring += OnContainerReconfiguring;
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
            Func<ITypeRegistrationsProvider, IConfigurationSource, IEnumerable<TypeRegistration>> registrationsAccessor)
        {
            ITypeRegistrationsProvider provider = null;
            ConfigurationSection section = configurationSource.GetSection(Name);
            if (section != null)
            {
                provider = section as ITypeRegistrationsProvider;
            }

            if (provider != null)
            {
                return registrationsAccessor(provider, configurationSource);
            }
            return Enumerable.Empty<TypeRegistration>();
        }


        private void OnContainerReconfiguring(object sender, ContainerReconfiguringEventArgs e)
        {
            e.AddTypeRegistrations(GetUpdatedRegistrations(e.ConfigurationSource));
        } 
    }
}
