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

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability
{
    /// <summary>
    /// Represents a mapper from database settings configuration to Wmi.
    /// </summary>
	public static class DatabaseSettingsWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="DatabaseBlockSetting"/> instances that describe the configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(DatabaseSettings configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(new DatabaseBlockSetting(configurationObject, configurationObject.DefaultDatabase));
		}

        /// <summary>
        /// Save changes maded to a <see cref="DatabaseBlockSetting"/> instance
        /// </summary>
        /// <param name="setting">The configuration object.</param>
        /// <param name="sourceElement"><see cref="ConfigurationElement"/> with the changes.</param>
		public static bool SaveChanges(DatabaseBlockSetting setting, ConfigurationElement sourceElement)
		{
			DatabaseSettings section = (DatabaseSettings)sourceElement;

			section.DefaultDatabase = setting.DefaultDatabase;

			return true;
		}

        /// <summary>
        /// Creates the <see cref="ProviderMappingSetting"/> instances that describe the configurationObject.
        /// </summary>
        /// <param name="providerMapping">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateDbProviderMappingWmiObjects(DbProviderMapping providerMapping,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new ProviderMappingSetting(providerMapping,
					providerMapping.DbProviderName,
					providerMapping.DatabaseType.AssemblyQualifiedName));
		}

        /// <summary>
        /// Save changes maded to a <see cref="ProviderMappingSetting"/> instance
        /// </summary>
        /// <param name="providerMappingSetting">The configuration object.</param>
        /// <param name="sourceElement"><see cref="ConfigurationElement"/> with the changes.</param>
		public static bool SaveChanges(ProviderMappingSetting providerMappingSetting, ConfigurationElement sourceElement)
		{
			DbProviderMapping element = (DbProviderMapping)sourceElement;

			element.DatabaseTypeName = providerMappingSetting.DatabaseType;

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(DatabaseBlockSetting),
				typeof(ProviderMappingSetting));
		}
	}
}
