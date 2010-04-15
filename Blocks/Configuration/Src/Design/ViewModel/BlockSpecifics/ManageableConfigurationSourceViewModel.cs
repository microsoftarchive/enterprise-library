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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class ManageableConfigurationSourceViewModel : CollectionElementViewModel
    {
        public ManageableConfigurationSourceViewModel(
            ElementCollectionViewModel containingCollection,
            ConfigurationElement thisElement,
            AssemblyLocator assemblyLocator)
            : base(containingCollection, thisElement)
        {
            this.assemblyLocator = assemblyLocator;
        }

        private AssemblyLocator assemblyLocator;

        public override void Initialize(InitializeContext context)
        {
            var sourceElement = this.ConfigurationElement as ManageableConfigurationSourceElement;
            if (sourceElement != null)
            {
                sourceElement.ConfigurationManageabilityProviders.Clear();

                var retriever = new ConfigurationManageabilityProviderAttributeRetriever(this.assemblyLocator);
                foreach (var sectionProviderElement in GetConfigurationManageabilityProviders(retriever))
                {
                    sourceElement.ConfigurationManageabilityProviders.Add(sectionProviderElement);
                }
            }
        }

        private static IEnumerable<ConfigurationSectionManageabilityProviderData> GetConfigurationManageabilityProviders(
            ConfigurationManageabilityProviderAttributeRetriever retriever)
        {
            foreach (var sectionAttribute in retriever.SectionManageabilityProviderAttributes)
            {
                var sectionProviderElement =
                    new ConfigurationSectionManageabilityProviderData(
                        sectionAttribute.SectionName,
                        sectionAttribute.ManageabilityProviderType);

                foreach (var elementAttribute in retriever.ElementManageabilityProviderAttributes)
                {
                    if (elementAttribute.SectionManageabilityProviderType == sectionAttribute.ManageabilityProviderType)
                    {
                        sectionProviderElement.ManageabilityProviders.Add(
                            new ConfigurationElementManageabilityProviderData(
                                elementAttribute.TargetType.Name,
                                elementAttribute.ManageabilityProviderType,
                                elementAttribute.TargetType));
                    }
                }

                yield return sectionProviderElement;
            }
        }
    }
#pragma warning restore 1591
}
