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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Manageability
{
    /// <summary>
    /// Represents a builder for <see cref="ManageableConfigurationSourceElement"/> objects.
    /// </summary>
    public sealed class ManageableConfigurationSourceElementBuilder
    {
        readonly ManageableConfigurationSourceElementNode node;
        readonly ConfigurationManageabilityProviderAttributeRetriever retriever;

        /// <summary>
        /// Initialize a new instance of the <see cref="ManageableConfigurationSourceElementBuilder"/> class with the node to build the element.
        /// </summary>
        /// <param name="node">The node to user to build the element</param>
        public ManageableConfigurationSourceElementBuilder(ManageableConfigurationSourceElementNode node)
            : this(node, ConfigurationManageabilityProviderAttributeRetriever.CreateInstance(node.Site)) { }

        /// <summary>
        /// Initialize a new instance of the <see cref="ManageableConfigurationSourceElementBuilder"/> class with the node to build the element.
        /// </summary>
        /// <param name="node">The node to user to build the element</param>
        /// <param name="retriever">
        /// The retriver for the configuration attribute.
        /// </param>
        public ManageableConfigurationSourceElementBuilder(ManageableConfigurationSourceElementNode node,
                                                           ConfigurationManageabilityProviderAttributeRetriever retriever)
        {
            this.node = node;
            this.retriever = retriever;
        }

        /// <summary>
        /// Builds the <see cref="ManageableConfigurationSourceElement"/> object.
        /// </summary>
        /// <returns>A <see cref="ManageableConfigurationSourceElement"/> object.</returns>
        public ManageableConfigurationSourceElement Build()
        {
            ManageableConfigurationSourceElement element
                = new ManageableConfigurationSourceElement(
                    node.Name,
                    node.File,
                    node.ApplicationName,
                    node.EnableGroupPolicies,
                    node.EnableWmi);
            foreach (ConfigurationSectionManageabilityProviderData data
                in BuildSectionManageabilityProvidersData(retriever.SectionManageabilityProviderAttributes, retriever.ElementManageabilityProviderAttributes))
            {
                element.ConfigurationManageabilityProviders.Add(data);
            }

            return element;
        }

        /// <summary>
        /// Builds a collection of <see cref="ConfigurationSectionManageabilityProviderData"/> objects.
        /// </summary>
        /// <returns>A collection of <see cref="ConfigurationSectionManageabilityProviderData"/> objects.</returns>
        public static IEnumerable<ConfigurationSectionManageabilityProviderData> BuildSectionManageabilityProvidersData(
            IEnumerable<ConfigurationSectionManageabilityProviderAttribute> sectionManageabilityProviderAttributes,
            IEnumerable<ConfigurationElementManageabilityProviderAttribute> elementManageabilityProviderAttributes)
        {
            List<ConfigurationSectionManageabilityProviderData> result = new List<ConfigurationSectionManageabilityProviderData>();

            foreach (ConfigurationSectionManageabilityProviderAttribute sectionAttribute in sectionManageabilityProviderAttributes)
            {
                ConfigurationSectionManageabilityProviderData providerData
                    = new ConfigurationSectionManageabilityProviderData(sectionAttribute.SectionName, sectionAttribute.ManageabilityProviderType);

                foreach (ConfigurationElementManageabilityProviderAttribute elementAttribute in elementManageabilityProviderAttributes)
                {
                    if (elementAttribute.SectionManageabilityProviderType == sectionAttribute.ManageabilityProviderType)
                    {
                        providerData.ManageabilityProviders.Add(
                            new ConfigurationElementManageabilityProviderData(
                                elementAttribute.TargetType.Name,
                                elementAttribute.ManageabilityProviderType,
                                elementAttribute.TargetType));
                    }
                }

                result.Add(providerData);
            }

            return result;
        }
    }
}
