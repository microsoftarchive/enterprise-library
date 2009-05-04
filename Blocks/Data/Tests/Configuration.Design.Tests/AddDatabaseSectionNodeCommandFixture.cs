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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Tests
{
    [TestClass]
    public class AddDatabaseSectionNodeCommandFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void ExectueAddsDefaultNodes()
        {
            AddDatabaseSectionNodeCommand cmd = new AddDatabaseSectionNodeCommand(ServiceProvider);
            cmd.Execute(ApplicationNode);

            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            ConfigurationNode node = Hierarchy.FindNodeByType(typeof(DatabaseSectionNode));
            Assert.IsNotNull(node);

            node = Hierarchy.FindNodeByType(typeof(ConnectionStringSettingsNode));
            Assert.IsNotNull(node);

            node = Hierarchy.FindNodeByType(typeof(ProviderMappingsNode));
            Assert.IsNotNull(node);
        }
    }
}
