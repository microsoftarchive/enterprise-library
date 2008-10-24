//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability
{
	/// <summary>
	/// Represents the configuration information from a 
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.Security.Configuration.CustomSecurityCacheProviderData"/> instance.
	/// </summary>
	/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Configuration.CustomSecurityCacheProviderData"/>
	/// <seealso cref="NamedConfigurationSetting"/>
	/// <seealso cref="ConfigurationSetting"/>	
	[ManagementEntity]
	public partial class CustomSecurityCacheProviderSetting : SecurityCacheProviderSetting
	{
		private string providerType;
		private string[] attributes;

        /// <summary>
        /// Initialize an instance of the <see cref="CustomSecurityCacheProviderSetting"/> class with a configuration source element,
        /// the name of the security cache provider, the provider type and the attributes for the provider.
        /// </summary>
        /// <param name="sourceElement"></param>
        /// <param name="name"></param>
        /// <param name="providerType"></param>
        /// <param name="attributes"></param>
		public CustomSecurityCacheProviderSetting(ConfigurationElement sourceElement, string name, string providerType, string[] attributes)
			: base(sourceElement, name)
		{
			this.providerType = providerType;
			this.attributes = attributes;
		}

		/// <summary>
		/// Gets the name of the type for the custom security cache provider for the represented configuration object.
		/// </summary>
		/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.NameTypeConfigurationElement.Type">
		/// Inherited NameTypeConfigurationElement.Type</seealso>
		[ManagementConfiguration]
		public string ProviderType
		{
			get { return providerType; }
			set { providerType = value; }
		}

		/// <summary>
		/// Gets the collection of attributes for the custom security cache provider represented as a 
		/// <see cref="string"/> array of key/value pairs for the represented configuration object.
		/// </summary>
		/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Configuration.CustomSecurityCacheProviderData.Attributes">
		/// CustomSecurityCacheProviderData.Attributes</seealso>
		[ManagementConfiguration]
		public string[] Attributes
		{
			get { return attributes; }
			set { attributes = value; }
		}

        /// <summary>
        /// Returns an enumeration of the published <see cref="CustomSecurityCacheProviderSetting"/> instances.
        /// </summary>
		[ManagementEnumerator]
		public static IEnumerable<CustomSecurityCacheProviderSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<CustomSecurityCacheProviderSetting>();
		}

        /// <summary>
        /// Returns the <see cref="CustomSecurityCacheProviderSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="CustomSecurityCacheProviderSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static CustomSecurityCacheProviderSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<CustomSecurityCacheProviderSetting>(ApplicationName, SectionName, Name);
		}

        /// <summary>
        /// Saves the changes on the <see cref="CustomSecurityCacheProviderSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return CustomSecurityCacheProviderDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
