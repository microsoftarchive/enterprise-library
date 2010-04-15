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
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration
{
	/// <summary>
	/// Oracle-specific configuration section.
	/// </summary>
    [ResourceDescription(typeof(DesignResources), "OracleConnectionSettingsDescription")]
    [ResourceDisplayName(typeof(DesignResources), "OracleConnectionSettingsDisplayName")]
	public class OracleConnectionSettings : SerializableConfigurationSection
	{
		private const string oracleConnectionDataCollectionProperty = "";

		/// <summary>
		/// The section name for the <see cref="OracleConnectionSettings"/>.
		/// </summary>
		public const string SectionName = "oracleConnectionSettings";

		/// <summary>
		/// Initializes a new instance of the <see cref="OracleConnectionSettings"/> class with default values.
		/// </summary>
		public OracleConnectionSettings()
		{
		}

		/// <summary>
		/// Retrieves the <see cref="OracleConnectionSettings"/> from the configuration source.
		/// </summary>
		/// <param name="configurationSource">The configuration source to retrieve the configuration from.</param>
		/// <returns>The configuration section, or <see langword="null"/> (<b>Nothing</b> in Visual Basic) 
		/// if not present in the configuration source.</returns>
		public static OracleConnectionSettings GetSettings(IConfigurationSource configurationSource)
		{
		    if (configurationSource == null) throw new ArgumentNullException("configurationSource");

			return configurationSource.GetSection(SectionName) as OracleConnectionSettings;
		}

		/// <summary>
		/// Collection of Oracle-specific connection information.
		/// </summary>
		[ConfigurationProperty(oracleConnectionDataCollectionProperty, IsRequired=false, IsDefaultCollection=true)]
		[ConfigurationCollection(typeof(OracleConnectionData))]
        [ResourceDescription(typeof(DesignResources), "OracleConnectionSettingsOracleConnectionsDataDescription")]
        [ResourceDisplayName(typeof(DesignResources), "OracleConnectionSettingsOracleConnectionsDataDisplayName")]
        public NamedElementCollection<OracleConnectionData> OracleConnectionsData
		{
			get { return (NamedElementCollection<OracleConnectionData>)base[oracleConnectionDataCollectionProperty]; }
		}
	}
}
