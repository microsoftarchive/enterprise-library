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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability
{
    /// <summary>
    /// Represents a mapper for Oracle connection settings in configuration to Wmi.
    /// </summary>
	public static class OracleConnectionSettingsWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="OracleConnectionSettings"/> instances that describe the configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(OracleConnectionSettings configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			// no section level objects
		}

        /// <summary>
        /// Creates the <see cref="OracleConnectionSetting"/> instances that describe the configurationObject.
        /// </summary>
        /// <param name="connectionData">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateOracleConnectionSettingWmiObjects(OracleConnectionData connectionData,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			string[] packages = GeneratePackagesArray(connectionData.Packages);

			wmiSettings.Add(new OracleConnectionSetting(connectionData, connectionData.Name, packages));
		}

		private static string[] GeneratePackagesArray(NamedElementCollection<OraclePackageData> packages)
		{
			string[] packagesArray = new string[packages.Count];
			int i = 0;
			foreach (OraclePackageData package in packages)
			{
				packagesArray[i++]
					= KeyValuePairEncoder.EncodeKeyValuePair(package.Name, package.Prefix);
			}
			return packagesArray;
		}

		/// <summary>
        /// Save changes maded to a <see cref="OracleConnectionSetting"/> instance
		/// </summary>
		/// <param name="setting">The configuration object.</param>
        /// <param name="sourceElement"><see cref="ConfigurationElement"/> with the changes.</param>
		public static bool SaveChanges(OracleConnectionSetting setting, ConfigurationElement sourceElement)
		{
			OracleConnectionData element = (OracleConnectionData)sourceElement;

			Dictionary<String, String> packagesDictionary = new Dictionary<string, string>();
			foreach (string encodedKeyValuePair in setting.Packages)
			{
				KeyValuePairParser.ExtractKeyValueEntries(encodedKeyValuePair, packagesDictionary);
			}

			element.Packages.Clear();
			foreach (KeyValuePair<String, String> kvp in packagesDictionary)
			{
				element.Packages.Add(new OraclePackageData(kvp.Key, kvp.Value));
			}

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(OracleConnectionSetting));
		}
	}
}
