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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Tests
{
    [TestClass]
    public class AddOracleConnectionElementNodeCommandFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void ExectueAddsDefaultNodes()
        {
            AddDatabaseSectionNodeCommand cmd = new AddDatabaseSectionNodeCommand(ServiceProvider);
            cmd.Execute(ApplicationNode);

            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            ConnectionStringSettingsNode connectionStringNode = (ConnectionStringSettingsNode)Hierarchy.FindNodeByType(typeof(ConnectionStringSettingsNode));
            AddOracleConnectionElementNodeCommand oracleCmd = new AddOracleConnectionElementNodeCommand(ServiceProvider);
            oracleCmd.Execute(connectionStringNode);

            Assert.IsNotNull(Hierarchy.FindNodeByType(connectionStringNode, typeof(OracleConnectionElementNode)));
        }
    }
}
