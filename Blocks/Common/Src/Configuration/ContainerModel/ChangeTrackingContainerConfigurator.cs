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
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// Base class that manages the logic for tracking configuration source changes and
    /// reconfiguring the container, and raising all the appropriate events.
    /// </summary>
    public abstract class ChangeTrackingContainerConfigurator :
        IContainerConfigurator,
        IContainerReconfiguringEventSource,
        IDisposable
    {
        private IConfigurationSource configurationSource;
        private object getUpdatedRegistrationsLock = new object();
        private ConfigurationChangeEventSourceImpl eventSource = new ConfigurationChangeEventSourceImpl();

        /// <summary>
        /// The event raised when this container must be reconfigured.
        /// </summary>
        public virtual event EventHandler<ContainerReconfiguringEventArgs> ContainerReconfiguring;

        /// <summary>
        /// Deregister for change notification on the configuration source.
        /// </summary>
        public virtual void Dispose()
        {
            if (configurationSource != null)
            {
                configurationSource.SourceChanged -= OnConfigurationSourceChanged;
                configurationSource = null;
            }
        }

        /// <summary>
        /// The <see cref="ConfigurationChangeEventSource"/> implementation that should
        /// be registered with the container. This object will be used to signal interested
        /// objects that the container has completed reconfiguration.
        /// </summary>
        protected ConfigurationChangeEventSource ChangeEventSource { get { return eventSource; } }

        /// <summary>
        /// Consume the set of <see cref="TypeRegistration"/> objects and
        /// configure the associated container.
        /// </summary>
        /// <param name="configurationSource">Configuration source to read registrations from.</param>
        /// <param name="rootProvider"><see cref="ITypeRegistrationsProvider"/> that knows how to
        /// read the <paramref name="configurationSource"/> and return all relevant type registrations.</param>
        public void RegisterAll(IConfigurationSource configurationSource, ITypeRegistrationsProvider rootProvider)
        {
            RegisterAllCore(configurationSource, rootProvider);

            this.configurationSource = configurationSource;
            configurationSource.SourceChanged += OnConfigurationSourceChanged;
        }

        /// <summary>
        /// When overridden in a derived class, this method should perform the actual registration with the container.
        /// </summary>
        /// <param name="configurationSource">Configuration source to read registrations from.</param>
        /// <param name="rootProvider"><see cref="ITypeRegistrationsProvider"/> that knows how to
        /// read the <paramref name="configurationSource"/> and return all relevant type registrations.</param>
        protected abstract void RegisterAllCore(IConfigurationSource configurationSource,
            ITypeRegistrationsProvider rootProvider);

        /// <summary>
        /// When overridden in a derived class, this method should reconfigure the container
        /// with the provided <paramref name="updatedRegistrations"/>.
        /// </summary>
        /// <param name="updatedRegistrations">The new type registrations to apply to the container.</param>
        protected abstract void RegisterUpdates(IEnumerable<TypeRegistration> updatedRegistrations);

        /// <summary>
        /// When overridden in a derived class, this method should return an implementation
        /// of <see cref="IServiceLocator"/> that wraps the actual container.
        /// </summary>
        /// <returns>The <see cref="IServiceLocator"/> that objects can use to re-resolve
        /// dependencies after the container has been reconfigured.</returns>
        protected abstract IServiceLocator GetLocator();

        /// <summary>
        /// Raise the <see cref="ContainerReconfiguring"/> event in response to a configuration source change.
        /// </summary>
        /// <param name="sender">Source of the event - the configuraton source.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnConfigurationSourceChanged(object sender, ConfigurationSourceChangedEventArgs e)
        {
            ContainerReconfiguringEventArgsImpl eventArgs =
                new ContainerReconfiguringEventArgsImpl(e.ConfigurationSource, e.ChangedSectionNames);

            lock (getUpdatedRegistrationsLock)
            {
                var handler = ContainerReconfiguring;
                if (handler != null)
                {
                    handler(this, eventArgs);
                }

                if (eventArgs.HasRegistrations)
                {
                    RegisterUpdates(eventArgs.Registrations);
                    eventSource.ConfigurationSourceChanged(e.ConfigurationSource, GetLocator(), e.ChangedSectionNames);
                }
            }
        }

        private class ContainerReconfiguringEventArgsImpl : ContainerReconfiguringEventArgs
        {
            private IEnumerable<TypeRegistration> registrations = Enumerable.Empty<TypeRegistration>();

            public ContainerReconfiguringEventArgsImpl(IConfigurationSource configurationSource, IEnumerable<string> changedSectionNames)
                : base(configurationSource, changedSectionNames) { }

            public override void AddTypeRegistrations(IEnumerable<TypeRegistration> newRegistrations)
            {
                registrations = registrations.Concat(newRegistrations);
                HasRegistrations = true;
            }

            public IEnumerable<TypeRegistration> Registrations
            {
                get { return registrations; }
            }

            public bool HasRegistrations { get; set; }
        }
    }
}
