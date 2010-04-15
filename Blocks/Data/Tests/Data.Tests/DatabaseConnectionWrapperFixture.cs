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

using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    /// <summary>
    /// Summary description for DatabaseConnectionWrapperFixture
    /// </summary>
    [TestClass]
    public class DatabaseConnectionWrapperFixture
    {
        private DbConnection connection;

        [TestInitialize]
        public void Setup()
        {
            var factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            var db = factory.CreateDefault();
            connection = db.CreateConnection();
            connection.Open();
        }

        [TestCleanup]
        public void Teardown()
        {
            if(connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

        }

        [TestMethod]
        public void ConnectionIsClosedWhenDisposingWrapper()
        {
            DatabaseConnectionWrapper wrapper;
            using(wrapper = new DatabaseConnectionWrapper(connection))
            {
            }

            AssertDisposed(wrapper);
        }

        [TestMethod]
        public void AddRefRequiresExtraClose()
        {
            var wrapper = new DatabaseConnectionWrapper(connection);
            using(wrapper.AddRef())
            {
            }

            AssertNotDisposed(wrapper);
        }

        [TestMethod]
        public void MultipleDisposesCleanupMultipleAddRefs()
        {
            // Start at refcount 1
            var wrapper = new DatabaseConnectionWrapper(connection);

            wrapper.AddRef();
            wrapper.AddRef();

            wrapper.Dispose();
            AssertNotDisposed(wrapper);

            wrapper.Dispose();
            AssertNotDisposed(wrapper);

            wrapper.Dispose();
            AssertDisposed(wrapper);
        }

        private void AssertDisposed(DatabaseConnectionWrapper wrapper)
        {
            Assert.IsTrue(wrapper.IsDisposed);
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        private void AssertNotDisposed(DatabaseConnectionWrapper wrapper)
        {
            Assert.IsFalse(wrapper.IsDisposed);
            Assert.AreEqual(ConnectionState.Open, connection.State);
        }

    }
}
