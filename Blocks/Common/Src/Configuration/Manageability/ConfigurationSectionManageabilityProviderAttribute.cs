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
	/// Specifies which <see cref="ConfigurationSectionManageabilityProvider"/> must be used to provide manageability
	/// for a configuration section.
	/// </summary>
	/// <remarks>
	/// Manageability providers for configuration sections are registered to configuration section name.
	/// The attribute is bound to assemblies.
	/// </remarks>
	/// <seealso cref="ConfigurationSectionManageabilityProvider"/>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class ConfigurationSectionManageabilityProviderAttribute : Attribute
	{
		private string sectionName;
		private Type manageabilityProviderType;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationElementManageabilityProviderAttribute"/> class.
		/// </summary>
		/// <param name="sectionName">The name of the section that needs manageability.</param>
		/// <param name="manageabilityProviderType">The <see cref="ConfigurationSectionManageabilityProvider"/> type.</param>
		public ConfigurationSectionManageabilityProviderAttribute(string sectionName, Type manageabilityProviderType)
		{
			this.sectionName = sectionName;
			this.manageabilityProviderType = manageabilityProviderType;
		}

		/// <summary>
		/// Gets the name of the <see cref="System.Configuration.ConfigurationSection"/> for which the registered 
		/// <see cref="ConfigurationElementManageabilityProvider"/> type provides manageability.
		/// </summary>
		public string SectionName
		{
			get { return sectionName; }
		}

		/// <summary>
		/// Gets the registered <see cref="ConfigurationSectionManageabilityProvider"/> type.
		/// </summary>
		public Type ManageabilityProviderType
		{
			get { return manageabilityProviderType; }
		}
	}
}
