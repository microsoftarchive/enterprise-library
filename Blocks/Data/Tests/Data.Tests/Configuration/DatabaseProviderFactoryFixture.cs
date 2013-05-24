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
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Configuration
{
    [TestClass]
    public class DatabaseProviderFactoryFixture
    {
        [TestMethod]
        public void CanCreateDatabaseFromFactory()
        {
            var factory = new DatabaseProviderFactory(new SystemConfigurationSource(false).GetSection);
            Database createdObject = factory.Create("Service_Dflt");

            Assert.IsNotNull(createdObject);
            Assert.IsInstanceOfType(createdObject, typeof(SqlDatabase));
            Assert.AreEqual(@"server=(localdb)\v11.0;database=northwind;integrated security=true;",
                            createdObject.ConnectionStringWithoutCredentials);
        }

        [TestMethod]
        public void CanCreateDefaultDatabaseFromFactory()
        {
            var factory = new DatabaseProviderFactory(new SystemConfigurationSource(false).GetSection);
            Database createdObject = factory.CreateDefault();

            Assert.IsNotNull(createdObject);
            Assert.IsInstanceOfType(createdObject, typeof(SqlDatabase));
            Assert.AreEqual(@"server=(localdb)\v11.0;database=northwind;integrated security=true;",
                            createdObject.ConnectionStringWithoutCredentials);
        }

        [TestMethod]
        public void CanCreateSqlDatabaseFromFactory()
        {
            var factory = new DatabaseProviderFactory(new SystemConfigurationSource(false).GetSection);
            Database createdObject = factory.Create("Service_Dflt");

            Assert.IsNotNull(createdObject);
            Assert.AreEqual(@"server=(localdb)\v11.0;database=northwind;integrated security=true;",
                            createdObject.ConnectionStringWithoutCredentials);
        }

        [TestMethod]
        public void SkipsConnectionStringsWithoutProviderNamesOrWithProviderNamesWhichDoNotMapToAProviderFactory()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            ConnectionStringsSection section = new ConnectionStringsSection();
            section.ConnectionStrings.Add(new ConnectionStringSettings("cs1", "cs1", "System.Data.SqlClient"));
            section.ConnectionStrings.Add(new ConnectionStringSettings("cs2", "cs2"));
            section.ConnectionStrings.Add(new ConnectionStringSettings("cs3", "cs3", "a bogus provider name"));
            section.ConnectionStrings.Add(new ConnectionStringSettings("cs4", "cs4", "System.Data.SqlClient"));
            configurationSource.Add("connectionStrings", section);

            var factory = new DatabaseProviderFactory(configurationSource.GetSection);

            Assert.AreEqual("cs1", factory.Create("cs1").ConnectionString);
            Assert.AreEqual("cs4", factory.Create("cs4").ConnectionString);
            try
            {
                factory.Create("cs2");
                Assert.Fail("should have thrown");
            }
            catch (InvalidOperationException)
            {
                // expected, connection string is ignored
            }

            try
            {
                factory.Create("cs3");
                Assert.Fail("should have thrown");
            }
            catch (InvalidOperationException)
            {
                // expected, connection string is ignored
            }
        }

#pragma warning disable 612, 618
        [TestMethod]
        public void CanCreateOracleDatabaseFromFactory()
        {
            var factory = new DatabaseProviderFactory(new SystemConfigurationSource(false).GetSection);

            OracleDatabase createdObject = (OracleDatabase)factory.Create("OracleTest");
            Assert.IsNotNull(createdObject);
            Assert.AreEqual(@"server=entlib;", createdObject.ConnectionStringWithoutCredentials);

            // can do the configured package mapping?
            Assert.AreEqual(DatabaseFactoryFixture.OracleTestTranslatedStoredProcedureInPackageWithTranslation,
                            createdObject.GetStoredProcCommand(
                                DatabaseFactoryFixture.OracleTestStoredProcedureInPackageWithTranslation).CommandText);
            Assert.AreEqual(DatabaseFactoryFixture.OracleTestStoredProcedureInPackageWithoutTranslation,
                            createdObject.GetStoredProcCommand(
                                DatabaseFactoryFixture.OracleTestStoredProcedureInPackageWithoutTranslation).CommandText);
        }
#pragma warning restore 612, 618

        [TestMethod]
        public void CanCreateGenericDatabaseFromFactory()
        {
            var factory = new DatabaseProviderFactory(new SystemConfigurationSource(false).GetSection);

            GenericDatabase createdObject = (GenericDatabase)factory.Create("OdbcDatabase");
            Assert.IsNotNull(createdObject);
            Assert.AreEqual(@"some connection string;",
                            createdObject.ConnectionStringWithoutCredentials);
            Assert.AreEqual(DbProviderFactories.GetFactory("System.Data.Odbc"), createdObject.DbProviderFactory);
        }
    }
}
