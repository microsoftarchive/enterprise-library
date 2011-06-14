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
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity
{
    /// <summary>
    /// 
    /// </summary>
    partial class UnityContainerConfigurator
    {
        private readonly IUnityContainer container;

        /// <summary>
        /// This method performs the registration of the <see cref="IConfigurationSource"/> with the container.
        /// </summary>
        /// <param name="configurationSource">Configuration source to read registrations from.</param>
#if !SILVERLIGHT
        protected override void RegisterConfigurationSource(IConfigurationSource configurationSource)
#else
        protected virtual void RegisterConfigurationSource(IConfigurationSource configurationSource)
#endif
        {
            this.container.RegisterInstance<IConfigurationSource>(configurationSource, new ExternallyControlledLifetimeManager());
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

        private static InjectionParameterValue GetInjectionParameterValue(ParameterValue dependencyParameter)
        {
            var visitor = new UnityParameterVisitor();
            visitor.Visit(dependencyParameter);
            return visitor.InjectionParameter;
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

            public override void AddPolicies(Type serviceType, Type typeToCreate, string name, IPolicyList policies)
            {
                PolicyListAccessor.AddDefaultPolicy(ServiceType, typeToCreate, name, policies);
            }
        }

#if !SILVERLIGHT
        private class LifetimeInspector : UnityContainerExtension
#else
#pragma warning disable 1591
        /// <summary>
        /// This class belongs to the Enterprise Library infrastructure and is not
        /// intended to be used directly from your code.
        /// </summary>
        public class LifetimeInspector : UnityContainerExtension
#endif
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

#if !SILVERLIGHT
        private class PolicyListAccessor : UnityContainerExtension
#else
        /// <summary>
        /// This class belongs to the Enterprise Library infrastructure and is not
        /// intended to be used directly from your code.
        /// </summary>
        public class PolicyListAccessor : UnityContainerExtension
#endif
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
                    NamedTypeBuildKey mappedKey = mappingPolicy.Map(key, null);
                    if (string.Equals(mappedKey.Name, name))
                    {
                        Context.Policies.Clear<IBuildKeyMappingPolicy>(key);
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
#if SILVERLIGHT
#pragma warning restore 1591
#endif

        private class UnityParameterVisitor : ParameterValueVisitor
        {
            public InjectionParameterValue InjectionParameter { get; private set; }

            protected override void VisitConstantParameterValue(ConstantParameterValue parameterValue)
            {
                this.InjectionParameter = new InjectionParameter(parameterValue.Type, parameterValue.Value);
            }

            protected override void VisitResolvedParameterValue(ContainerResolvedParameter parameterValue)
            {
                InjectionParameter = new ResolvedParameter(parameterValue.Type, parameterValue.Name);
            }

            protected override void VisitEnumerableParameterValue(ContainerResolvedEnumerableParameter parameterValue)
            {
                var resolveParameters = parameterValue.Names
                        .Select(name => new ResolvedParameter(parameterValue.ElementType, name))
                        .ToArray();

                InjectionParameter = new ResolvedArrayParameter(parameterValue.ElementType, resolveParameters);
            }
        }
    }
}
