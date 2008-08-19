//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
	/// <summary>
	/// <para>Represents a view for navigating the <see cref="DatabaseSettings"/> configuration data.</para>
	/// </summary>
	public class DatabaseConfigurationView
	{
		private static readonly DbProviderMapping defaultSqlMapping = new DbProviderMapping(DbProviderMapping.DefaultSqlProviderName, typeof(SqlDatabase));
		private static readonly DbProviderMapping defaultOracleMapping = new DbProviderMapping(DbProviderMapping.DefaultOracleProviderName, typeof(OracleDatabase));
		private static readonly DbProviderMapping defaultGenericMapping = new DbProviderMapping(DbProviderMapping.DefaultGenericProviderName, typeof(GenericDatabase));

		private readonly IConfigurationSource configurationSource;

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="DatabaseConfigurationView"/> class with an <see cref="IConfigurationSource"/> object.</para>
		/// </summary>
		/// <param name="configurationSource">
		/// <para>A <see cref="IConfigurationSource"/> object.</para>
		/// </param>
		public DatabaseConfigurationView(IConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
		}

		/// <summary>
		/// <para>Gets the <see cref="DatabaseSettings"/> configuration data.</para>
		/// </summary>
		/// <returns>
		/// <para>The <see cref="DatabaseSettings"/> configuration data.</para>
		/// </returns>
		public DatabaseSettings DatabaseSettings
		{
			get { return (DatabaseSettings)configurationSource.GetSection(DatabaseSettings.SectionName); }
		}

		/// <summary>
		/// <para>Gets the name of the default configured <see cref="Database"/>.</para>
		/// </summary>
		/// <returns>
		/// <para>The name of the default configured <see cref="Database"/>.</para>
		/// </returns>
		public string DefaultName
		{
			get
			{
				DatabaseSettings settings = this.DatabaseSettings;
				string databaseName = settings != null ? settings.DefaultDatabase : null;
				return databaseName;
			}
		}

		/// <summary>
		/// Returns the <see cref="ConnectionStringSettings"/> object with the given name from the connection strings
		/// configuration section in the receiver's configuration source.
		/// </summary>
		/// <remarks>
		/// The connection string will be retrieved from the configuration source if it contains the connection strings section,
		/// otherwise it will be retrieved from the default configuration file.
		/// </remarks>
		/// <param name="name">The name for the desired connection string configuration.</param>
		/// <returns>The connection string configuration.</returns>
		/// <exception cref="ArgumentException">if <paramref name="name"/> is <see langword="null"/> (<b>Nothing</b> in Visual Basic) or empty.</exception>
		/// <exception cref="ConfigurationErrorsException">if the connection string object is not found, or if it does not specify a provider name.</exception>
		public ConnectionStringSettings GetConnectionStringSettings(string name)
		{
			ValidateInstanceName(name);

			ConnectionStringSettings connectionStringSettings;
			ConfigurationSection configSection = configurationSource.GetSection("connectionStrings");
			if ((configSection != null) && (configSection is ConnectionStringsSection))
			{
				ConnectionStringsSection connectionStringsSection = configSection as ConnectionStringsSection;
				connectionStringSettings = connectionStringsSection.ConnectionStrings[name];
			}
			else
				connectionStringSettings = ConfigurationManager.ConnectionStrings[name];

			ValidateConnectionStringSettings(name, connectionStringSettings);
			return connectionStringSettings;
		}

		/// <summary>
		/// Returns an enumeration of the <see cref="ConnectionStringSettings"/> instances available in the 
		/// receiver's configuration source.
		/// </summary>
		/// <remarks>
		/// The connection strings will be retrieved from the configuration source if it contains the connection strings section,
		/// otherwise they will be retrieved from the default configuration file.
		/// </remarks>
		/// <returns>The enumeration of available connection strings.</returns>
		public IEnumerable<ConnectionStringSettings> GetConnectionStringSettingsCollection()
		{
			ConnectionStringSettingsCollection collection;
			ConfigurationSection configSection = configurationSource.GetSection("connectionStrings");
			if ((configSection != null) && (configSection is ConnectionStringsSection))
			{
				ConnectionStringsSection connectionStringsSection = configSection as ConnectionStringsSection;
				collection = connectionStringsSection.ConnectionStrings;
			}
			else
			{
				collection = ConfigurationManager.ConnectionStrings;
			}

			foreach (ConnectionStringSettings settings in collection)
			{
				yield return settings;
			}
		}

		private void ValidateInstanceName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
			}
		}

		private static void ValidateDbProviderFactory(string name, DbProviderFactory providerFactory)
		{
			if (providerFactory == null)
			{
				throw new ConfigurationErrorsException(
					string.Format(
						Resources.Culture,
						Resources.ExceptionNoProviderDefinedForConnectionString,
						name));
			}
		}

		private static void ValidateConnectionStringSettings(string name, ConnectionStringSettings connectionStringSettings)
		{
			if (connectionStringSettings == null)
			{
				throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionNoDatabaseDefined, name));
			}

			if (string.IsNullOrEmpty(connectionStringSettings.ProviderName))
			{
				throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionNoProviderDefinedForConnectionString, name));
			}
		}

		/// <summary>
		/// Returns the <see cref="DbProviderMapping"/> that specifies the mapping between an ADO.NET provider factory and a
		/// <see cref="Database"/> instance.
		/// </summary>
		/// <remarks>
		/// The mapping based in logical names will be probed first. If there is no success, the default type based mappings
		/// will be considered. If no default mapping is defined for the provider factory type, the generic database will be used.
		/// </remarks>
		/// <param name="name">The name of the <see cref="Database"/> instance.</param>
		/// <param name="dbProviderName">The logical provider name.</param>
		/// <returns>The <see cref="DbProviderMapping"/> that matches the <paramref name="dbProviderName"/>.</returns>
		public DbProviderMapping GetProviderMapping(string name, string dbProviderName)
		{
			DatabaseSettings settings = this.DatabaseSettings;
			if (settings != null)
			{
				DbProviderMapping existingMapping = settings.ProviderMappings.Get(dbProviderName);
				if (existingMapping != null)
				{
					return existingMapping;
				}
			}

			DbProviderMapping defaultMapping = this.GetDefaultMapping(name, dbProviderName);
			if (defaultMapping != null)
			{
				return defaultMapping;
			}

			return this.GetGenericMapping();
		}

		private DbProviderMapping GetDefaultMapping(string name, string dbProviderName)
		{
			// try to short circuit by default name
			if (DbProviderMapping.DefaultSqlProviderName.Equals(dbProviderName))
				return defaultSqlMapping;

			if (DbProviderMapping.DefaultOracleProviderName.Equals(dbProviderName))
				return defaultOracleMapping;


			// get the default based on type
			DbProviderFactory providerFactory = DbProviderFactories.GetFactory(dbProviderName);
			ValidateDbProviderFactory(name, providerFactory);

			if (SqlClientFactory.Instance == providerFactory)
				return defaultSqlMapping;

			if (OracleClientFactory.Instance == providerFactory)
				return defaultOracleMapping;

			return null;
		}

		private DbProviderMapping GetGenericMapping()
		{
			return defaultGenericMapping;
		}
	}
}