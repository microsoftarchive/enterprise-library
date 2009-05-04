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
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
    /// <summary>
    /// Provides a configuration-like view over the data access block sections
    /// </summary>
    /// <remarks>
    /// As the DataAccessBlock relies on a number of configuraiton sections (such as connectionStrings), this
    /// config settings provides an abstraction over all these to simplify creating <see cref="TypeRegistration"/> entries.
    /// </remarks>
    public class DatabaseSyntheticConfigSettings : ITypeRegistrationsProvider
    {
        private static readonly DbProviderMapping defaultSqlMapping =
            new DbProviderMapping(DbProviderMapping.DefaultSqlProviderName, typeof(SqlDatabase));
        private static readonly DbProviderMapping defaultOracleMapping =
            new DbProviderMapping(DbProviderMapping.DefaultOracleProviderName, typeof(OracleDatabase));
        private static readonly DbProviderMapping defaultGenericMapping =
            new DbProviderMapping(DbProviderMapping.DefaultGenericProviderName, typeof(GenericDatabase));

        private readonly IConfigurationSource configurationSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSyntheticConfigSettings"/> class with a configuration 
        /// source.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        public DatabaseSyntheticConfigSettings(IConfigurationSource configurationSource)
        {
            this.configurationSource = configurationSource;
        }

        /// <summary>
        /// Gets the default database instance name.
        /// </summary>
        public string DefaultDatabase
        {
            get
            {
                var databaseSettings = configurationSource.GetSection(DatabaseSettings.SectionName) as DatabaseSettings;
                return databaseSettings != null ? databaseSettings.DefaultDatabase : string.Empty;
            }
        }

        /// <summary>
        /// Gets the objects describing the database instances in the configuration source.
        /// </summary>
        /// <remarks>
        /// Databases are derived from the collection of connection strings.
        /// </remarks>
        public IEnumerable<DatabaseData> Databases
        {
            get
            {
                var databaseSettings = (DatabaseSettings)configurationSource.GetSection(DatabaseSettings.SectionName);

                foreach (ConnectionStringSettings connectionString in GetConnectionStrings())
                {
                    if (IsValidProviderName(connectionString.ProviderName))
                    {
                        yield return GetDatabaseData(connectionString, databaseSettings);
                    }
                }
            }
        }

        private ConnectionStringSettingsCollection GetConnectionStrings()
        {
            var section = configurationSource.GetSection("connectionStrings") as ConnectionStringsSection;

            return section != null ? section.ConnectionStrings : ConfigurationManager.ConnectionStrings;
        }

        private static bool IsValidProviderName(string providerName)
        {
            return DbProviderFactories.GetFactoryClasses().Rows.Find(providerName) != null;
        }

        private DatabaseData GetDatabaseData(ConnectionStringSettings connectionString, DatabaseSettings databaseSettings)
        {
            var mapping = GetProviderMapping(connectionString.ProviderName, databaseSettings);

            var configurationElementTypeAttribute = GetAttribute(mapping.DatabaseType);

            var databaseData =
                CreateDatabaseData(configurationElementTypeAttribute.ConfigurationType, connectionString, configurationSource);

            return databaseData;
        }

        private static ConfigurationElementTypeAttribute GetAttribute(Type databaseType)
        {
            var configurationElementTypeAttribute = (ConfigurationElementTypeAttribute)
                                                    Attribute.GetCustomAttribute(databaseType, typeof(ConfigurationElementTypeAttribute), false);

            if (configurationElementTypeAttribute == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ExceptionNoConfigurationElementTypeAttribute,
                        databaseType.Name));
            }

            return configurationElementTypeAttribute;
        }

        private static DatabaseData CreateDatabaseData(
            Type configurationElementType,
            ConnectionStringSettings settings,
            IConfigurationSource source)
        {
            object newInstance;

            try
            {
                newInstance = Activator.CreateInstance(
                    configurationElementType,
                    new object[] { settings, source }
                    );
            }
            catch (MissingMethodException e)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ExceptionDatabaseDataTypeDoesNotHaveRequiredConstructor,
                        configurationElementType),
                    e);
            }
            try
            {
                return (DatabaseData)newInstance;
            }
            catch (InvalidCastException e)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ExceptionDatabaseDataTypeDoesNotInheritFromDatabaseData,
                        configurationElementType),
                    e);
            }
        }

        private static DbProviderMapping GetProviderMapping(string dbProviderName, DatabaseSettings databaseSettings)
        {
            if (databaseSettings != null)
            {
                var existingMapping = databaseSettings.ProviderMappings.Get(dbProviderName);
                if (existingMapping != null)
                {
                    return existingMapping;
                }
            }

            var defaultMapping = GetDefaultMapping(dbProviderName);
            return defaultMapping ?? GetGenericMapping();
        }

        private static DbProviderMapping GetDefaultMapping(string dbProviderName)
        {
            // try to short circuit by default name
            if (DbProviderMapping.DefaultSqlProviderName.Equals(dbProviderName))
                return defaultSqlMapping;

            if (DbProviderMapping.DefaultOracleProviderName.Equals(dbProviderName))
                return defaultOracleMapping;

            // get the default based on type
            var providerFactory = DbProviderFactories.GetFactory(dbProviderName);

            if (SqlClientFactory.Instance == providerFactory)
                return defaultSqlMapping;

            if (OracleClientFactory.Instance == providerFactory)
                return defaultOracleMapping;

            return null;
        }

        private static DbProviderMapping GetGenericMapping()
        {
            return defaultGenericMapping;
        }

        
        /// <summary>
        /// Creates <see cref="TypeRegistration"/> entries based on <see cref="DatabaseSyntheticConfigSettings"/>
        /// </summary>
        /// <returns>An set of <see cref="TypeRegistration"/> entries.</returns>
        public IEnumerable<TypeRegistration> CreateRegistrations()
        {
            foreach (var data in Databases)
            {
                yield return data.GetContainerConfigurationModel();
            }
        }
    }
}
