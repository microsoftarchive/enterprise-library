//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration class that stores the policy set in configuration.
    /// </summary>
    public partial class PolicyInjectionSettings : ITypeRegistrationsProvider
    {
        /// <summary>
        /// Section name as it appears in the configuration file.
        /// </summary>
        public const string SectionName = BlockSectionNames.PolicyInjection;

        /// <summary>
        /// Adds to the <paramref name="container"/> the policy definitions represented in the configuration file.
        /// </summary>
        /// <param name="container">The container on which the injection policies must be configured.</param>
        /// <param name="configurationSource">The configuration source from which additional information
        /// can be retrieved, if necessary.</param>
        public void ConfigureContainer(IUnityContainer container, IConfigurationSource configurationSource)
        {
            Guard.ArgumentNotNull(container, "container");
            Guard.ArgumentNotNull(configurationSource, "configurationSource");

            UnityContainerConfigurator configurator = new UnityContainerConfigurator(container);
            configurator.RegisterAll(configurationSource, this);
        }

        /// <summary>
        /// Get the set of <see cref="TypeRegistration"/> objects needed to
        /// register the Policy Injection settings represented by this config section.
        /// </summary>
        /// <param name="configurationSource">This is currently ignored by this routine.</param>
        /// <returns>The set of <see cref="TypeRegistration"/> objects.</returns>
        public IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            List<TypeRegistration> registrations = new List<TypeRegistration>();

            foreach (var policyData in this.Policies)
            {
                registrations.AddRange(policyData.GetRegistrations());
            }

            registrations.Add(GetPolicyInjectorRegistration());

            return registrations;
        }

        /// <summary>
        /// Return the <see cref="TypeRegistration"/> objects needed to reconfigure
        /// the container after a configuration source has changed.
        /// </summary>
        /// <remarks>If there are no reregistrations, return an empty sequence.</remarks>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> containing
        /// the configuration information.</param>
        /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
        public IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
        {
            return GetRegistrations(configurationSource);
        }

        private static TypeRegistration GetPolicyInjectorRegistration()
        {
            return new TypeRegistration<PolicyInjector>(() => new PolicyInjector(Container.Resolved<IServiceLocator>()))
                       {
                           IsDefault = true,
                           IsPublicName =  true
                       };
        }
    }
}
