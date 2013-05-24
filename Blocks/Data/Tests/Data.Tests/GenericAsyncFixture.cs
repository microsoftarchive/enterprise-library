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
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public abstract class Given_GenericDatabaseInstance : ArrangeActAssert
    {
        protected Database Database { get; private set; }
        private const string northwind = @"server=(localdb)\v11.0;database=Northwind;Integrated Security=true";

        protected override void Arrange()
        {
            base.Arrange();

            Database = new GenericDatabase(northwind, SqlClientFactory.Instance);
        }
    }

    [TestClass]
    public class When_UsingGenericDatabase : Given_GenericDatabaseInstance
    {
        [TestMethod]
        public void Then_SupportsAsyncIsFalse()
        {
            Assert.IsFalse(Database.SupportsAsync);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Then_AsyncOperationThrows()
        {
            var command = Database.GetStoredProcCommand("Ten Most Popular Products");
            Database.BeginExecuteReader(command, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Then_AsyncAccessorThrows()
        {
            var accessor = Database.CreateSprocAccessor<object>("Ten Most Popular Products");
            accessor.BeginExecute(null, null);
        }
    }

}
