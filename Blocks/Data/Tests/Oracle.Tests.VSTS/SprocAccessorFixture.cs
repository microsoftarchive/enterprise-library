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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
#pragma warning disable 612, 618
    [TestClass]
    public class WhenSprocAccessorIsCreatedForOracleDatabase : ArrangeActAssert
    {
        OracleDatabase database;

        protected override void Arrange()
        {
            EnvironmentHelper.AssertOracleClientIsInstalled();
            database = new OracleDatabase("server=entlib;user id=testuser;password=testuser");
        }

        [TestMethod]
        public void ThenCanExecuteParameterizedSproc()
        {
            var accessor = database.CreateSprocAccessor<Customer>("GetCustomersTest");
            var result = accessor.Execute("BLAUS", null).ToArray();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        private class Customer
        {
            public string ContactName { get; set; }
        }
    }
#pragma warning restore 612, 618
}
