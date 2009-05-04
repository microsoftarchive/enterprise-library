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
    /// Representa a mapper for connection strings in configuration to Wmi.
    /// </summary>
	public static class ConnectionStringsWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="ConnectionStringSettings"/> instances that describe the configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(ConnectionStringsSection configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			// no settings for the section
		}

        /// <summary>
        /// Creates the <see cref="ConnectionStringSettings"/> instances that describe the configurationObject.
        /// </summary>
        /// <param name="connectionString">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateConnectionStringWmiObjects(ConnectionStringSettings connectionString,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new ConnectionStringSetting(connectionString,
					connectionString.Name,
					connectionString.ConnectionString,
					connectionString.ProviderName));
		}

        /// <summary>
        /// Save changes maded to a <see cref="ConnectionStringSettings"/> instance
        /// </summary>
        /// <param name="setting">The configuration object.</param>
        /// <param name="sourceElement"><see cref="ConfigurationElement"/> with the changes.</param>
		public static bool SaveChanges(ConnectionStringSetting setting, ConfigurationElement sourceElement)
		{
			ConnectionStringSettings element = (ConnectionStringSettings)sourceElement;

			element.ConnectionString = setting.ConnectionString;
			element.ProviderName = setting.ProviderName;

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(ConnectionStringSetting));
		}
	}
}
