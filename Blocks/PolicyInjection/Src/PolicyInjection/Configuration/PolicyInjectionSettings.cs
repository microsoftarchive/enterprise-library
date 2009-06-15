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
using System.Configuration;
using System.Linq;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A <see cref="ConfigurationSection"/> that stores the policy set in configuration.
    /// </summary>
    public class PolicyInjectionSettings : SerializableConfigurationSection, ITypeRegistrationsProvider
    {
        //private const string InjectorsPropertyName = "injectors";
        private const string PoliciesPropertyName = "policies";

        /// <summary>
        /// Section name as it appears in the config file.
        /// </summary>
        public const string SectionName = BlockSectionNames.PolicyInjection;

        /// <summary>
        /// Gets or sets the collection of Policies from configuration.
        /// </summary>
        /// <value>The <see cref="PolicyData"/> collection.</value>
        [ConfigurationProperty(PoliciesPropertyName)]
        public NamedElementCollection<PolicyData> Policies
        {
            get { return (NamedElementCollection<PolicyData>)base[PoliciesPropertyName]; }
            set { base[PoliciesPropertyName] = value; }
        }

        /// <summary>
        /// Gets a value indicating whether an unknown element is encountered during deserialization.
        /// </summary>
        /// <param name="elementName">The name of the unknown subelement.</param>
        /// <param name="reader">The <see cref="XmlReader"/> being used for deserialization.</param>
        /// <returns>true when an unknown element is encountered while deserializing; otherwise, false.</returns>
        /// <remarks>This class will ignore an element named "injectors".</remarks>
        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            if (elementName == "injectors")
            {
                reader.Skip();
                return true;
            }

            return base.OnDeserializeUnrecognizedElement(elementName, reader);
        }

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
    }
}
