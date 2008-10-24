//===============================================================================
// Microsoft patterns & practices Enterprise Library
// SQL Configuration Source QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Design.Tests
{
    [TestClass]
    public class SqlConfigurationSourceElementNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void SqlConfigurationSourceElementNodeDefaults()
        {
            SqlConfigurationSourceElementNode sqlConfigNode = new SqlConfigurationSourceElementNode();
            Assert.AreEqual("Sql Configuration Source", sqlConfigNode.Name);
            Assert.AreEqual(String.Empty, sqlConfigNode.ConnectionString);
            Assert.AreEqual(String.Empty, sqlConfigNode.GetStoredProcedure);
            Assert.AreEqual(String.Empty, sqlConfigNode.SetStoredProcedure);
            Assert.AreEqual(String.Empty, sqlConfigNode.RefreshStoredProcedure);
            Assert.AreEqual(String.Empty, sqlConfigNode.RemoveStoredProcedure);
            Assert.AreEqual(typeof(SqlConfigurationSource), sqlConfigNode.Type);
        }
    }
}
