//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability
{
    /// <summary>
    /// <para>This type supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
    /// be used directly from your code.</para>
    /// Represents the behavior required to provide Group Policy for the Cryptography Application Block, and it also manages
    /// the creation of the ADM template categories and policies required to edit Group Policy Objects for the block.
    /// </summary>
    /// <remarks>
    /// This class performs the actual Group Policy update for the <see cref="CryptographySettings"/>
    /// configuration section. Processing for <see cref="HashProviderData"/> and <see cref="SymmetricProviderData"/> 
    /// instances is delegated to <see cref="ConfigurationElementManageabilityProvider"/> objects registered to the 
    /// configuration object data types.
    /// </remarks>
    /// <seealso cref="ConfigurationSectionManageabilityProvider"/>
    /// <seealso cref="ConfigurationElementManageabilityProvider"/>
    public sealed class CryptographySettingsManageabilityProvider
        : ConfigurationSectionManageabilityProviderBase<CryptographySettings>
    {
        /// <summary>
        /// The name of the default hash provider property.
        /// </summary>
        public const String DefaultHashProviderPropertyName = "defaultHashInstance";

        /// <summary>
        /// The name of the default symmetric cryptography provider property.
        /// </summary>
        public const String DefaultSymmetricCryptoProviderPropertyName = "defaultSymmetricCryptoInstance";

        /// <summary>
        /// The name of the hash providers property.
        /// </summary>
        public const String HashProvidersKeyName = "hashProviders";

        /// <summary>
        /// The name of the symmetric cryptography providers property.
        /// </summary>
        public const String SymmetricCryptoProvidersKeyName = "symmetricCryptoProviders";

        /// <summary>
        /// <para>This method supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
        /// be used directly from your code.
        /// </para>
        /// Initializes a new instance of the <see cref="CryptographySettingsManageabilityProvider"/> class with a 
        /// given set of manageability providers to use when dealing with the configuration hash and symmetric cryptography providers.
        /// </summary>
        /// <param name="subProviders">The mapping from configuration element type to
        /// <see cref="ConfigurationElementManageabilityProvider"/>.</param>
        public CryptographySettingsManageabilityProvider(IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders)
            : base(subProviders)
        {
        }

        /// <summary>
        /// <para>This method supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
        /// be used directly from your code.</para>
        /// Adds the ADM instructions that describe the policies that can be used to override the configuration
        /// information for the Cryptography Application Block.
        /// </summary>
        /// <seealso cref="ConfigurationSectionManageabilityProvider.AddAdministrativeTemplateDirectives(AdmContentBuilder, ConfigurationSection, IConfigurationSource, String)"/>
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
            CryptographySettings configurationSection,
            IConfigurationSource configurationSource,
            String sectionKey)
        {
            contentBuilder.StartPolicy(Resources.CryptographySettingsPolicyName, sectionKey);
            {
                contentBuilder.AddDropDownListPartForNamedElementCollection<HashProviderData>(Resources.CryptographySettingsDefaultHashProviderPartName,
                    DefaultHashProviderPropertyName,
                    configurationSection.HashProviders,
                    configurationSection.DefaultHashProviderName,
                    true);

                contentBuilder.AddDropDownListPartForNamedElementCollection<SymmetricProviderData>(Resources.CryptographySettingsDefaultSymmetricCryptoProviderPartName,
                    DefaultSymmetricCryptoProviderPropertyName,
                    configurationSection.SymmetricCryptoProviders,
                    configurationSection.DefaultSymmetricCryptoProviderName,
                    true);
            }
            contentBuilder.EndPolicy();

            AddElementsPolicies<HashProviderData>(contentBuilder,
                configurationSection.HashProviders,
                configurationSource,
                sectionKey + @"\" + HashProvidersKeyName,
                Resources.HashProvidersCategoryName);
            AddElementsPolicies<SymmetricProviderData>(contentBuilder,
                configurationSection.SymmetricCryptoProviders,
                configurationSource,
                sectionKey + @"\" + SymmetricCryptoProvidersKeyName,
                Resources.SymmetricCryptoProvidersCategoryName);
        }

        /// <summary>
        /// Gets the name of the category that represents the whole configuration section.
        /// </summary>
        protected override string SectionCategoryName
        {
            get { return Resources.CryptographySectionCategoryName; }
        }

        /// <summary>
        /// Gets the name of the managed configuration section.
        /// </summary>
        protected override string SectionName
        {
            get { return "securityCryptographyConfiguration"; }
        }

        /// <summary>
        /// Overrides the <paramref name="configurationSection"/>'s properties with the Group Policy values from 
        /// the registry.
        /// </summary>
        /// <param name="configurationSection">The configuration section that must be managed.</param>
        /// <param name="policyKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides.</param>
        protected override void OverrideWithGroupPoliciesForConfigurationSection(CryptographySettings configurationSection,
            IRegistryKey policyKey)
        {
            String defaultHashProviderNameOverride
                = GetDefaultProviderPolicyOverride(DefaultHashProviderPropertyName, policyKey);
            String defaultSymmetricCryptoProviderNameOverride
                = GetDefaultProviderPolicyOverride(DefaultSymmetricCryptoProviderPropertyName, policyKey);

            configurationSection.DefaultHashProviderName = defaultHashProviderNameOverride;
            configurationSection.DefaultSymmetricCryptoProviderName = defaultSymmetricCryptoProviderNameOverride;
        }

        private static String GetDefaultProviderPolicyOverride(String propertyName, IRegistryKey policyKey)
        {
            String overrideValue = policyKey.GetStringValue(propertyName);

            return AdmContentBuilder.NoneListItem.Equals(overrideValue) ? String.Empty : overrideValue;
        }

        /// <summary>
        /// Overrides the <paramref name="configurationSection"/>'s configuration elements' properties 
        /// with the Group Policy values from the registry, if any.
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
        protected override void OverrideWithGroupPoliciesForConfigurationElements(CryptographySettings configurationSection,
            bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey)
        {
            OverrideWithGroupPoliciesForElementCollection(configurationSection.HashProviders,
                HashProvidersKeyName,
                readGroupPolicies, machineKey, userKey);
            OverrideWithGroupPoliciesForElementCollection(configurationSection.SymmetricCryptoProviders,
                SymmetricCryptoProvidersKeyName,
                readGroupPolicies, machineKey, userKey);
        }
    }
}
