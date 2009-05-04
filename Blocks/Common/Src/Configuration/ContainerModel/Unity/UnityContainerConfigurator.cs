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
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity
{
    /// <summary>
    /// The <see cref="UnityContainer"/> specific configurator for <see cref="TypeRegistration"/> entries.
    /// </summary>
    public class UnityContainerConfigurator : IContainerConfigurator
    {
        private readonly IUnityContainer container;

        /// <summary>
        /// Initializer for the configurator.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/> to configure.</param>
        public UnityContainerConfigurator(IUnityContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Registers all the <see cref="TypeRegistration"/> entries with the container.
        /// </summary>
        /// <param name="registrationEntries">The type registration entries to add to the container.</param>
        public void RegisterAll(IEnumerable<TypeRegistration> registrationEntries)
        {
            foreach (var registrationEntry in registrationEntries)
            {
                Register(registrationEntry);
            }
        }

        /// <summary>
        /// Registers the <see cref="TypeRegistration"/> entry with the container.
        /// </summary>
        /// <param name="registrationEntry">The type registration entry to add to the container.</param>
        public void Register(TypeRegistration registrationEntry)
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

            container.RegisterType(
                registrationEntry.ServiceType,
                registrationEntry.ImplementationType,
                registrationEntry.Name,
                new ContainerControlledLifetimeManager(),
                injectionMembers.ToArray());
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
    }
}
