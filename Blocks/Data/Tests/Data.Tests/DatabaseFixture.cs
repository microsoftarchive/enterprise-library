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
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    [TestClass]
    public class DatabaseFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructDatabaseWithNullConnectionStringThrows()
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            new TestDatabase(null, factory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructDatabaseWithNullDbProviderFactoryThrows()
        {
            new TestDatabase("foo", null);
        }

        class TestDatabase : Database
        {
            public TestDatabase(string connectionString,
                                DbProviderFactory dbProviderFactory)
                : base(connectionString, dbProviderFactory) {}

            //protected override char ParameterToken
            //{
            //    get { return 'a'; }
            //}

            protected override void DeriveParameters(DbCommand discoveryCommand) {}
        }
    }
}
