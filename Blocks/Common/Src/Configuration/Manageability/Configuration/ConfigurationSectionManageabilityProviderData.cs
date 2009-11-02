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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration
{
	/// <summary>
	/// Represents the configuration settings that describe an <see cref="ConfigurationSectionManageabilityProvider"/>.
	/// </summary>
    [ResourceDescription(typeof(DesignResources), "ConfigurationSectionManageabilityProviderDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ConfigurationSectionManageabilityProviderDataDisplayName")]
	public class ConfigurationSectionManageabilityProviderData : NameTypeConfigurationElement
	{
		private const String manageabilityProvidersCollectionPropertyName = "manageabilityProviders";

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationSectionManageabilityProviderData"/> class with default values.
		/// </summary>
		public ConfigurationSectionManageabilityProviderData()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationElementManageabilityProviderData"/> class.
		/// </summary>
		/// <param name="sectionName">The name for the configuration section that is managed by the provider type.</param>
		/// <param name="providerType">The <see cref="ConfigurationSectionManageabilityProvider"/> type.</param>
		public ConfigurationSectionManageabilityProviderData(String sectionName, Type providerType)
			: base(sectionName, providerType)
		{ }

		/// <summary>
		/// Gets the collection of <see cref="ConfigurationElementManageabilityProviderData"/> that represent
		/// the <see cref="ConfigurationElementManageabilityProvider"/> instances that the 
		/// <see cref="ConfigurationSectionManageabilityProvider"/> instance represented by the receiver might require
		/// to provide manageability to configuration elements.
		/// </summary>
		[ConfigurationProperty(manageabilityProvidersCollectionPropertyName)]
        [ConfigurationCollection(typeof(ConfigurationElementManageabilityProviderData))]
        [ResourceDescription(typeof(DesignResources), "ConfigurationSectionManageabilityProviderDataManageabilityProvidersDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ConfigurationSectionManageabilityProviderDataManageabilityProvidersDisplayName")]
		public NamedElementCollection<ConfigurationElementManageabilityProviderData> ManageabilityProviders
		{
			get { return (NamedElementCollection<ConfigurationElementManageabilityProviderData>)base[manageabilityProvidersCollectionPropertyName]; }
		}

		internal ConfigurationSectionManageabilityProvider CreateManageabilityProvider()
		{
			IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
			foreach (ConfigurationElementManageabilityProviderData data in this.ManageabilityProviders)
			{
				ConfigurationElementManageabilityProvider subManageabilityProvider = data.CreateManageabilityProvider();
				subProviders.Add(data.TargetType, subManageabilityProvider);
			}

			return (ConfigurationSectionManageabilityProvider)Activator.CreateInstance(this.Type, subProviders);
		}
	}
}
