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
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            ConnectionString = @"server=(localdb)\v11.0;database=Northwind;Integrated Security=true; Async=True";
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
    public class WhenExecutingSproc : SprocAccessorContext
    {
        [TestMethod]
        public void ThenConvertsResultInObjects()
        {
            var x = Database.ExecuteSprocAccessor<Product>("Ten Most Expensive Products");
            Assert.AreEqual(10, x.Count());
            Assert.IsNotNull(x.First().TenMostExpensiveProducts);
            Assert.AreNotEqual(0, x.First().UnitPrice);
        }
    }

    [TestClass]
    public class WhenExecutingSprocPassingRowMapper : SprocAccessorContext
    {
        IRowMapper<Product> rowMapper;

        protected override void Arrange()
        {
            base.Arrange();

            rowMapper = new RowMapper();
        }

        [TestMethod]
        public void ThenConvertsResultInObjectsUsingRowMapper()
        {
            var x = Database.ExecuteSprocAccessor<Product>("Ten Most Expensive Products", rowMapper);
            Assert.AreEqual(10, x.Count());
            Assert.AreEqual("pname", x.First().TenMostExpensiveProducts);
            Assert.AreEqual(23, x.First().UnitPrice);
        }

        private class RowMapper : IRowMapper<Product>
        {
            public Product MapRow(IDataRecord row)
            {
                return new Product
                {
                    TenMostExpensiveProducts = "pname",
                    UnitPrice = 23
                };
            }
        }
    }

    [TestClass]
    public class WhenExecutingSprocPassingResultSetMapper : SprocAccessorContext
    {
        IResultSetMapper<Product> resultSetMapper;

        protected override void Arrange()
        {
            base.Arrange();

            resultSetMapper = new ResultSetMapper();
        }

        [TestMethod]
        public void ThenConvertsResultInObjectsUsingRowMapper()
        {
            var x = Database.ExecuteSprocAccessor<Product>("Ten Most Expensive Products", resultSetMapper);
            Assert.AreEqual(1, x.Count());
            Assert.AreEqual("pname", x.First().TenMostExpensiveProducts);
            Assert.AreEqual(23, x.First().UnitPrice);
        }

        private class ResultSetMapper : IResultSetMapper<Product>
        {
            public IEnumerable<Product> MapSet(IDataReader reader)
            {
                yield return new Product
                {
                    TenMostExpensiveProducts = "pname",
                    UnitPrice = 23
                };
            }
        }
    }

    [TestClass]
    public class WhenExecutingSprocPassingParameterMapper : SprocAccessorContext
    {
        IParameterMapper parameterMapper;

        protected override void Arrange()
        {
            base.Arrange();

            parameterMapper = new ParameterMapper();
        }

        [TestMethod]
        public void ThenConvertsResultInObjectsUsingRowMapper()
        {
            var x = Database.ExecuteSprocAccessor<ProductSales>("SalesByCategory", parameterMapper);
            Assert.IsNotNull(x);
            Assert.AreEqual("Chai", x.First().ProductName);
        }

        private class ParameterMapper : IParameterMapper
        {
            public void AssignParameters(DbCommand command, object[] parameterValues)
            {
                command.Parameters.Add(new SqlParameter("@CategoryName", "Beverages"));
            }
        }

        private class ProductSales
        {
            public string ProductName { get; set; }
            public double TotalPurchase { get; set; }
        }
    }

    [TestClass]
    public class WhenExecutingSprocPassingParameterMapperAndRowMapper : SprocAccessorContext
    {

        IParameterMapper parameterMapper;
        IRowMapper<ProductSales> rowMapper;

        protected override void Arrange()
        {
            base.Arrange();

            parameterMapper = new ParameterMapper();
            rowMapper = new RowMapper();
        }

        [TestMethod]
        public void ThenConvertsResultInObjectsUsingRowMapper()
        {
            var x = Database.ExecuteSprocAccessor<ProductSales>("SalesByCategory", parameterMapper, rowMapper);
            Assert.IsNotNull(x);
            Assert.AreEqual("pname", x.First().ProductName);
            Assert.AreEqual(12, x.First().TotalPurchase);
        }

        private class ParameterMapper : IParameterMapper
        {
            public void AssignParameters(DbCommand command, object[] parameterValues)
            {
                command.Parameters.Add(new SqlParameter("@CategoryName", "Beverages"));
            }
        }

        private class ProductSales
        {
            public string ProductName { get; set; }
            public double TotalPurchase { get; set; }
        }

        private class RowMapper : IRowMapper<ProductSales>
        {
            public ProductSales MapRow(IDataRecord row)
            {
                return new ProductSales
                {
                    ProductName = "pname",
                    TotalPurchase = 12
                };
            }
        }
    }

    [TestClass]
    public class WhenExecutingSprocPassingParameterMapperAndResultSetMapper : SprocAccessorContext
    {
        IParameterMapper parameterMapper;
        IResultSetMapper<ProductSales> resultSetMapper;

        protected override void Arrange()
        {
            base.Arrange();

            parameterMapper = new ParameterMapper();
            resultSetMapper = new ResultSetMapper();
        }

        [TestMethod]
        public void ThenConvertsResultInObjectsUsingRowMapper()
        {
            var x = Database.ExecuteSprocAccessor<ProductSales>("SalesByCategory", parameterMapper, resultSetMapper);
            Assert.IsNotNull(x);
            Assert.AreEqual(1, x.Count());
            Assert.AreEqual("pname", x.First().ProductName);
            Assert.AreEqual(12, x.First().TotalPurchase);
        }

        private class ParameterMapper : IParameterMapper
        {
            public void AssignParameters(DbCommand command, object[] parameterValues)
            {
                command.Parameters.Add(new SqlParameter("@CategoryName", "Beverages"));
            }
        }

        private class ProductSales
        {
            public string ProductName { get; set; }
            public double TotalPurchase { get; set; }
        }

        private class ResultSetMapper : IResultSetMapper<ProductSales>
        {
            public IEnumerable<ProductSales> MapSet(IDataReader reader)
            {
                yield return new ProductSales
                {
                    ProductName = "pname",
                    TotalPurchase = 12
                };
            }
        }
    }

    [TestClass]
    public class WhenCreatingSprocAccessor : SprocAccessorContext
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
            var sprocAccessor = Database.CreateSprocAccessor<Product>("Ten Most Expensive Products", MapBuilder<Product>.MapNoProperties().Build());
            Assert.IsNotNull(sprocAccessor);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenCreateSprocAccessorWithNullMapperThrows()
        {
            Database.CreateSprocAccessor<Product>("prodedure name", (IRowMapper<Product>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenCreateSprocAccessorWithNullResultSetMapperThrows()
        {
            Database.CreateSprocAccessor<Product>("prodedure name", (IResultSetMapper<Product>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenCreateSprocAccessorWithNullParameterMapperThrows()
        {
            Database.CreateSprocAccessor<Product>("prodedure name", null, MapBuilder<Product>.BuildAllProperties());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenCreateSprocAccessorWithNullDatabaseThrows()
        {
            new SprocAccessor<Product>(null, "procedure name", MapBuilder<Product>.BuildAllProperties());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThenCreateSprocAccessorWithEmptyArgThrowsArgumentException()
        {
            Database.CreateSprocAccessor<Product>(string.Empty);
        }
    }

    [TestClass]
    public class WhenExecutingSprocAccessor : SprocAccessorContext
    {
        private DataAccessor<Product> accessor;

        protected override void Arrange()
        {
            base.Arrange();

            accessor = Database.CreateSprocAccessor<Product>("Ten Most Expensive Products");
        }

        [TestMethod]
        public void ThenReturnsResultsAsEnumerable()
        {
            Assert.IsNotNull(accessor.Execute());
        }

        [TestMethod]
        public void ThenClosesConnectionAfterResultsAreEnumerated()
        {
            var result = accessor.Execute();
            Assert.AreEqual(10, result.Count());

            Assert.AreEqual(ConnectionState.Closed, base.ConnectionState);
        }

        [TestMethod]
        public void ThenClosesConnectionEvenThoughEnumerationIsntFinished()
        {
            var result = accessor.Execute();
            var foo = result.First();

            Assert.AreEqual(ConnectionState.Closed, base.ConnectionState);
        }

        [TestMethod]
        public void ThenClosesConnectionAfterIteratingPartially()
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
        public void ThenSetsPropertiesBasedOnPropertyName()
        {
            var result = accessor.Execute();
            Product firstProduct = result.First();
            Assert.IsNotNull(firstProduct.TenMostExpensiveProducts);
            Assert.AreNotEqual(0d, firstProduct.UnitPrice);
        }
    }

    [TestClass]
    public class WhenExecutingSprocAccessorAsynchronously : SprocAccessorContext
    {
        private DataAccessor<Product> accessor;
        private IAsyncResult asyncResult;

        protected override void Arrange()
        {
            base.Arrange();

            accessor = Database.CreateSprocAccessor<Product>("Ten Most Expensive Products");
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
            Assert.AreEqual(10, result.Count());

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
            Assert.IsNotNull(firstProduct.TenMostExpensiveProducts);
            Assert.AreNotEqual(0d, firstProduct.UnitPrice);
        }
    }

    [TestClass]
    public class WhenParameterizedSprocAccessorIsCreated : SprocAccessorContext
    {
        private DataAccessor<ProductSales> accessor;

        protected override void Arrange()
        {
            base.Arrange();

            accessor = Database.CreateSprocAccessor<ProductSales>("SalesByCategory");
        }

        [TestMethod]
        public void ThenCanPassParameterInExecute()
        {
            var result = accessor.Execute("Beverages", "1998");
            Assert.IsNotNull(result);
            var enumerared = result.ToList();
        }


        private class ProductSales
        {
            public string ProductName { get; set; }
            public double TotalPurchase { get; set; }
        }
    }

    [TestClass]
    public class WhenSprocAccessorIsCreatedPassingCustomRowMapper : SprocAccessorContext
    {
        private DataAccessor<Product> accessor;
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
        private DataAccessor<Product> accessor;
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
            accessor.Execute().ToList();
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

    [TestClass]
    public class WhenSprocAccessorIsCreatedPassingParameterMapper : SprocAccessorContext
    {
        private DataAccessor<ProductSales> accessor;
        private SqlParameterMapper parameterMapper;

        protected override void Arrange()
        {
            base.Arrange();
            parameterMapper = new SqlParameterMapper();

            accessor = Database.CreateSprocAccessor<ProductSales>("SalesByCategory", parameterMapper);
        }

        [TestMethod]
        public void ThenParameterMapperIsCalledOnceOnExecute()
        {
            var result = accessor.Execute("Beverages").ToList();
            Assert.AreEqual(1, parameterMapper.AssignParametersCallCount);
        }

        [TestMethod]
        public void ThenParameterMapperOutputIsUsedToExecuteSproc()
        {
            var result = accessor.Execute("Beverages");
            Assert.IsNotNull(result);
            Assert.AreEqual("Chai", result.First().ProductName);
        }

        private class SqlParameterMapper : IParameterMapper
        {
            public int AssignParametersCallCount = 0;

            public void AssignParameters(DbCommand command, object[] parameterValues)
            {
                AssignParametersCallCount++;

                DbParameter parameter = command.CreateParameter();
                parameter.ParameterName = "@CategoryName";
                parameter.Value = parameterValues.First();

                command.Parameters.Add(parameter);
            }
        }

        private class ProductSales
        {
            public string ProductName { get; set; }
            public double TotalPurchase { get; set; }
        }
    }


    [TestClass]
    public class When_UsingSprocAccessorAgainstGenericDatabase : Given_GenericDatabaseInstance
    {
        [TestMethod]
        public void Then_CanCallNonParameterizedSproc()
        {
            var accessor = Database.CreateSprocAccessor<object>("Ten Most Expensive Products");
            var result = accessor.Execute();

            Assert.AreEqual(10, result.Count());
        }

        [TestMethod]
        public void Then_CallingParemeterizedSpocThrowsException()
        {
            var accessor = Database.CreateSprocAccessor<object>("SalesByCategory");

            try
            {
                var result = accessor.Execute("Chai");
                Assert.Fail();
            }
            catch (InvalidOperationException ioe)
            {
                Assert.IsTrue(ioe.Message.Contains("IParameterMapper"));
            }
        }
    }


}
