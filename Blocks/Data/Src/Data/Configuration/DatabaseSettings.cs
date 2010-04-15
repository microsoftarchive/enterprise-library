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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
	/// <summary>
	/// <para>Represents the root configuration for data.</para>
	/// </summary>
	/// <remarks>
	/// <para>The class maps to the <c>databaseSettings</c> element in configuration.</para>
	/// </remarks>
    [ResourceDescription(typeof(DesignResources), "DatabaseSettingsDescription")]
    [ResourceDisplayName(typeof(DesignResources), "DatabaseSettingsDisplayName")]
	public class DatabaseSettings : SerializableConfigurationSection
	{
		private const string defaultDatabaseProperty = "defaultDatabase";
		private const string dbProviderMappingsProperty = "providerMappings";

		/// <summary>
		/// The name of the data configuration section.
		/// </summary>
		public const string SectionName = "dataConfiguration";

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="DatabaseSettings"/> class.</para>
		/// </summary>
		public DatabaseSettings()
			: base()
		{
		}

		/// <summary>
		/// Retrieves the <see cref="DatabaseSettings"/> from a configuration source.
		/// </summary>
		/// <param name="configurationSource">The <see cref="IConfigurationSource"/> to query for the database settings.</param>
		/// <returns>The database settings from the configuration source, or <see langword="null"/> (<b>Nothing</b> in Visual Basic) if the 
		/// configuration source does not contain database settings.</returns>
		public static DatabaseSettings GetDatabaseSettings(IConfigurationSource configurationSource)
		{
		    if (configurationSource == null) throw new ArgumentNullException("configurationSource");

			return (DatabaseSettings)configurationSource.GetSection(SectionName);
		}

		/// <summary>
		/// <para>Gets or sets the default database instance name.</para>
		/// </summary>
		/// <value>
		/// <para>The default database instance name.</para>
		/// </value>
		/// <remarks>
		/// <para>This property maps to the <c>defaultInstance</c> element in configuration.</para>
		/// </remarks>
		[ConfigurationProperty(defaultDatabaseProperty, IsRequired = false)]
        [Reference(typeof(ConnectionStringSettingsCollection), typeof(ConnectionStringSettings))]
        [ResourceDescription(typeof(DesignResources), "DatabaseSettingsDefaultDatabaseDescription")]
        [ResourceDisplayName(typeof(DesignResources), "DatabaseSettingsDefaultDatabaseDisplayName")]
		public string DefaultDatabase
		{
			get
			{
				return (string)this[defaultDatabaseProperty];
			}
			set
			{
				this[defaultDatabaseProperty] = value;
			}
		}

		/// <summary>
		/// Holds the optional mappings from ADO.NET's database providers to Enterprise Library's database types.
		/// </summary>
		/// <seealso cref="DbProviderMapping"/>
		[ConfigurationProperty(dbProviderMappingsProperty, IsRequired = false)]
        [ConfigurationCollection(typeof(DbProviderMapping))]
        [ResourceDescription(typeof(DesignResources), "DatabaseSettingsProviderMappingsDescription")]
        [ResourceDisplayName(typeof(DesignResources), "DatabaseSettingsProviderMappingsDisplayName")]
		public NamedElementCollection<DbProviderMapping> ProviderMappings
		{
			get
			{
				return (NamedElementCollection<DbProviderMapping>)base[dbProviderMappingsProperty];
			}
		}
	}
}
