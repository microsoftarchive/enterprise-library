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

using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability
{
	/// <summary>
	/// <para>This type supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
	/// be used directly from your code.</para>
	/// Represents the behavior required to provide Group Policy updates and to publish the <see cref="ConfigurationSetting"/> 
	/// instances associated to the configuration information for the Security Application Block, and it also manages
	/// the creation of the ADM template categories and policies required to edit Group Policy Objects for the block.
	/// </summary>
	/// <remarks>
	/// This class performs the actual Group Policy update and Wmi object generation for the <see cref="SecuritySettings"/>
	/// configuration section. Processing for <see cref="AuthorizationProviderData"/> and <see cref="SecurityCacheProviderData"/> 
	/// instances is delegated to <see cref="ConfigurationElementManageabilityProvider"/> objects registered to the 
	/// configuration object data types.
	/// </remarks>
	/// <seealso cref="ConfigurationSectionManageabilityProvider"/>
	/// <seealso cref="ConfigurationElementManageabilityProvider"/>
	public sealed class SecuritySettingsManageabilityProvider
		: ConfigurationSectionManageabilityProviderBase<SecuritySettings>
	{
        /// <summary>
        /// The name of the default authorization provider property.
        /// </summary>
		public const String DefaultAuthorizationProviderPropertyName = "defaultAuthorizationInstance";

        /// <summary>
        /// The name of the default security cache provider property.
        /// </summary>
		public const String DefaultSecurityCacheProviderPropertyName = "defaultSecurityCacheInstance";

        /// <summary>
        /// The name of the authorization provider key property.
        /// </summary>
		public const String AuthorizationProvidersKeyName = "authorizationProviders";

        /// <summary>
        /// The name of the security cache provider key provider.
        /// </summary>
		public const String SecurityCacheProvidersKeyName = "securityCacheProviders";

		/// <summary>
		/// <para>This method supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
		/// be used directly from your code.
		/// </para>
		/// Initializes a new instance of the <see cref="SecuritySettingsManageabilityProvider"/> class with a 
		/// given set of manageability providers to use when dealing with the configuration authorization and security cache providers.
		/// </summary>
		/// <param name="subProviders">The mapping from configuration element type to
		/// <see cref="ConfigurationElementManageabilityProvider"/>.</param>
		public SecuritySettingsManageabilityProvider(IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders)
			: base(subProviders)
		{
			SecuritySettingsWmiMapper.RegisterWmiTypes();
		}

		/// <summary>
		/// <para>This method supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
		/// be used directly from your code.</para>
		/// Adds the ADM instructions that describe the policies that can be used to override the configuration
		/// information for the Security Application Block.
		/// </summary>
		/// <seealso cref="ConfigurationSectionManageabilityProvider.AddAdministrativeTemplateDirectives(AdmContentBuilder, ConfigurationSection, IConfigurationSource, String)"/>
		protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			SecuritySettings configurationSection,
			IConfigurationSource configurationSource,
			String sectionKey)
		{
			contentBuilder.StartPolicy(Resources.SecuritySettingsPolicyName, sectionKey);
			{
				contentBuilder.AddDropDownListPartForNamedElementCollection<AuthorizationProviderData>(Resources.SecuritySettingsDefaultAuthorizationProviderPartName,
					DefaultAuthorizationProviderPropertyName,
					configurationSection.AuthorizationProviders,
					configurationSection.DefaultAuthorizationProviderName,
					true);

				contentBuilder.AddDropDownListPartForNamedElementCollection<SecurityCacheProviderData>(Resources.SecuritySettingsDefaultSecurityCacheProviderPartName,
					DefaultSecurityCacheProviderPropertyName,
					configurationSection.SecurityCacheProviders,
					configurationSection.DefaultSecurityCacheProviderName,
					true);
			}
			contentBuilder.EndPolicy();

			AddElementsPolicies<AuthorizationProviderData>(contentBuilder,
				configurationSection.AuthorizationProviders,
				configurationSource,
				sectionKey + @"\" + AuthorizationProvidersKeyName,
				Resources.AuthorizationProvidersCategoryName);
			AddElementsPolicies<SecurityCacheProviderData>(contentBuilder,
				configurationSection.SecurityCacheProviders,
				configurationSource,
				sectionKey + @"\" + SecurityCacheProvidersKeyName,
				Resources.SecurityCacheProvidersCategoryName);
		}

		/// <summary>
		/// Gets the name of the category that represents the whole configuration section.
		/// </summary>
		protected override string SectionCategoryName
		{
			get { return Resources.SecuritySectionCategoryName; }
		}

		/// <summary>
		/// Gets the name of the managed configuration section.
		/// </summary>
		protected override string SectionName
		{
			get { return SecuritySettings.SectionName; }
		}

		/// <summary>
		/// Overrides the <paramref name="configurationSection"/>'s properties with the Group Policy values from 
		/// the registry.
		/// </summary>
		/// <param name="configurationSection">The configuration section that must be managed.</param>
		/// <param name="policyKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides.</param>
		protected override void OverrideWithGroupPoliciesForConfigurationSection(SecuritySettings configurationSection,
			IRegistryKey policyKey)
		{
			String defaultAuthorizationProviderNameOverride
				= GetDefaultProviderPolicyOverride(DefaultAuthorizationProviderPropertyName, policyKey);
			String defaultSecurityCacheProviderNameOverride
				= GetDefaultProviderPolicyOverride(DefaultSecurityCacheProviderPropertyName, policyKey);

			configurationSection.DefaultAuthorizationProviderName = defaultAuthorizationProviderNameOverride;
			configurationSection.DefaultSecurityCacheProviderName = defaultSecurityCacheProviderNameOverride;
		}

		private static String GetDefaultProviderPolicyOverride(String propertyName, IRegistryKey policyKey)
		{
			String overrideValue = policyKey.GetStringValue(propertyName);

			return AdmContentBuilder.NoneListItem.Equals(overrideValue) ? String.Empty : overrideValue;
		}

		/// <summary>
		/// Overrides the <paramref name="configurationSection"/>'s configuration elements' properties 
		/// with the Group Policy values from the registry, if any, and creates the <see cref="ConfigurationSetting"/> 
		/// instances that describe these configuration elements.
		/// </summary>
		/// <param name="configurationSection">The configuration section that must be managed.</param>
		/// <param name="readGroupPolicies"><see langword="true"/> if Group Policy overrides must be applied; otherwise, 
		/// <see langword="false"/>.</param>
		/// <param name="machineKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides for the 
		/// configuration section at the machine level, or <see langword="null"/> 
		/// if there is no such registry key.</param>
		/// <param name="userKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides for the 
		/// configuration section at the user level, or <see langword="null"/> 
		/// if there is no such registry key.</param>
		/// <param name="generateWmiObjects"><see langword="true"/> if WMI objects must be generated; otherwise, 
		/// <see langword="false"/>.</param>
		/// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		protected override void OverrideWithGroupPoliciesAndGenerateWmiObjectsForConfigurationElements(SecuritySettings configurationSection,
			bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey,
			bool generateWmiObjects, ICollection<ConfigurationSetting> wmiSettings)
		{
			OverrideWithGroupPoliciesAndGenerateWmiObjectsForElementCollection(configurationSection.AuthorizationProviders,
				AuthorizationProvidersKeyName,
				readGroupPolicies, machineKey, userKey,
				generateWmiObjects, wmiSettings);
			OverrideWithGroupPoliciesAndGenerateWmiObjectsForElementCollection(configurationSection.SecurityCacheProviders,
				SecurityCacheProvidersKeyName,
				readGroupPolicies, machineKey, userKey,
				generateWmiObjects, wmiSettings);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="configurationSection"></param>
		/// <param name="wmiSettings"></param>
		protected override void GenerateWmiObjectsForConfigurationSection(SecuritySettings configurationSection, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			SecuritySettingsWmiMapper.GenerateWmiObjects(configurationSection, wmiSettings);
		}
	}
}
