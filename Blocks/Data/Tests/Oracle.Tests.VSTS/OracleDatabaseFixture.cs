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
using System.Data.OracleClient;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
#pragma warning disable 612, 618
    [TestClass]
    public class OracleDatabaseFixture
    {
        IConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            EnvironmentHelper.AssertOracleClientIsInstalled();
            configurationSource = OracleTestConfigurationSource.CreateConfigurationSource();
        }

        [TestMethod]
        public void CanConnectToOracleAndExecuteAReader()
        {
            var oracleDatabase = new DatabaseSyntheticConfigSettings(this.configurationSource).GetDatabase("OracleTest").BuildDatabase();

            DbConnection connection = oracleDatabase.CreateConnection();
            Assert.IsNotNull(connection);
            Assert.IsTrue(connection is OracleConnection);
            connection.Open();
            DbCommand cmd = oracleDatabase.GetSqlStringCommand("Select * from Region");
            cmd.CommandTimeout = 0;
        }

        [TestMethod]
        public void CanExecuteCommandWithEmptyPackages()
        {
            ConnectionStringSettings data = ConfigurationManager.ConnectionStrings["OracleTest"];

            OracleDatabase oracleDatabase = new OracleDatabase(data.ConnectionString);
            DbConnection connection = oracleDatabase.CreateConnection();
            Assert.IsNotNull(connection);
            Assert.IsTrue(connection is OracleConnection);
            connection.Open();
            DbCommand cmd = oracleDatabase.GetSqlStringCommand("Select * from Region");
            cmd.CommandTimeout = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructingAnOracleDatabaseWithNullPackageListThrows()
        {
            ConnectionStringSettings data = ConfigurationManager.ConnectionStrings["OracleTest"];

            new OracleDatabase(data.ConnectionString, null);
        }
    }
#pragma warning restore 612, 618
}
