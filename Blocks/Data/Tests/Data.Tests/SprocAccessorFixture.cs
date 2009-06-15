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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public abstract class SprocAccessorContext : ArrangeActAssert
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
            public string TenMostExpensiveProducts { get; set; }
            public decimal UnitPrice { get; set; }
        }

        private class TestableSqlDatabase : SqlDatabase
        {
            SprocAccessorContext context;
            public TestableSqlDatabase(string connectionstring, SprocAccessorContext context)
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
    public class WhenDatabaseIsInitialized : SprocAccessorContext
    {
        [TestMethod]
        public void ThenCanCreateSprocAccessor()
        {
            var sprocAccessor = Database.CreateSprocAccessor<Product>("Ten Most Expensive Products");
            Assert.IsNotNull(sprocAccessor);
        }

        [TestMethod]
        public void ThenCanCreateSprocAccessorWithRowMapper()
        {
            var sprocAccessor = Database.CreateSprocAccessor<Product>("Ten Most Expensive Products", new MapBuilder<Product>().BuildMapper());
            Assert.IsNotNull(sprocAccessor);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenCreateSprocAccessorWithNullMapperThrows()
        {
            Database.CreateSprocAccessor<Product>("prodedure name", (IRowMapper<Product>) null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenCreateSprocAccessorWithNullResultSetMapperThrows()
        {
            Database.CreateSprocAccessor<Product>("prodedure name", (IResultSetMapper<Product>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenCreateSprocAccessorWithNullDatabaseThrows()
        {
            new SprocAccessor<Product>(null, "procedure name", new MapBuilder<Product>().BuildMapper());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThenCreateSprocAccessorWithEmptyArgThrowsArgumentException()
        {
            Database.CreateSprocAccessor<Product>(string.Empty);
        }
    }

    [TestClass]
    public class WhenSprocAccessorIsCreated : SprocAccessorContext
    {
        private SprocAccessor<Product> accessor;

        protected override void Arrange()
        {
            base.Arrange();

            accessor = Database.CreateSprocAccessor<Product>("Ten Most Expensive Products");
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
            Assert.AreEqual(10, result.Count());

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

        [TestMethod]
        public void ThenExecuteSetsPropertiesBasedOnPropertyName()
        {
            var result = accessor.Execute();
            Product firstProduct = result.First();
            Assert.IsNotNull(firstProduct.TenMostExpensiveProducts);
            Assert.AreNotEqual(0d, firstProduct.UnitPrice);
        }
    }

    [TestClass]
    public class WhenParameterizedSprocAccessorIsCreated : SprocAccessorContext
    {
        private SprocAccessor<Product> accessor;

        protected override void Arrange()
        {
            base.Arrange();

            accessor = Database.CreateSprocAccessor<Product>("SalesByCategory");
        }

        [TestMethod]
        public void ThenCanPassParameterInExecute()
        {
            var result = accessor.Execute("Beverages", "1998");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [Ignore]
        public void ThenCanOmitOptionalParameters()
        {
            var result = accessor.Execute("Beverages");
            Assert.IsNotNull(result);
        }
    }

    [TestClass]
    public class WhenSprocAccessorIsCreatedPassingCustomRowMapper : SprocAccessorContext
    {
        private SprocAccessor<Product> accessor;
        private CustomMapper mapper;

        protected override void Arrange()
        {
            base.Arrange();

            mapper = new CustomMapper();
            accessor = Database.CreateSprocAccessor<Product>("Ten Most Expensive Products", mapper);
        }

        [TestMethod]
        public void ThenMapperIsCalledForEveryRow()
        {
            accessor.Execute().ToList();
            Assert.AreEqual(10, mapper.MapRowCallCount);
        }

        private class CustomMapper : IRowMapper<Product>
        {
            public int MapRowCallCount = 0;

            public Product MapRow(IDataRecord row)
            {
                MapRowCallCount++;
                return new Product();
            }
        }
    }

    [TestClass]
    public class WhenSprocAccessorIsCreatedPassingCustomResultSetMapper : SprocAccessorContext
    {
        private SprocAccessor<Product> accessor;
        private CustomMapper mapper;

        protected override void Arrange()
        {
            base.Arrange();

            mapper = new CustomMapper();
            accessor = Database.CreateSprocAccessor<Product>("Ten Most Expensive Products", mapper);
        }


        [TestMethod]
        public void ThenMapperIsCalledOncePerExecute()
        {
            accessor.Execute();
            Assert.AreEqual(1, mapper.MapSetCallCount);
        }

        private class CustomMapper : IResultSetMapper<Product>
        {
            public int MapSetCallCount = 0;

            public IEnumerable<Product> MapSet(IDataReader reader)
            {
                MapSetCallCount++;
                return Enumerable.Empty<Product>();
            }
        }
    }
}
