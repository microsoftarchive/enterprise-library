//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability.Properties;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability
{
    /// <summary>
    /// Provides an implementation for <see cref="DatabaseSettings"/> that performs
    /// policy override processing for the section, performing appropriate logging of
    /// policy processing errors, from policy override processing for configuration objects
    /// contained by the section.
    /// </summary>
    public class DatabaseSettingsManageabilityProvider
        : ConfigurationSectionManageabilityProviderBase<DatabaseSettings>
    {
        /// <summary>
        /// The name of the default database property.
        /// </summary>
        public const String DefaultDatabasePropertyName = "defaultDatabase";

        /// <summary>
        /// The name of the provider mappings property.
        /// </summary>
        public const String ProviderMappingsKeyName = "providerMappings";

        /// <summary>
        /// The name of the database type property.
        /// </summary>
        public const String DatabaseTypePropertyName = "databaseType";

        private static readonly String[] DatabaseTypeNames
            = new String[] { 
				typeof(SqlDatabase).AssemblyQualifiedName, 
				typeof(OracleDatabase).AssemblyQualifiedName,
				typeof(GenericDatabase).AssemblyQualifiedName };

        /// <summary>
        /// Initialize a new instance of the <see cref="DatabaseSettingsManageabilityProvider"/> class with the sub providers.
        /// </summary>
        /// <param name="subProviders">A set of sub providers.</param>
        public DatabaseSettingsManageabilityProvider(IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders)
            : base(subProviders)
        { }

        /// <summary>
        /// Adds the ADM instructions that describe the policies that can be used to override the configuration
        /// information represented by a configuration section.
        /// </summary>
        /// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
        /// <param name="configurationSection">The configuration section instance.</param>
        /// <param name="configurationSource">The configuration source from where to get additional configuration
        /// information, if necessary.</param>
        /// <param name="sectionKey">The root key for the section's policies.</param>
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
            DatabaseSettings configurationSection,
            IConfigurationSource configurationSource, String sectionKey)
        {
            contentBuilder.StartPolicy(Resources.DatabaseSettingsPolicyName, sectionKey);
            {
                List<AdmDropDownListItem> connectionStrings = new List<AdmDropDownListItem>();
                ConnectionStringsSection connectionStringsSection
                    = (ConnectionStringsSection)configurationSource.GetSection("connectionStrings");
                if (connectionStringsSection != null)
                {
                    foreach (ConnectionStringSettings connectionString in connectionStringsSection.ConnectionStrings)
                    {
                        connectionStrings.Add(new AdmDropDownListItem(connectionString.Name, connectionString.Name));
                    }
                }
                contentBuilder.AddDropDownListPart(Resources.DatabaseSettingsDefaultDatabasePartName,
                    DefaultDatabasePropertyName,
                    connectionStrings,
                    configurationSection.DefaultDatabase);
            }
            contentBuilder.EndPolicy();

            if (configurationSection.ProviderMappings.Count > 0)
            {
                contentBuilder.StartCategory(Resources.ProviderMappingsCategoryName);
                {
                    foreach (DbProviderMapping providerMapping in configurationSection.ProviderMappings)
                    {
                        contentBuilder.StartPolicy(String.Format(CultureInfo.InvariantCulture,
                                                                Resources.ProviderMappingPolicyNameTemplate,
                                                                providerMapping.Name),
                            sectionKey + @"\" + ProviderMappingsKeyName + @"\" + providerMapping.Name);

                        contentBuilder.AddComboBoxPart(Resources.ProviderMappingDatabaseTypePartName,
                            DatabaseTypePropertyName,
                            providerMapping.DatabaseType.AssemblyQualifiedName,
                            255,
                            false,
                            DatabaseTypeNames);

                        contentBuilder.EndPolicy();
                    }
                }
                contentBuilder.EndCategory();
            }
        }

        /// <summary>
        /// Gets the name of the category that represents the whole configuration section.
        /// </summary>
        protected override string SectionCategoryName
        {
            get { return Resources.DatabaseCategoryName; }
        }

        /// <summary>
        /// Gets the name of the managed configuration section.
        /// </summary>
        protected override string SectionName
        {
            get { return DatabaseSettings.SectionName; }
        }

        /// <summary>
        /// Overrides the <paramref name="configurationSection"/>'s properties with the Group Policy values from 
        /// the registry.
        /// </summary>
        /// <param name="configurationSection">The configuration section that must be managed.</param>
        /// <param name="policyKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides.</param>
        protected override void OverrideWithGroupPoliciesForConfigurationSection(DatabaseSettings configurationSection,
            IRegistryKey policyKey)
        {
            String defaultDatabaseOverride = policyKey.GetStringValue(DefaultDatabasePropertyName);

            configurationSection.DefaultDatabase = defaultDatabaseOverride;
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
        protected override void OverrideWithGroupPoliciesForConfigurationElements(DatabaseSettings configurationSection,
            bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey)
        {
            List<DbProviderMapping> mappingsToRemove = new List<DbProviderMapping>();

            IRegistryKey machineMappingsKey = null;
            IRegistryKey userMappingsKey = null;

            try
            {
                LoadRegistrySubKeys(ProviderMappingsKeyName,
                    machineKey, userKey,
                    out machineMappingsKey, out userMappingsKey);

                foreach (DbProviderMapping providerMapping in configurationSection.ProviderMappings)
                {
                    IRegistryKey machineMappingKey = null;
                    IRegistryKey userMappingKey = null;

                    try
                    {
                        LoadRegistrySubKeys(providerMapping.Name,
                            machineMappingsKey, userMappingsKey,
                            out machineMappingKey, out userMappingKey);

                        if (!OverrideWithGroupPoliciesForDbProviderMapping(providerMapping,
                                readGroupPolicies, machineMappingKey, userMappingKey))
                        {
                            mappingsToRemove.Add(providerMapping);
                        }
                    }
                    finally
                    {
                        ReleaseRegistryKeys(machineMappingKey, userMappingKey);
                    }
                }
            }
            finally
            {
                ReleaseRegistryKeys(machineMappingsKey, userMappingsKey);
            }

            foreach (DbProviderMapping providerMapping in mappingsToRemove)
            {
                configurationSection.ProviderMappings.Remove(providerMapping.Name);
            }
        }

        private bool OverrideWithGroupPoliciesForDbProviderMapping(DbProviderMapping providerMapping,
            bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey)
        {
            if (readGroupPolicies)
            {
                IRegistryKey policyKey = machineKey ?? userKey;
                if (policyKey != null)
                {
                    if (policyKey.IsPolicyKey && !policyKey.GetBoolValue(PolicyValueName).Value)
                    {
                        return false;
                    }
                    try
                    {
                        Type databaseTypeOverride = policyKey.GetTypeValue(DatabaseTypePropertyName);

                        providerMapping.DatabaseType = databaseTypeOverride;
                    }
                    catch (RegistryAccessException ex)
                    {
                        LogExceptionWhileOverriding(ex);
                    }
                }
            }

            return true;
        }
    }
}
