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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    /// <summary>
    /// Summary description for OracleDatabaseWithContainerFixture
    /// </summary>
    [TestClass]
    public class OracleDatabaseWithContainerFixture
    {
        public const string OracleTestStoredProcedureInPackageWithTranslation = "TESTPACKAGETOTRANSLATEGETCUSTOMERDETAILS";
        public const string OracleTestTranslatedStoredProcedureInPackageWithTranslation = "TESTPACKAGE.TESTPACKAGETOTRANSLATEGETCUSTOMERDETAILS";
        public const string OracleTestStoredProcedureInPackageWithoutTranslation = "TESTPACKAGETOKEEPGETCUSTOMERDETAILS";

        [TestMethod]
        public void CanCreateOracleDatabase()
        {
            Database database = EnterpriseLibraryContainer.Current.GetInstance<Database>("OracleTest");
            Assert.IsNotNull(database);
            Assert.AreSame(typeof(OracleDatabase), database.GetType());

            // package mappings aren't exposed
            DbCommand dbCommand1 = database.GetStoredProcCommand(OracleTestStoredProcedureInPackageWithTranslation);
            Assert.AreEqual((object)OracleTestTranslatedStoredProcedureInPackageWithTranslation, dbCommand1.CommandText);

            DbCommand dbCommand2 = database.GetStoredProcCommand(OracleTestStoredProcedureInPackageWithoutTranslation);
            Assert.AreEqual((object)OracleTestStoredProcedureInPackageWithoutTranslation, dbCommand2.CommandText);
        }
    }
}
