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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Configuration
{
    [TestClass]
    public class GivenAConfigurationSourceWithAnEmptyConnectionStringsSection
    {
        private DictionaryConfigurationSource configSource;
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            configSource = new DictionaryConfigurationSource();
            configSource.Add("connectionStrings", new ConnectionStringsSection());
            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [TestMethod]
        public void WhenConstructed_ThenThereAreNoDatabases()
        {
            Assert.AreEqual(0, configSettings.Databases.Count());
        }

        [TestMethod]
        public void WhenConstructed_ThenThereIsNoDefaultDatabase()
        {
            Assert.AreEqual(string.Empty, configSettings.DefaultDatabase);
        }
    }

    [TestClass]
    public class GivenAConfigurationSourceWithADatabaseSettingsSectionWithNoDefaultDatabase
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            configSource.Add(DatabaseSettings.SectionName, new DatabaseSettings());
            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [TestMethod]
        public void WhenConstructed_ThenThereIsNoDefaultDatabase()
        {
            Assert.AreEqual(string.Empty, configSettings.DefaultDatabase);
        }
    }

    [TestClass]
    public class GivenAConfigurationSourceWithADatabaseSettingsSectionWithDefaultDatabase
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            configSource.Add(DatabaseSettings.SectionName, new DatabaseSettings { DefaultDatabase = "default" });
            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [TestMethod]
        public void WhenConstructed_ThenThereIsDefaultDatabase()
        {
            Assert.AreEqual("default", configSettings.DefaultDatabase);
        }
    }

    [TestClass]
    public class GivenAConfigurationSourceWithAConnectionStringSettingWithNoProviderName
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("someSetting", "someConnectionString"));

            configSource.Add("connectionStrings", connectionStrings);

            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [TestMethod]
        public void WhenConstructed_ThenThereAreNoDatabases()
        {
            Assert.AreEqual(0, configSettings.Databases.Count());
        }
    }

    [TestClass]
    public class GivenAConfigurationSourceWithAConnectionStringSettingWithNonRegisteredProviderName
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(new ConnectionStringSettings
                                                        {
                                                            Name = "someSetting",
                                                            ConnectionString = "someConnectionString",
                                                            ProviderName = "non registered"
                                                        });

            configSource.Add("connectionStrings", connectionStrings);

            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [TestMethod]
        public void WhenConstructed_ThenThereAreNoDatabases()
        {
            Assert.AreEqual(0, configSettings.Databases.Count());
        }
    }

    [TestClass]
    public class GivenAConfigurationSourceWithASqlProviderConnectionString
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(
                new ConnectionStringSettings("someSetting", "someConnectionString", "System.Data.SqlClient"));

            configSource.Add("connectionStrings", connectionStrings);

            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [TestMethod]
        public void ThenSettingsContainASingleSqlDatabaseData()
        {
            Assert.IsInstanceOfType(configSettings.Databases.ElementAt(0), typeof(SqlDatabaseData));
        }
    }

    [TestClass]
    public class GivenAConfigurationSourceWithAnOdbcProviderConnectionString
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(
                new ConnectionStringSettings("someSetting", "someConnectionString", "System.Data.Odbc"));

            configSource.Add("connectionStrings", connectionStrings);

            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [TestMethod]
        public void ThenSettingsContainASingleGenericDatabaseData()
        {
            Assert.IsInstanceOfType(configSettings.Databases.ElementAt(0), typeof(GenericDatabaseData));
        }

        [TestMethod]
        public void ThenGenericDatabaseDataHasOdbcDataProvider()
        {
            Assert.AreEqual("System.Data.Odbc", ((GenericDatabaseData)configSettings.Databases.ElementAt(0)).ProviderName);
        }
    }

    [TestClass]
    public class GivenAConfigurationSourceWithAnOdbcProviderConnectionStringAndAProviderMappingToASqlDatabase
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(
                new ConnectionStringSettings("someSetting", "someConnectionString", "System.Data.Odbc"));
            configSource.Add("connectionStrings", connectionStrings);

            var databaseSettings = new DatabaseSettings();
            databaseSettings.ProviderMappings.Add(new DbProviderMapping("System.Data.Odbc", typeof(SqlDatabase)));
            configSource.Add(DatabaseSettings.SectionName, databaseSettings);

            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [TestMethod]
        public void ThenSettingsContainASingleSqlDatabaseData()
        {
            Assert.IsInstanceOfType(configSettings.Databases.ElementAt(0), typeof(SqlDatabaseData));
        }
    }

    [TestClass]
    public class GivenAConfigurationSourceWithAConnectionStringForAProviderMappedToADatabaseClassWithNoConfigurationElementTypeAttribute
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(
                new ConnectionStringSettings("someSetting", "someConnectionString", "System.Data.Odbc"));
            configSource.Add("connectionStrings", connectionStrings);

            var databaseSettings = new DatabaseSettings();
            databaseSettings.ProviderMappings.Add(new DbProviderMapping("System.Data.Odbc", typeof(TestDatabaseWithNoConfigurationElementTypeAttribute)));
            configSource.Add(DatabaseSettings.SectionName, databaseSettings);

            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        public class TestDatabaseWithNoConfigurationElementTypeAttribute
        {
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]  // TODO appropriate exception?
        public void TheRequestingForConfigurationObjectThrows()
        {
            configSettings.Databases.ElementAt(0);
        }
    }

    [TestClass]
    public class GivenAConfigurationSourceWithAConnectionStringForAProviderMappedToADatabaseWithAConfigurationElementTypeWithNoZeroArgumentConstructor
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(
                new ConnectionStringSettings("someSetting", "someConnectionString", "System.Data.Odbc"));
            configSource.Add("connectionStrings", connectionStrings);

            var databaseSettings = new DatabaseSettings();
            databaseSettings.ProviderMappings.Add(new DbProviderMapping("System.Data.Odbc", typeof(TestDatabase)));
            configSource.Add(DatabaseSettings.SectionName, databaseSettings);

            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [ConfigurationElementType(typeof(TestDatabaseData))]
        public class TestDatabase
        {
        }

        public class TestDatabaseData : DatabaseData
        {
            public TestDatabaseData(ConnectionStringSettings connectionString, Func<string, ConfigurationSection> configurationSource, int ignored)
                : base(connectionString, configurationSource)
            {
            }

            public override Database BuildDatabase()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]  // TODO appropriate exception?
        public void TheRequestingForConfigurationObjectThrows()
        {
            configSettings.Databases.ElementAt(0);
        }
    }

    [TestClass]
    public class GivenAConfigurationSourceWithAConnectionStringForAProviderMappedToADatabaseWithANonDatabaseDataConfigurationElementType
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(
                new ConnectionStringSettings("someSetting", "someConnectionString", "System.Data.Odbc"));
            configSource.Add("connectionStrings", connectionStrings);

            var databaseSettings = new DatabaseSettings();
            databaseSettings.ProviderMappings.Add(new DbProviderMapping("System.Data.Odbc", typeof(TestDatabase)));
            configSource.Add(DatabaseSettings.SectionName, databaseSettings);

            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [ConfigurationElementType(typeof(TestDatabaseData))]
        public class TestDatabase
        {
        }

        public class TestDatabaseData
        {
            public TestDatabaseData(ConnectionStringSettings connectionString, Func<string, ConfigurationSection> configurationSource)
            {
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]  // TODO appropriate exception?
        public void TheRequestingForConfigurationObjectThrows()
        {
            configSettings.Databases.ElementAt(0);
        }
    }

    [TestClass]
    public class GivenAConfigurationSourceWithAConnectionStringWithAnEmptyProviderName
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(
                new ConnectionStringSettings("someSetting", "someConnectionString"));
            configSource.Add("connectionStrings", connectionStrings);

            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ThenGettingTheConnectionStringByNameThrows()
        {
            configSettings.GetDatabase("someSetting");
        }
    }

    [TestClass]
    public class GivenAConfigurationSourceWithAConnectionStringWithAnInvalidProviderName
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(
                new ConnectionStringSettings("someSetting", "someConnectionString", "invalid provider name"));
            configSource.Add("connectionStrings", connectionStrings);

            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ThenGettingTheConnectionStringByNameThrows()
        {
            configSettings.GetDatabase("someSetting");
        }
    }

    #region Oracle database tests

    [TestClass]
    public class GivenASourceWithAConnectionStringForTheOracleProvider
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(
                new ConnectionStringSettings("someSetting", "someConnectionString", "System.Data.OracleClient"));

            configSource.Add("connectionStrings", connectionStrings);

            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [TestMethod]
        public void ThenSettingsContainASingleOracleDatabaseData()
        {
            Assert.IsInstanceOfType(configSettings.Databases.ElementAt(0), typeof(OracleDatabaseData));
        }

        [TestMethod]
        public void ThenCreatedOracleDatabaseDataHasNoPackageMappings()
        {
            Assert.AreEqual(0, ((OracleDatabaseData)configSettings.Databases.ElementAt(0)).PackageMappings.Count());
        }
    }

    [TestClass]
    public class GivenASourceWithAConnectionStringForTheOracleProviderAndOracleSection
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(
                new ConnectionStringSettings("someSetting", "someConnectionString", "System.Data.OracleClient"));
            configSource.Add("connectionStrings", connectionStrings);

            var oracleSettings = new OracleConnectionSettings();
            configSource.Add(OracleConnectionSettings.SectionName, oracleSettings);

            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [TestMethod]
        public void ThenSettingsContainASingleOracleDatabaseData()
        {
            Assert.IsInstanceOfType(configSettings.Databases.ElementAt(0), typeof(OracleDatabaseData));
        }

        [TestMethod]
        public void ThenCreatedOracleDatabaseDataHasNoPackageMappings()
        {
            Assert.AreEqual(0, ((OracleDatabaseData)configSettings.Databases.ElementAt(0)).PackageMappings.Count());
        }
    }

    [TestClass]
    public class GivenASourceWithAConnectionStringForTheOracleProviderAndPackageMappingsInTheOracleSection
    {
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            var configSource = new DictionaryConfigurationSource();

            var connectionStrings = new ConnectionStringsSection();
            connectionStrings.ConnectionStrings.Add(
                new ConnectionStringSettings("someSetting", "someConnectionString", "System.Data.OracleClient"));
            configSource.Add("connectionStrings", connectionStrings);

            var oracleConnectionData = new OracleConnectionData { Name = "someSetting" };
            oracleConnectionData.Packages.Add(new OraclePackageData("foo", "bar"));
            var oracleSettings = new OracleConnectionSettings();
            oracleSettings.OracleConnectionsData.Add(oracleConnectionData);
            configSource.Add(OracleConnectionSettings.SectionName, oracleSettings);

            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [TestMethod]
        public void ThenSettingsContainASingleOracleDatabaseData()
        {
            Assert.IsInstanceOfType(configSettings.Databases.ElementAt(0), typeof(OracleDatabaseData));
        }

        [TestMethod]
        public void ThenCreatedOracleDatabaseDataHasSinglePackageMapping()
        {
            Assert.AreEqual(1, ((OracleDatabaseData)configSettings.Databases.ElementAt(0)).PackageMappings.Count());
        }

        [TestMethod]
        public void ThenThePackageMappingsCorrespondToTheOracleSpecificPackageMappings()
        {
            Assert.AreEqual("foo", ((OracleDatabaseData)configSettings.Databases.ElementAt(0)).PackageMappings.ElementAt(0).Name);
            Assert.AreEqual("bar", ((OracleDatabaseData)configSettings.Databases.ElementAt(0)).PackageMappings.ElementAt(0).Prefix);
        }
    }

    #endregion

    [TestClass]
    public class GivenAConfigurationSourceWithNoConnectionStringsSection
    {
        private DictionaryConfigurationSource configSource;
        private DatabaseSyntheticConfigSettings configSettings;

        [TestInitialize]
        public void Setup()
        {
            configSource = new DictionaryConfigurationSource();
            configSettings = new DatabaseSyntheticConfigSettings(configSource.GetSection);
        }

        [TestMethod]
        public void WhenConstructed_DatabasesMatchTheConnectionStringsInTheConfigurationFileForTheAppDomain()
        {
            var databases = configSettings.Databases.ToDictionary(data => data.Name);

            Assert.IsTrue(databases.Count >= 8);
            CollectionAssert.AreEquivalent(
                new[] 
                {
                    "LocalSqlServer",
                    "Service_Dflt", 
                    "OracleTest", 
                    "OdbcDatabase", 
                    "mapping1", 
                    "mapping2", 
                    "NewDatabase", 
                    "DbWithSqlServerAuthn", 
                    "NorthwindPersistFalse"
                },
                databases.Keys);

        }
    }
}
