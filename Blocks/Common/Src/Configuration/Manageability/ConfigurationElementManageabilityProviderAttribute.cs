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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	/// <summary>
	/// Specifies which <see cref="ConfigurationElementManageabilityProvider"/> must be used to provide manageability
	/// for instances a given <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.NamedConfigurationElement"/> subclass.
	/// </summary>
	/// <remarks>
	/// Manageability providers for configuration elements are registered both to the configuration element type 
	/// and the manageability provider for the configuration section where the configuration element resides.
	/// The attribute is bound to assemblies.
	/// </remarks>
	/// <seealso cref="ConfigurationElementManageabilityProvider"/>
	/// <seealso cref="ConfigurationSectionManageabilityProvider"/>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class ConfigurationElementManageabilityProviderAttribute : Attribute
	{
		private Type manageabilityProviderType;
		private Type targetType;
		private Type sectionManageabilityProviderType;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationElementManageabilityProviderAttribute"/> class.
		/// </summary>
		/// <param name="manageabilityProviderType">The <see cref="ConfigurationElementManageabilityProvider"/> type.</param>
		/// <param name="targetType">The <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.NamedConfigurationElement"/> type.
		/// </param>
		/// <param name="sectionManageabilityProviderType">The <see cref="ConfigurationSectionManageabilityProvider"/> type.</param>
		public ConfigurationElementManageabilityProviderAttribute(Type manageabilityProviderType, Type targetType, Type sectionManageabilityProviderType)
		{
			this.manageabilityProviderType = manageabilityProviderType;
			this.targetType = targetType;
			this.sectionManageabilityProviderType = sectionManageabilityProviderType;
		}

		/// <summary>
		/// Gets the registered <see cref="ConfigurationElementManageabilityProvider"/> type.
		/// </summary>
		public Type ManageabilityProviderType
		{
			get { return manageabilityProviderType; }
		}

		/// <summary>
		/// Gets the <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.NamedConfigurationElement"/> type 
		/// for which the registered <see cref="ConfigurationElementManageabilityProvider"/> type provides manageability.
		/// </summary>
		public Type TargetType
		{
			get { return targetType; }
		}

		/// <summary>
		/// Gets the <see cref="ConfigurationSectionManageabilityProvider"/> registered to manage the 
		/// section where the <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.NamedConfigurationElement"/> 
		/// instances managed by the registered <see cref="ConfigurationElementManageabilityProvider"/> type reside.
		/// </summary>
		public Type SectionManageabilityProviderType
		{
			get { return sectionManageabilityProviderType; }
		}
	}
}
