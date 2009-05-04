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

        [TestInitialize]
        public void Given()
        {
            var configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add("connectionStrings", new ConnectionStringsSection());
            settings = new DatabaseSyntheticConfigSettings(configurationSource);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatesNoRegistrations()
        {
            var typeRegistrations = settings.CreateRegistrations();

            Assert.AreEqual(0, typeRegistrations.Count());
        }
    }

    [TestClass]
    public class GivenConnectionStringForSqlServer
    {
        private DatabaseSyntheticConfigSettings settings;

        [TestInitialize]
        public void Given()
        {
            var configurationSource = new DictionaryConfigurationSource();
            var connectionStringsSection = new ConnectionStringsSection();
            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings
                    {
                        Name = "sql connection",
                        ConnectionString = "connection string",
                        ProviderName = "System.Data.SqlClient"
                    });
            configurationSource.Add("connectionStrings", connectionStringsSection);

            settings = new DatabaseSyntheticConfigSettings(configurationSource);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatesSingleTypeRegistrationForTheSuppliedName()
        {
            var typeRegistrations = settings.CreateRegistrations();

            Assert.AreEqual(1, typeRegistrations.Count());
            Assert.AreEqual("sql connection", typeRegistrations.ElementAt(0).Name);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenCreatedTypeRegistrationDescribingTheProvider()
        {
            var typeRegistrations = settings.CreateRegistrations();

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            registration
                .AssertForServiceType(typeof(Database))
                .ForName("sql connection")
                .ForImplementationType(typeof(SqlDatabase));

            registration.AssertConstructor()
                .WithValueConstructorParameter("connection string")
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenConnectionForOdbcDatabase
    {
        private DatabaseSyntheticConfigSettings settings;

        [TestInitialize]
        public void Given()
        {
            var configurationSource = new DictionaryConfigurationSource();
            var connectionStringsSection = new ConnectionStringsSection();
            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings
                    {
                        Name = "odbc connection",
                        ConnectionString = "connection string",
                        ProviderName = "System.Data.Odbc"
                    });
            configurationSource.Add("connectionStrings", connectionStringsSection);

            settings = new DatabaseSyntheticConfigSettings(configurationSource);
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenShouldReturnASingleRegistration()
        {
            Assert.AreEqual(1, settings.CreateRegistrations().Count());
        }

        [TestMethod]
        public void WhenCreatingRegistrations_ThenShouldProvideOdbcRegistration()
        {
            var typeRegistrations = settings.CreateRegistrations();

            TypeRegistration registration = typeRegistrations.ElementAt(0);

            registration
                .AssertForServiceType(typeof(Database))
                .ForName("odbc connection")
                .ForImplementationType(typeof(GenericDatabase));

            registration.AssertConstructor()
                .WithValueConstructorParameter("connection string")
                .WithValueConstructorParameter<DbProviderFactory>(System.Data.Odbc.OdbcFactory.Instance)
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenConfigurationSourceWithThreeConnections
    {
        private DatabaseSyntheticConfigSettings settings;
        private ConnectionStringsSection connectionStringsSection;

        [TestInitialize]
        public void Context()
        {
            var configurationSource = new DictionaryConfigurationSource();
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
            settings = new DatabaseSyntheticConfigSettings(configurationSource);
        }

        [TestMethod]
        public void WhenRegistrationsRequested_ThenReturnsThreeRegistration()
        {
            Assert.AreEqual(3, settings.CreateRegistrations().Count());
        }

        [TestMethod]
        public void WhenRegistrationsRequested_ThenContainsARegistrationPerConnectinString()
        {
            CollectionAssert.AreEqual(
                new List<string>(connectionStringsSection.ConnectionStrings.Cast<ConnectionStringSettings>().Select(s => s.Name)),
                new List<string>(settings.CreateRegistrations().Select(r => r.Name))
                );
        }
    }

    [TestClass]
    public class GivenConfigurationForAnOracleDatabase
    {
        private DatabaseSyntheticConfigSettings settings;

        [TestInitialize]
        public void Given()
        {
            // Setup connection strings
            var configurationSource = new DictionaryConfigurationSource();
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


            settings = new DatabaseSyntheticConfigSettings(configurationSource);
        }

        [TestMethod]
        public void WhenRegistrationsRequested_ThenResultsInASingleRegistration()
        {
            Assert.AreEqual(1, settings.CreateRegistrations().Count());
        }

        [TestMethod]
        public void WhenRegistrationsRequested_ThenProvidesRegistrationForOracleDatabase()
        {
            var registration = settings.CreateRegistrations().ElementAt(0);

            registration.AssertForServiceType(typeof(Database))
                .ForName("myConnectionName")
                .ForImplementationType(typeof(OracleDatabase));

            IEnumerable<IOraclePackage> packages;

            registration.AssertConstructor()
                .WithValueConstructorParameter("myConnectionString")
                .WithValueConstructorParameter(out packages)
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

            registration = databaseData.GetContainerConfigurationModel();
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

            registration = databaseData.GetContainerConfigurationModel();
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

            registration = databaseData.GetContainerConfigurationModel();
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
                .VerifyConstructorParameters();

            Assert.AreEqual(1, packages.Count());
            Assert.AreEqual("foo", packages.ElementAt(0).Name);
            Assert.AreEqual("bar", packages.ElementAt(0).Prefix);
        }
    }
}
