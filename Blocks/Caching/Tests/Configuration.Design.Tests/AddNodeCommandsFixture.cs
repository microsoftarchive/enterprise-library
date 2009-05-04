//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
    [TestClass]
    public class AddNodeCommandsFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void EnsureExcutingAddCacheManagerSettingsSetsDefaults()
        {
            AddCacheManagerSettingsNodeCommand cmd = new AddCacheManagerSettingsNodeCommand(ServiceProvider);
            cmd.Execute(ApplicationNode);

            CacheManagerSettingsNode node = (CacheManagerSettingsNode)Hierarchy.FindNodeByType(typeof(CacheManagerSettingsNode));

            Assert.AreEqual(1, node.Nodes.Count);
        }
    }
}
