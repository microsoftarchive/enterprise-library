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
    /// <summary>
    /// The <see cref="UnityContainer"/> specific configurator for <see cref="TypeRegistration"/> entries.
    /// </summary>
    public class UnityContainerConfigurator : ChangeTrackingContainerConfigurator
    {
        private readonly IUnityContainer container;

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

        /// <summary>
        /// Registers the <see cref="TypeRegistration"/> entry with the container.
        /// </summary>
        /// <param name="registrationEntry">The type registration entry to add to the container.</param>
        private void Register(TypeRegistration registrationEntry)
        {
            container.RegisterType(
                registrationEntry.ServiceType,
                registrationEntry.ImplementationType,
                registrationEntry.Name,
                CreateLifetimeManager(registrationEntry),
                GetInjectionMembers(registrationEntry));
        }

        private static InjectionMember[] GetInjectionMembers(TypeRegistration registrationEntry)
        {
            List<InjectionMember> injectionMembers =
                new List<InjectionMember>
                {
                    new InjectionConstructor(
                        (from param in registrationEntry.ConstructorParameters 
                         select GetInjectionParameterValue(param)).ToArray()),

                };

            injectionMembers.AddRange(
                    (from prop in registrationEntry.InjectedProperties
                     select new InjectionProperty(
                         prop.PropertyName,
                         GetInjectionParameterValue(prop.PropertyValue))
                    ).Cast<InjectionMember>()
                );

            if (registrationEntry.IsDefault)
            {
                injectionMembers.Add(new DefaultInjectionMember { ServiceType = registrationEntry.ServiceType });
            }

            return injectionMembers.ToArray();
        }

        private void EnterWriteLock()
        {
            container.Configure<ReaderWriterLockExtension>().EnterWriteLock();
        }

        private void ExitWriteLock()
        {
            container.Configure<ReaderWriterLockExtension>().ExitWriteLock();
        }

        private static InjectionParameterValue GetInjectionParameterValue(ParameterValue dependencyParameter)
        {
            if (dependencyParameter is ConstantParameterValue)
            {
                ConstantParameterValue parameterValue = (ConstantParameterValue)dependencyParameter;
                return new InjectionParameter(parameterValue.Type, parameterValue.Value);
            }

            if (dependencyParameter is ContainerResolvedParameter)
            {
                ContainerResolvedParameter containerResolvedParameter = (ContainerResolvedParameter)dependencyParameter;
                return new ResolvedParameter(containerResolvedParameter.Type, containerResolvedParameter.Name);
            }

            if (dependencyParameter is ContainerResolvedEnumerableParameter)
            {
                ContainerResolvedEnumerableParameter containerResolvedParameter =
                    (ContainerResolvedEnumerableParameter)dependencyParameter;

                IEnumerable<ResolvedParameter> resolveParameters =
                    from name in containerResolvedParameter.Names
                    select new ResolvedParameter(containerResolvedParameter.ElementType, name);

                return new ResolvedArrayParameter(containerResolvedParameter.ElementType, resolveParameters.ToArray());
            }

            throw new ArgumentException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resources.ExceptionUnrecognizedDependencyParameterType,
                    dependencyParameter.GetType()),
                "dependencyParameter");
        }

        private static LifetimeManager CreateLifetimeManager(TypeRegistration registrationEntry)
        {
            if (registrationEntry.Lifetime == TypeRegistrationLifetime.Transient)
            {
                return new TransientLifetimeManager();
            }
            return new ContainerControlledLifetimeManager();
        }

        private class DefaultInjectionMember : InjectionMember
        {
            public Type ServiceType { get; set; }

            public override void AddPolicies(Type typeToCreate, string name, IPolicyList policies)
            {
                PolicyListAccessor.AddDefaultPolicy(ServiceType, typeToCreate, name, policies);
            }
        }

        private class LifetimeInspector : UnityContainerExtension
        {
            protected override void Initialize()
            {
            }

            public bool HasResolvedLifetime(Type type, string name)
            {
                NamedTypeBuildKey key = new NamedTypeBuildKey(type, name);
                ILifetimePolicy lifetimePolicy = Context.Policies.Get<ILifetimePolicy>(key);

                if (lifetimePolicy != null)
                {
                    return (lifetimePolicy.GetValue() != null);
                }
                return false;
            }
        }


        private class PolicyListAccessor : UnityContainerExtension
        {
            protected override void Initialize()
            {
            }

            public void ResetRegistration(Type serviceType, Type implementationType, string name)
            {
                NamedTypeBuildKey key = new NamedTypeBuildKey(implementationType, name);
                Context.Policies.Clear(typeof(IPropertySelectorPolicy), key);

                RemoveIsDefault(serviceType, implementationType, name);
            }

            public void RemoveIsDefault(Type serviceType, Type implementationType, string name)
            {
                NamedTypeBuildKey key = new NamedTypeBuildKey(serviceType);

                IBuildKeyMappingPolicy mappingPolicy = Context.Policies.Get<IBuildKeyMappingPolicy>(key);
                if (mappingPolicy != null)
                {
                    object mappedKeyObject = mappingPolicy.Map(key);
                    if (mappedKeyObject is NamedTypeBuildKey)
                    {
                        NamedTypeBuildKey mappedKey = (NamedTypeBuildKey)mappedKeyObject;
                        if (string.Equals(mappedKey.Name, name))
                        {
                            Context.Policies.Clear<IBuildKeyMappingPolicy>(key);
                        }
                    }
                }
            }

            internal static void AddDefaultPolicy(Type serviceType, Type implementationType, string name, IPolicyList policies)
            {
                policies.Set<IBuildKeyMappingPolicy>(
                  new BuildKeyMappingPolicy(new NamedTypeBuildKey(implementationType, name)),
                  new NamedTypeBuildKey(serviceType));
            }

            public void AddDefaultRegistration(Type serviceType, Type implementationType, string name)
            {
                AddDefaultPolicy(serviceType, implementationType, name, Context.Policies);
            }
        }
    }
}
