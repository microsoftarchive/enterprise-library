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
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Configuration
{
    [TestClass]
    public class GivenNoConnectionStrings
    {
        private DatabaseSyntheticConfigSettings settings;
        private DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void Given()
        {
            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add("connectionStrings", new ConnectionStringsSection());
            settings = new DatabaseSyntheticConfigSettings();
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatesNoRegistrationsForDatabases()
        {
            var typeRegistrations = settings.GetRegistrations(configurationSource).Where(r => r.ServiceType == typeof(Database));

            Assert.AreEqual(0, typeRegistrations.Count());
        }
    }

    [TestClass]
    public class GivenConnectionStringForSqlServer
    {
        private DictionaryConfigurationSource configurationSource;
        private DatabaseSyntheticConfigSettings settings;

        [TestInitialize]
        public void Given()
        {
            configurationSource = new DictionaryConfigurationSource();
            var connectionStringsSection = new ConnectionStringsSection();
            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings
                    {
                        Name = "sql connection",
                        ConnectionString = "connection string",
                        ProviderName = "System.Data.SqlClient"
                    });
            configurationSource.Add("connectionStrings", connectionStringsSection);

            settings = new DatabaseSyntheticConfigSettings();
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatesSingleTypeRegistrationForTheSuppliedNamedDatabase()
        {
            var typeRegistrations = settings.GetRegistrations(configurationSource)
                .Where(r => r.ServiceType == typeof (Database)
                            && r.ImplementationType == typeof (SqlDatabase)
                            && r.Name == "sql connection");

            Assert.AreEqual(1, typeRegistrations.Count());
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationDescribingTheProvider()
        {
            var typeRegistrations = settings.GetRegistrations(configurationSource)
                .Where(r => r.ServiceType == typeof (Database));

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            registration
                .AssertForServiceType(typeof(Database))
                .ForName("sql connection")
                .ForImplementationType(typeof(SqlDatabase));

            registration.AssertConstructor()
                .WithValueConstructorParameter("connection string")
                .WithContainerResolvedParameter<IDataInstrumentationProvider>("sql connection")
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenConnectionForOdbcDatabase
    {
        private DictionaryConfigurationSource configurationSource;
        private DatabaseSyntheticConfigSettings settings;

        [TestInitialize]
        public void Given()
        {
            configurationSource = new DictionaryConfigurationSource();
            var connectionStringsSection = new ConnectionStringsSection();
            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings
                    {
                        Name = "odbc connection",
                        ConnectionString = "connection string",
                        ProviderName = "System.Data.Odbc"
                    });
            configurationSource.Add("connectionStrings", connectionStringsSection);

            settings = new DatabaseSyntheticConfigSettings();
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenShouldReturnASingleRegistration()
        {
            Assert.AreEqual(1, 
                settings.GetRegistrations(configurationSource)
                    .Where(r => r.ServiceType == typeof(Database) && r.ImplementationType == typeof(GenericDatabase))
                    .Count());
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenShouldProvideOdbcRegistration()
        {
            var typeRegistrations = settings.GetRegistrations(configurationSource)
                .Where(r => r.ServiceType == typeof (Database));

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            registration
                .AssertForServiceType(typeof(Database))
                .ForName("odbc connection")
                .ForImplementationType(typeof(GenericDatabase));

            registration.AssertConstructor()
                .WithValueConstructorParameter("connection string")
                .WithValueConstructorParameter<DbProviderFactory>(System.Data.Odbc.OdbcFactory.Instance)
                .WithContainerResolvedParameter<IDataInstrumentationProvider>("odbc connection")
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenConfigurationSourceWithThreeConnections
    {
        private DatabaseSyntheticConfigSettings settings;
        private ConnectionStringsSection connectionStringsSection;
        private DictionaryConfigurationSource configurationSource;
        [TestInitialize]
        public void Context()
        {
            configurationSource = new DictionaryConfigurationSource();
            connectionStringsSection = new ConnectionStringsSection();
            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings
                    {
                        Name = "sql connection",
                        ConnectionString = "connection string",
                        ProviderName = "System.Data.SqlClient"
                    });
            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings
                    {
                        Name = "odbc connection",
                        ConnectionString = "odbc connection string",
                        ProviderName = "System.Data.Odbc"
                    });
            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings
                    {
                        Name = "oracle connection",
                        ConnectionString = "oracle connection string",
                        ProviderName = "System.Data.OracleClient"
                    });
            configurationSource.Add("connectionStrings", connectionStringsSection);
            settings = new DatabaseSyntheticConfigSettings();
        }

        [TestMethod]
        public void WhenRegistrationsRequested_ThenReturnsThreeRegistrationsForDatabases()
        {
            Assert.AreEqual(3, settings.GetRegistrations(configurationSource).Where(r=> r.ServiceType == typeof(Database)).Count());
        }

        [TestMethod]
        public void WhenRegistrationsRequested_ThenContainsARegistrationPerConnectinString()
        {
            CollectionAssert.AreEqual(
                new List<string>(connectionStringsSection.ConnectionStrings.Cast<ConnectionStringSettings>().Select(s => s.Name)),
                new List<string>(settings.GetRegistrations(configurationSource)
                    .Where(r => r.ServiceType == typeof(Database)).Select(r => r.Name))
                );
        }
    }

    [TestClass]
    public class GivenConfigurationForAnOracleDatabase
    {
        private DictionaryConfigurationSource configurationSource;
        private DatabaseSyntheticConfigSettings settings;

        [TestInitialize]
        public void Given()
        {
            // Setup connection strings
            configurationSource = new DictionaryConfigurationSource();
            var connectionStringsSection = new ConnectionStringsSection();
            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings
                    {
                        Name = "myConnectionName",
                        ConnectionString = "myConnectionString",
                        ProviderName = "System.Data.OracleClient"
                    }
                );

            configurationSource.Add("connectionStrings", connectionStringsSection);

            // Setup oracle configuration sections
            var oracleConnectionData = new OracleConnectionData { Name = "myConnectionName" };
            oracleConnectionData.Packages.Add(new OraclePackageData("foo", "bar"));

            var oracleSettings = new OracleConnectionSettings();
            oracleSettings.OracleConnectionsData.Add(oracleConnectionData);
            configurationSource.Add(OracleConnectionSettings.SectionName, oracleSettings);


            settings = new DatabaseSyntheticConfigSettings();
        }

        [TestMethod]
        public void WhenRegistrationsRequested_ThenResultsInASingleRegistrationForOracleDatabase()
        {
            var registrations = settings.GetRegistrations(configurationSource)
                .Where(r => r.ServiceType == typeof (Database) && r.ImplementationType == typeof(OracleDatabase));

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenRegistrationsRequested_ThenProvidesRegistrationForOracleDatabase()
        {
            var registration = settings.GetRegistrations(configurationSource).
                Where(r => r.ServiceType == typeof(Database)).ElementAt(0);

            registration.AssertForServiceType(typeof(Database))
                .ForName("myConnectionName")
                .ForImplementationType(typeof(OracleDatabase));

            IEnumerable<IOraclePackage> packages;

            registration.AssertConstructor()
                .WithValueConstructorParameter("myConnectionString")
                .WithValueConstructorParameter(out packages)
                .WithContainerResolvedParameter<IDataInstrumentationProvider>("myConnectionName")
                .VerifyConstructorParameters();

            Assert.AreEqual(1, packages.Count());
            Assert.AreEqual("foo", packages.ElementAt(0).Name);
            Assert.AreEqual("bar", packages.ElementAt(0).Prefix);

        }
    }

    [TestClass]
    public class GivenSqlDatabaseDataElementRegistration
    {
        private TypeRegistration registration;


        [TestInitialize]
        public void Given()
        {
            var databaseData = new SqlDatabaseData(new ConnectionStringSettings
                                                       {
                                                           Name = "myConnectionName",
                                                           ConnectionString = "myConnectionString"
                                                       },
                                                   new DictionaryConfigurationSource()
                );

            registration = databaseData.GetRegistrations().First();
        }

        [TestMethod]
        public void ThenRegistrationTypeAndNameAreForSqlDatabase()
        {
            registration
                .AssertForServiceType(typeof(Database))
                .ForName("myConnectionName")
                .ForImplementationType(typeof(SqlDatabase));
        }

        [TestMethod]
        public void ThenConnectionStringIsProvidedAsParameter()
        {
            registration.AssertConstructor()
                .WithValueConstructorParameter("myConnectionString")
                .WithContainerResolvedParameter<IDataInstrumentationProvider>("myConnectionName")
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenGenericDatabaseElementRegistration
    {
        private TypeRegistration registration;

        [TestInitialize]
        public void Given()
        {
            var databaseData = new GenericDatabaseData(new ConnectionStringSettings
                                                           {
                                                               Name = "myConnectionName",
                                                               ConnectionString = "myConnectionString",
                                                               ProviderName = "System.Data.Odbc"
                                                           },
                                                       new DictionaryConfigurationSource()
                );

            registration = databaseData.GetRegistrations().First();
        }

        [TestMethod]
        public void ThenRegistrationTypeAndNameAreForGenericDatabase()
        {
            registration
                .AssertForServiceType(typeof(Database))
                .ForName("myConnectionName")
                .ForImplementationType(typeof(GenericDatabase));
        }

        [TestMethod]
        public void ThenConnectionStringIsProvidedAsParameter()
        {
            registration.AssertConstructor()
                .WithValueConstructorParameter("myConnectionString")
                .WithValueConstructorParameter<DbProviderFactory>(System.Data.Odbc.OdbcFactory.Instance)
                .WithContainerResolvedParameter<IDataInstrumentationProvider>("myConnectionName")
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenOracleDatabaseElementRegistrationWithPackageMappings
    {
        private TypeRegistration registration;

        [TestInitialize]
        public void Given()
        {
            var configurationSource = new DictionaryConfigurationSource();

            var oracleConnectionData = new OracleConnectionData { Name = "myConnectionName" };
            oracleConnectionData.Packages.Add(new OraclePackageData("foo", "bar"));
            var oracleSettings = new OracleConnectionSettings();
            oracleSettings.OracleConnectionsData.Add(oracleConnectionData);
            configurationSource.Add(OracleConnectionSettings.SectionName, oracleSettings);

            var databaseData = new OracleDatabaseData(
                new ConnectionStringSettings
                    {
                        Name = "myConnectionName",
                        ConnectionString = "myConnectionString",
                        ProviderName = "System.Data.Odbc"
                    },
                configurationSource);

            registration = databaseData.GetRegistrations().First();
        }

        [TestMethod]
        public void ThenRegistrationTypeAndNameAreForOracleDatabase()
        {
            registration
                .AssertForServiceType(typeof(Database))
                .ForName("myConnectionName")
                .ForImplementationType(typeof(OracleDatabase));
        }

        [TestMethod]
        public void ThenConnectionStringAndPackagesAreProvidedAsParameters()
        {
            IEnumerable<IOraclePackage> packages;

            registration.AssertConstructor()
                .WithValueConstructorParameter("myConnectionString")
                .WithValueConstructorParameter(out packages)
                .WithContainerResolvedParameter<IDataInstrumentationProvider>("myConnectionName")
                .VerifyConstructorParameters();

            Assert.AreEqual(1, packages.Count());
            Assert.AreEqual("foo", packages.ElementAt(0).Name);
            Assert.AreEqual("bar", packages.ElementAt(0).Prefix);
        }
    }

    [TestClass]
    public class GivenSyntheticConfigSettingsWithConnectionStringAndNoDatabaseSettings
    {
        private DictionaryConfigurationSource configurationSource;
        private DatabaseSyntheticConfigSettings settings;

        [TestInitialize]
        public void Given()
        {
            configurationSource = new DictionaryConfigurationSource();
            var connectionStringsSection = new ConnectionStringsSection();
            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings
                {
                    Name = "sql connection",
                    ConnectionString = "connection string",
                    ProviderName = "System.Data.SqlClient"
                });
            configurationSource.Add("connectionStrings", connectionStringsSection);
            settings = new DatabaseSyntheticConfigSettings();
        }

        [TestMethod]
        public void WhenCreatingRegistrations_CreatesNonDefaultEntryForTheConnectionString()
        {
            var typeRegistration = settings.GetRegistrations(configurationSource)
                .First(db => db.Name == "sql connection");

            Assert.IsFalse(typeRegistration.IsDefault);
        }
    }

    [TestClass]
    public class GivenSyntheticConfigSettingsWithConnectionStringAndDatabaseSettingsWithNoDefaultSet
    {
        private DictionaryConfigurationSource configurationSource;
        private DatabaseSyntheticConfigSettings settings;

        [TestInitialize]
        public void Given()
        {
            configurationSource = new DictionaryConfigurationSource();
            var connectionStringsSection = new ConnectionStringsSection();
            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings
                {
                    Name = "sql connection",
                    ConnectionString = "connection string",
                    ProviderName = "System.Data.SqlClient"
                });
            configurationSource.Add("connectionStrings", connectionStringsSection);
            configurationSource.Add(DatabaseSettings.SectionName, new DatabaseSettings());
            settings = new DatabaseSyntheticConfigSettings();
        }

        [TestMethod]
        public void WhenCreatingRegistrations_CreatesNonDefaultEntryForTheConnectionString()
        {
            var typeRegistration = settings.GetRegistrations(configurationSource)
                .First(db => db.Name == "sql connection");

            Assert.IsFalse(typeRegistration.IsDefault);
        }
    }

    [TestClass]
    public class GivenSyntheticConfigSettingsWithConnectionStringAndDatabaseSettingsWithDefaultSet
    {
        private DictionaryConfigurationSource configurationSource;
        private DatabaseSyntheticConfigSettings settings;

        [TestInitialize]
        public void Given()
        {
            configurationSource = new DictionaryConfigurationSource();
            var connectionStringsSection = new ConnectionStringsSection();
            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings
                {
                    Name = "sql connection",
                    ConnectionString = "connection string",
                    ProviderName = "System.Data.SqlClient"
                });
            configurationSource.Add("connectionStrings", connectionStringsSection);
            configurationSource.Add(
                DatabaseSettings.SectionName,
                new DatabaseSettings { DefaultDatabase = "sql connection" });
            settings = new DatabaseSyntheticConfigSettings();
        }

        [TestMethod]
        public void WhenCreatingRegistrations_CreatesNonDefaultEntryForTheConnectionString()
        {
            var typeRegistration = settings.GetRegistrations(configurationSource)
                .First(db => db.Name == "sql connection");

            Assert.IsTrue(typeRegistration.IsDefault);
        }
    }
}
