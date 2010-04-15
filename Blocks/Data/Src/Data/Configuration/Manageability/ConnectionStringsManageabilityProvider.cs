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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability
{
    /// <summary>
    /// Provides an implementation for <see cref="ConnectionStringsSection"/> that performs
    /// policy override processing for the section, performing appropriate logging of
    /// policy processing errors, from policy override processing for configuration objects
    /// contained by the section.
    /// </summary>
    public class ConnectionStringsManageabilityProvider
        : ConfigurationSectionManageabilityProviderBase<ConnectionStringsSection>
    {
        /// <summary>
        /// The name of the connection string property.
        /// </summary>
        public const String ConnectionStringPropertyName = "connectionString";

        /// <summary>
        /// The name of the provider property.
        /// </summary>
        public const String ProviderNamePropertyName = "providerName";

        /// <summary>
        /// Initialize a new instance of the <see cref="ConnectionStringsManageabilityProvider"/> class with the sub providers.
        /// </summary>
        /// <param name="subProviders">A set of sub providers.</param>
        public ConnectionStringsManageabilityProvider(IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders)
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
            ConnectionStringsSection configurationSection,
            IConfigurationSource configurationSource,
            String sectionKey)
        {
            contentBuilder.StartCategory(Resources.ConnectionStringsCategoryName);
            {
                foreach (ConnectionStringSettings connectionString in configurationSection.ConnectionStrings)
                {
                    contentBuilder.StartPolicy(String.Format(CultureInfo.InvariantCulture,
                                                            Resources.ConnectionStringPolicyNameTemplate,
                                                            connectionString.Name),
                        sectionKey + @"\" + connectionString.Name);

                    contentBuilder.AddEditTextPart(Resources.ConnectionStringConnectionStringPartName,
                        ConnectionStringPropertyName,
                        connectionString.ConnectionString,
                        500,
                        true);

                    contentBuilder.AddComboBoxPart(Resources.ConnectionStringProviderNamePartName,
                        ProviderNamePropertyName,
                        connectionString.ProviderName,
                        255,
                        true,
                        "System.Data.SqlClient",
                        "System.Data.OracleClient");

                    contentBuilder.EndPolicy();
                }
            }
            contentBuilder.EndCategory();
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
            get { return "connectionStrings"; }
        }

        /// <summary>
        /// Overrides the <paramref name="configurationSection"/>'s properties with the Group Policy values from 
        /// the registry.
        /// </summary>
        /// <param name="configurationSection">The configuration section that must be managed.</param>
        /// <param name="policyKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides.</param>
        protected override void OverrideWithGroupPoliciesForConfigurationSection(ConnectionStringsSection configurationSection,
            IRegistryKey policyKey)
        {
            // no section values to override
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
        protected override void OverrideWithGroupPoliciesForConfigurationElements(ConnectionStringsSection configurationSection,
            bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey)
        {
            List<ConnectionStringSettings> elementsToRemove = new List<ConnectionStringSettings>();

            foreach (ConnectionStringSettings connectionString in configurationSection.ConnectionStrings)
            {
                IRegistryKey machineOverrideKey = null;
                IRegistryKey userOverrideKey = null;

                try
                {
                    LoadRegistrySubKeys(connectionString.Name, machineKey, userKey, out machineOverrideKey, out userOverrideKey);

                    if (!OverrideWithGroupPoliciesForConnectionString(connectionString,
                            readGroupPolicies, machineOverrideKey, userOverrideKey))
                    {
                        elementsToRemove.Add(connectionString);
                    }
                }
                finally
                {
                    ReleaseRegistryKeys(machineOverrideKey, userOverrideKey);
                }
            }

            foreach (ConnectionStringSettings connectionString in elementsToRemove)
            {
                configurationSection.ConnectionStrings.Remove(connectionString);
            }
        }

        private bool OverrideWithGroupPoliciesForConnectionString(ConnectionStringSettings connectionString,
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
                        String connectionStringOverride = policyKey.GetStringValue(ConnectionStringPropertyName);
                        String providerNameOverride = policyKey.GetStringValue(ProviderNamePropertyName);

                        connectionString.ConnectionString = connectionStringOverride;
                        connectionString.ProviderName = providerNameOverride;
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
