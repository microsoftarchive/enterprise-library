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

using System.Data.Common;
using System.Data.Odbc;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Configuration
{
    [TestClass]
    public class DatabaseWithObjectBuildUperFixture
    {
        public const string OracleTestStoredProcedureInPackageWithTranslation = "TESTPACKAGETOTRANSLATEGETCUSTOMERDETAILS";
		public const string OracleTestTranslatedStoredProcedureInPackageWithTranslation = "TESTPACKAGE.TESTPACKAGETOTRANSLATEGETCUSTOMERDETAILS";
		public const string OracleTestStoredProcedureInPackageWithoutTranslation = "TESTPACKAGETOKEEPGETCUSTOMERDETAILS";

        [TestMethod]
        public void CanCreateSqlDatabase()
        {
            Database database = EnterpriseLibraryFactory.BuildUp<Database>("Service_Dflt");
            Assert.IsNotNull(database);
            Assert.AreSame(typeof(SqlDatabase), database.GetType());
        }

        [TestMethod]
        public void CanCreateOracleDatabase()
        {
            Database database = EnterpriseLibraryFactory.BuildUp<Database>("OracleTest");
            Assert.IsNotNull(database);
            Assert.AreSame(typeof(OracleDatabase), database.GetType());

            // package mappings aren't exposed
            DbCommand dbCommand1 = database.GetStoredProcCommand(OracleTestStoredProcedureInPackageWithTranslation);
            Assert.AreEqual((object)OracleTestTranslatedStoredProcedureInPackageWithTranslation, dbCommand1.CommandText);

            DbCommand dbCommand2 = database.GetStoredProcCommand(OracleTestStoredProcedureInPackageWithoutTranslation);
            Assert.AreEqual((object)OracleTestStoredProcedureInPackageWithoutTranslation, dbCommand2.CommandText);
        }

        [TestMethod]
        public void CanCreateGenericDatabase()
        {
            Database database = EnterpriseLibraryFactory.BuildUp<Database>("OdbcDatabase");
            Assert.IsNotNull(database);
            Assert.AreSame(typeof(GenericDatabase), database.GetType());

            // provider factories aren't exposed
            DbCommand command = database.GetStoredProcCommand("ignore");
            Assert.AreSame(typeof(OdbcCommand), command.GetType());
        }

        [TestMethod]
        public void CreatedDatabaseIsInstrumented()
        {
            Database database = EnterpriseLibraryFactory.BuildUp<Database>("Service_Dflt");
        }
    }
}
