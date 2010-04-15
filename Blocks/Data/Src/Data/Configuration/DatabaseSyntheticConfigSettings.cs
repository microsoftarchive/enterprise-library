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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
    /// <summary>
    /// Provides a configuration-like view over the Data Access Application Block sections
    /// </summary>
    /// <remarks>
    /// As the DataAccessBlock relies on a number of configuration sections (such as connectionStrings), this
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

        private IConfigurationSource configurationSource;

        /// <summary>
        /// Default constructor, used when creating registrations for containers.
        /// </summary>
        public DatabaseSyntheticConfigSettings()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSyntheticConfigSettings"/> class
        /// with the given <see cref="IConfigurationSource"/>.
        /// </summary>
        /// <remarks>This constructor is primarily for test convenience.</remarks>
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
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
            }

            ConnectionStringSettingsCollection connectionStringsCollection = GetConnectionStrings();
            ConnectionStringSettings connectionStringSettings = connectionStringsCollection[name];

            ValidateConnectionStringSettings(name, connectionStringSettings);
            return connectionStringSettings;
        }

        private static void ValidateConnectionStringSettings(string name, ConnectionStringSettings connectionStringSettings)
        {
            if (connectionStringSettings == null)
            {
                throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionNoDatabaseDefined, name));
            }

            if (string.IsNullOrEmpty(connectionStringSettings.ProviderName))
            {
                throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionNoProviderDefinedForConnectionString, name));
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

        /// <summary>
        /// This method is made public for unit testing purposes.
        /// </summary>
        /// <param name="dbProviderName"></param>
        /// <returns></returns>
        public DbProviderMapping GetProviderMapping(string dbProviderName)
        {
            DatabaseSettings settings = (DatabaseSettings)configurationSource.GetSection(DatabaseSettings.SectionName);
            return GetProviderMapping(dbProviderName, settings);
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

        private static TypeRegistration GetInstrumentationProviderRegistration(string instanceName, IConfigurationSource configurationSource)
        {
            var instrumentationSection = InstrumentationConfigurationSection.GetSection(configurationSource);
            return new TypeRegistration<IDataInstrumentationProvider>(
                () => new NewDataInstrumentationProvider(
                    instanceName,
                    instrumentationSection.PerformanceCountersEnabled,
                    instrumentationSection.EventLoggingEnabled,
                    instrumentationSection.ApplicationInstanceName))
                {
                    Name = instanceName
                };
        }

        private TypeRegistration GetDefaultDataEventLoggerRegistration()
        {
            var instrumentationConfigurationSection = InstrumentationConfigurationSection.GetSection(configurationSource);
            return new TypeRegistration<DefaultDataEventLogger>(
                () => new DefaultDataEventLogger(instrumentationConfigurationSection.EventLoggingEnabled))
                {
                    IsDefault = true
                };

        }

        /// <summary>
        /// Creates <see cref="TypeRegistration"/> entries based on <see cref="DatabaseSyntheticConfigSettings"/>
        /// </summary>
        /// <returns>A set of <see cref="TypeRegistration"/> entries.</returns>
        public IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException("configurationSource");

            this.configurationSource = configurationSource;

            return DoGetRegistrations().Select(r => MarkAsPublicName<Database>(r)).ToList();
        }

        private static TypeRegistration MarkAsPublicName<TService>(TypeRegistration registration)
        {
            if(registration.ServiceType == typeof(TService))
            {
                registration.IsPublicName = true;
            }
            return registration;
        }

        private IEnumerable<TypeRegistration> DoGetRegistrations()
        {
            var defaultDatabase = DefaultDatabase;

            foreach (DatabaseData data in Databases.ToList())
            {
                foreach (TypeRegistration typeRegistration in data.GetRegistrations().ToList())
                {
                    if (typeRegistration.ServiceType == typeof(Database)
                        && String.Equals(typeRegistration.Name, defaultDatabase))
                    {
                        typeRegistration.IsDefault = true;
                    }

                    yield return typeRegistration;
                    yield return GetInstrumentationProviderRegistration(typeRegistration.Name, configurationSource);
                }
            }

            yield return GetDefaultDataEventLoggerRegistration();
        }

        /// <summary>
        /// Return the <see cref="TypeRegistration"/> objects needed to reconfigure
        /// the container after a configuration source has changed.
        /// </summary>
        /// <remarks>If there are no reregistrations, return an empty sequence.</remarks>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> containing
        /// the configuration information.</param>
        /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
        public IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
        {
            return GetRegistrations(configurationSource);
        }
    }
}
