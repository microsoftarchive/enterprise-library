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
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            ConnectionString = @"server=(localdb)\v11.0;database=Northwind;Integrated Security=true; Async=True";
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
    public class WhenExecutingSqlString : SqlStringAccessorContext
    {
        [TestMethod]
        public void ThenConvertsResultInObjects()
        {
            var x = Database.ExecuteSqlStringAccessor<Product>("Select 'p' as Productname, 12 as UnitPrice");
            Assert.AreEqual(1, x.Count());
            Assert.AreEqual("p", x.First().ProductName);
            Assert.AreEqual(12, x.First().UnitPrice);
        }
    }

    [TestClass]
    public class WhenExecutingSqlStringPassingRowMapper : SqlStringAccessorContext
    {
        [TestMethod]
        public void ThenConvertsResultInObjectsUsingRowMapper()
        {
            var x = Database.ExecuteSqlStringAccessor<Product>("Select 'p' as Productname, 12 as UnitPrice", new RowMapper());
            Assert.AreEqual(1, x.Count());
            Assert.AreEqual("pname", x.First().ProductName);
            Assert.AreEqual(23, x.First().UnitPrice);
        }

        private class RowMapper : IRowMapper<Product>
        {
            public Product MapRow(IDataRecord row)
            {
                return new Product
                {
                    ProductName = "pname",
                    UnitPrice = 23
                };
            }
        }
    }

    [TestClass]
    public class WhenExecutingSqlStringPassingResultSetMapper : SqlStringAccessorContext
    {
        [TestMethod]
        public void ThenConvertsResultInObjectsUsingRowMapper()
        {
            var x = Database.ExecuteSqlStringAccessor<Product>("Select 'hello'", new ResultSetMapper());
            Assert.AreEqual(1, x.Count());
            Assert.AreEqual("pname", x.First().ProductName);
            Assert.AreEqual(23, x.First().UnitPrice);
        }

        private class ResultSetMapper : IResultSetMapper<Product>
        {
            public IEnumerable<Product> MapSet(IDataReader reader)
            {
                yield return new Product
                {
                    ProductName = "pname",
                    UnitPrice = 23
                };
            }
        }
    }

    [TestClass]
    public class WhenCreatingSqlStringAccessor : SqlStringAccessorContext
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
            var stringAccessor = Database.CreateSqlStringAccessor<Product>("SELECT * from Products", MapBuilder<Product>.MapNoProperties().Build());
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
            new SqlStringAccessor<Product>(null, "SELECT 'test'", MapBuilder<Product>.BuildAllProperties());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenPassingNullParameterMapperThrows()
        {
            Database.CreateSqlStringAccessor<Product>("SELECT 'test'", null, MapBuilder<Product>.BuildAllProperties());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThenCreateSqlStringAccessorWithEmptyArgThrowsArgumentException()
        {
            Database.CreateSqlStringAccessor<Product>(string.Empty);
        }
    }

    [TestClass]
    public class WhenExecutingSqlStringAccessor : SqlStringAccessorContext
    {
        private DataAccessor<Product> accessor;

        protected override void Arrange()
        {
            base.Arrange();

            accessor = Database.CreateSqlStringAccessor<Product>("SELECT TOP 5 * from Products");
        }

        [TestMethod]
        public void ThenReturnsEnumerable()
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
    public class WhenExecutingSqlStringAccessorAsynchronously : SqlStringAccessorContext
    {
        private DataAccessor<Product> accessor;
        private IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            accessor = Database.CreateSqlStringAccessor<Product>("Select 'book' as ProductName, 12 as UnitPrice");
            asyncResult = accessor.BeginExecute(null, null);
        }

        [TestMethod]
        public void ThenEndExecuteReturnsResultsAsEnumerable()
        {
            Assert.IsNotNull(accessor.EndExecute(asyncResult));
        }

        [TestMethod]
        public void ThenClosesConnectionAfterResultsAreEnumerated()
        {
            var result = accessor.EndExecute(asyncResult);
            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(ConnectionState.Closed, base.ConnectionState);
        }

        [TestMethod]
        public void ThenClosesConnectionEvenThoughEnumerationIsntFinished()
        {
            var result = accessor.EndExecute(asyncResult);
            var foo = result.First();

            Assert.AreEqual(ConnectionState.Closed, base.ConnectionState);
        }

        [TestMethod]
        public void ThenClosesConnectionAfterIteratingPartially()
        {
            var resultSet = accessor.EndExecute(asyncResult);

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
            var result = accessor.EndExecute(asyncResult).ToList();
            Assert.AreEqual(ConnectionState.Closed, base.ConnectionState);
        }


        //TODO: should we throw our own exception?
        //now it says: Invalid attempt to call Read when reader is closed.
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotReExecuteCommandByReiterating()
        {
            var results = accessor.EndExecute(asyncResult);
            int itCount1 = results.Count();
            int itCount2 = results.Count();
        }


        [TestMethod]
        public void ThenSetsPropertiesBasedOnPropertyName()
        {
            var result = accessor.EndExecute(asyncResult);
            Product firstProduct = result.First();
            Assert.IsNotNull(firstProduct.ProductName);
            Assert.AreNotEqual(0d, firstProduct.UnitPrice);
        }
    }

    [TestClass]
    public class WhenParameterizedSqlStringAccessorIsCreated : SqlStringAccessorContext
    {
        private DataAccessor<Product> accessor;

        protected override void Arrange()
        {
            base.Arrange();

            accessor = Database.CreateSqlStringAccessor<Product>("SELECT TOP 5 * from Products WHERE ProductName = @p1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PassingOnlyParameterValuesToExecuteThrows()
        {
            accessor.Execute("Chai").ToList();
        }
    }

    [TestClass]
    public class WhenParameterizedSqlStringAccessorIsCreatedWithParameterMapper : SqlStringAccessorContext
    {
        private DataAccessor<Product> accessor;
        private SqlParameterMapper parameterMapper;

        protected override void Arrange()
        {
            base.Arrange();
            parameterMapper = new SqlParameterMapper();
            accessor = Database.CreateSqlStringAccessor<Product>("SELECT TOP 5 * from Products WHERE ProductName = @p1", parameterMapper);
        }

        [TestMethod]
        public void ThenParameterMapperIsCalledOnceOnExecute()
        {
            accessor.Execute("Chai").ToList();
            Assert.AreEqual(1, parameterMapper.AssignParametersCallCount);
        }

        [TestMethod]
        public void ThenExecuteReturnsResultSet()
        {
            var result = accessor.Execute("Chai");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Chai", result.First().ProductName);
        }


        private class SqlParameterMapper : IParameterMapper
        {
            public int AssignParametersCallCount = 0;

            public void AssignParameters(DbCommand command, object[] parameterValues)
            {
                AssignParametersCallCount++;

                DbParameter parameter = command.CreateParameter();
                parameter.ParameterName = "@p1";
                parameter.Value = parameterValues.First();

                command.Parameters.Add(parameter);
            }
        }
    }

    [TestClass]
    public class WhenSqlStringAccessorIsMappingToNullableFields : SqlStringAccessorContext
    {
        private const string cleanupQuery = "Delete from products where ProductName='Test'";
        private const string insertQuery = "INSERT INTO [Northwind].[dbo].[Products]([ProductName],[SupplierID],[CategoryID],[QuantityPerUnit],[UnitPrice],[UnitsInStock],[UnitsOnOrder],[ReorderLevel],[Discontinued]) VALUES('Test',null,null,null,null,null,null,null,53)";

        private DataAccessor<ProductSupplier> accessor;

        public class ProductSupplier
        {

            public string ProductName { get; set; }
            public int? SupplierID { get; set; }
        }

        protected override void Arrange()
        {
            base.Arrange();

            Database.ExecuteNonQuery(CommandType.Text, cleanupQuery);
            Database.ExecuteNonQuery(CommandType.Text, insertQuery);

            accessor = Database.CreateSqlStringAccessor("Select ProductName,SupplierID from Products WHERE ProductName='Test'",
                MapBuilder<ProductSupplier>.BuildAllProperties());
        }

        protected override void Teardown()
        {
            Database.ExecuteNonQuery(CommandType.Text, cleanupQuery);
            base.Teardown();
        }

        [TestMethod]
        public void ThenNullableFieldIsProperlyMappedWhenRowContainsNull()
        {
            var suppliers = accessor.Execute().ToList();
            Assert.AreEqual(1, suppliers.Count);

            Assert.IsFalse(suppliers[0].SupplierID.HasValue);
        }
    }
}
