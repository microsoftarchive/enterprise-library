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
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public abstract class SqlStringAccessorContext : ArrangeActAssert
    {
        protected ConnectionState ConnectionState;
        protected int NumberOfConnectionsCreated;
        protected string ConnectionString;
        protected Database Database;

        protected override void Arrange()
        {
            ConnectionString = @"server=(local)\SQLEXPRESS;database=Northwind;Integrated Security=true";
            Database = new TestableSqlDatabase(ConnectionString, this);
        }

        protected class Product
        {
            public string ProductName { get; set; }
            public decimal UnitPrice { get; set; }
        }

        private class TestableSqlDatabase : SqlDatabase
        {
            SqlStringAccessorContext context;
            public TestableSqlDatabase(string connectionstring, SqlStringAccessorContext context)
                : base(connectionstring)
            {
                this.context = context;
            }

            public override DbConnection CreateConnection()
            {
                context.NumberOfConnectionsCreated++;

                DbConnection connection = base.CreateConnection();
                connection.StateChange += (sender, args) => { context.ConnectionState = args.CurrentState; };
                return connection;
            }
        }
    }

    [TestClass]
    public class WhenCreateSqlStringAccessor : SqlStringAccessorContext
    {
        [TestMethod]
        public void ThenCanCreateSqlStringAccessor()
        {
            var stringAccessor = Database.CreateSqlStringAccessor<Product>("SELECT * from Products");
            Assert.IsNotNull(stringAccessor);
        }

        [TestMethod]
        public void ThenCanCreateSqlStringAccessorWithRowMapper()
        {
            var stringAccessor = Database.CreateSqlStringAccessor<Product>("SELECT * from Products", new MapBuilder<Product>().BuildMapper());
            Assert.IsNotNull(stringAccessor);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenCreateSqlStringAccessorWithNullMapperThrows()
        {
            Database.CreateSqlStringAccessor<Product>("SELECT 'test'", (IRowMapper<Product>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenCreateSqlStringAccessorWithNullResultSetMapperThrows()
        {
            Database.CreateSqlStringAccessor<Product>("SELECT 'test'", (IResultSetMapper<Product>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenCreateSqlStringAccessorWithNullDatabaseThrows()
        {
            new SqlStringAccessor<Product>(null, "SELECT 'test'", new MapBuilder<Product>().BuildMapper());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThenCreateSqlStringAccessorWithEmptyArgThrowsArgumentException()
        {
            Database.CreateSqlStringAccessor<Product>(string.Empty);
        }
    }

    [TestClass]
    public class WhenSqlStringAccessorIsCreated : SqlStringAccessorContext
    {
        private SqlStringAccessor<Product> accessor;

        protected override void Arrange()
        {
            base.Arrange();

            accessor = Database.CreateSqlStringAccessor<Product>("SELECT TOP 5 * from Products");
        }

        [TestMethod]
        public void ThenExecuteReturnsEnumerable()
        {
            Assert.IsNotNull(accessor.Execute());
        }


        [TestMethod]
        public void ThenExecuteReturnsSprocResults()
        {
            var result = accessor.Execute();
            Assert.AreEqual(5, result.Count());

            Assert.AreEqual(ConnectionState.Closed, base.ConnectionState);
        }

        [TestMethod]
        public void ThenConnectionIsClosedEvenThoughEnumerationIsntFinished()
        {
            var result = accessor.Execute();

            Assert.AreEqual(ConnectionState.Open, base.ConnectionState);
            var foo = result.First();

            Assert.AreEqual(ConnectionState.Closed, base.ConnectionState);
        }

        [TestMethod]
        public void ThenConnectionIsClosedAfterIteratingPartially()
        {
            var resultSet = accessor.Execute();

            int i = 0;
            foreach (var result in resultSet)
            {
                i++;
                if (i == 3) break;
            }

            Assert.AreEqual(ConnectionState.Closed, base.ConnectionState);
        }

        [TestMethod]
        public void ThenConnectionIsClosedAfterExecuting()
        {
            var result = accessor.Execute().ToList();
            Assert.AreEqual(ConnectionState.Closed, base.ConnectionState);
        }
    }

    [TestClass]
    public class WhenParameterizedSqlStringAccessorIsCreated : SqlStringAccessorContext
    {
        private SqlStringAccessor<Product> accessor;

        protected override void Arrange()
        {
            base.Arrange();

            accessor = Database.CreateSqlStringAccessor<Product>("SELECT TOP 5 * from Products WHERE ProductName = @p1");
        }

        [TestMethod]
        public void ThenCanPassParameterInExecute()
        {
            //todo: check
            var result = accessor.Execute(new SqlParameter("p1", "Chai"));
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
    }
}
