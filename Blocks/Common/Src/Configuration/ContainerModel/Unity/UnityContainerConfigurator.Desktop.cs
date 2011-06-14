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
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity
{
    public partial class UnityContainerConfigurator : ChangeTrackingContainerConfigurator
    {
        /// <summary>
        /// Initializer for the configurator.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/> to configure.</param>
        public UnityContainerConfigurator(IUnityContainer container)
        {
            this.container = container;

            this.container.AddNewExtensionIfNotPresent<Interception>();
            this.container.AddNewExtensionIfNotPresent<TransientPolicyBuildUpExtension>();
            this.container.AddNewExtensionIfNotPresent<ReaderWriterLockExtension>();
            this.container.AddNewExtensionIfNotPresent<LifetimeInspector>();
            this.container.AddNewExtensionIfNotPresent<PolicyListAccessor>();
            AddValidationExtension();

            container.RegisterInstance(ChangeEventSource);
        }

        /// <summary>
        /// Consume the set of <see cref="TypeRegistration"/> objects and
        /// configure the associated container.
        /// </summary>
        /// <param name="configurationSource">Configuration source to read registrations from.</param>
        /// <param name="rootProvider"><see cref="ITypeRegistrationsProvider"/> that knows how to
        /// read the <paramref name="configurationSource"/> and return all relevant type registrations.</param>
        protected override void RegisterAllCore(IConfigurationSource configurationSource, ITypeRegistrationsProvider rootProvider)
        {
            EnterWriteLock();
            try
            {
                foreach (var registration in rootProvider.GetRegistrations(configurationSource))
                {
                    Register(registration);
                }
            }
            finally
            {
                ExitWriteLock();
            }
        }

        /// <summary>
        /// When overridden in a derived class, this method should reconfigure the container
        /// with the provided <paramref name="updatedRegistrations"/>.
        /// </summary>
        /// <param name="updatedRegistrations">The new type registrations to apply to the container.</param>
        protected override void RegisterUpdates(IEnumerable<TypeRegistration> updatedRegistrations)
        {
            EnterWriteLock();
            try
            {
                foreach (TypeRegistration updatedRegistration in updatedRegistrations)
                {
                    PolicyListAccessor policyListAccessor = this.container.Configure<PolicyListAccessor>();
                    policyListAccessor.ResetRegistration(updatedRegistration.ServiceType, updatedRegistration.ImplementationType, updatedRegistration.Name);

                    if (ShouldReRegister(updatedRegistration))
                    {
                        Register(updatedRegistration);
                    }
                    else if (updatedRegistration.IsDefault)
                    {
                        FixupDefaultRegistration(updatedRegistration);
                    }
                }
            }
            finally
            {
                ExitWriteLock();
            }
        }

        private void FixupDefaultRegistration(TypeRegistration updatedRegistration)
        {
            var policyListAccessor = this.container.Configure<PolicyListAccessor>();
            policyListAccessor.AddDefaultRegistration(updatedRegistration.ServiceType,
                                                      updatedRegistration.ImplementationType,
                                                      updatedRegistration.Name);
        }

        private bool ShouldReRegister(TypeRegistration updatedRegistration)
        {
            LifetimeInspector lifetimeInspector = container.Configure<LifetimeInspector>();
            return !lifetimeInspector.HasResolvedLifetime(
                        updatedRegistration.ImplementationType,
                        updatedRegistration.Name);

        }

        /// <summary>
        /// When overridden in a derived class, this method should return an implementation
        /// of <see cref="IServiceLocator"/> that wraps the actual container.
        /// </summary>
        /// <returns>The <see cref="IServiceLocator"/> that objects can use to re-resolve
        /// dependencies after the container has been reconfigured.</returns>
        protected override IServiceLocator GetLocator()
        {
            return new UnityServiceLocator(container);
        }

        private void EnterWriteLock()
        {
            container.Configure<ReaderWriterLockExtension>().EnterWriteLock();
        }

        private void ExitWriteLock()
        {
            container.Configure<ReaderWriterLockExtension>().ExitWriteLock();
        }

        private void AddValidationExtension()
        {
            // We load this by name so we don't have a hard dependency from common -> validation
            const string partialExtensionTypeName = "Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Unity.ValidationBlockExtension, Microsoft.Practices.EnterpriseLibrary.Validation";
            const string fullExtensionTypeName =
                "Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Unity.ValidationBlockExtension, Microsoft.Practices.EnterpriseLibrary.Validation, Culture=neutral, Version=5.0.505.0, PublicKeyToken=31bf3856ad364e35";

            Type extensionType = Type.GetType(partialExtensionTypeName) ?? Type.GetType(fullExtensionTypeName);
            if (extensionType != null && container.Configure(extensionType) == null)
            {
                var vabExtension = (UnityContainerExtension)Activator.CreateInstance(extensionType);
                container.AddExtension(vabExtension);
            }
        }
    }
}
