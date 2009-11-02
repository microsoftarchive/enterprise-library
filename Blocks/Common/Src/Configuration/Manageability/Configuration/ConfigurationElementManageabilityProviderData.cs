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
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration
{
	/// <summary>
	/// Represents the configuration settings that describe an <see cref="ConfigurationElementManageabilityProvider"/>.
	/// </summary>
    [ResourceDescription(typeof(DesignResources), "ConfigurationElementManageabilityProviderDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ConfigurationElementManageabilityProviderDataDisplayName")]
	public class ConfigurationElementManageabilityProviderData : NameTypeConfigurationElement
	{
		private const string targetTypePropertyName = "targetType";

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationElementManageabilityProviderData"/> class with default values.
		/// </summary>
		public ConfigurationElementManageabilityProviderData()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationElementManageabilityProviderData"/> class.
		/// </summary>
		/// <param name="name">The name of the configuration element.</param>
		/// <param name="providerType">The <see cref="ConfigurationElementManageabilityProvider"/> type.</param>
		/// <param name="targetType">The <see cref="NamedConfigurationElement"/> type that is managed by the provider type.</param>
		public ConfigurationElementManageabilityProviderData(String name, Type providerType, Type targetType)
			: base(name, providerType)
		{
			this.TargetType = targetType;
		}

		/// <summary>
		/// Gets or sets the <see cref="NamedConfigurationElement"/> type that is managed by the provider type.
		/// </summary>
		[ConfigurationProperty(targetTypePropertyName, IsRequired = true)]
		[TypeConverter(typeof(AssemblyQualifiedTypeNameConverter))]
        [ResourceDescription(typeof(DesignResources), "ConfigurationElementManageabilityProviderDataTargetTypeDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ConfigurationElementManageabilityProviderDataTargetTypeDisplayName")]
		public Type TargetType
		{
			get { return (Type)base[targetTypePropertyName]; }
			set { base[targetTypePropertyName] = value; }
		}

		internal ConfigurationElementManageabilityProvider CreateManageabilityProvider()
		{
			return (ConfigurationElementManageabilityProvider)Activator.CreateInstance(this.Type);
		}
	}
}
