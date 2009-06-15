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
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public abstract class MapBuilderContext<TResult> : ArrangeActAssert
        where TResult : new()
    {
        protected IMapBuilderContext<TResult> builder;

        protected override void Arrange()
        {
            builder = new MapBuilder<TResult>();
        }
    }

    [TestClass]
    public class WhenMapBuilderBuildsMapper : MapBuilderContext<WhenMapBuilderBuildsMapper.MapResult>
    {
        IRowMapper<MapResult> mapper;

        protected override void Arrange()
        {
            base.Arrange();
            mapper = builder.BuildMapper();
        }

        [TestMethod]
        public void ThenMapBuilderIsReflectionMapper()
        {
            Assert.IsInstanceOfType(mapper, typeof(ReflectionRowMapper<MapResult>));
        }

        public class MapResult
        {
        }
    }

    [TestClass]
    public class WhenMapperIsBuildForType : MapBuilderContext<WhenMapperIsBuildForType.Customer>
    {
        protected PropertyInfo CustomerNameProperty;

        protected override void Arrange()
        {
            base.Arrange();

            CustomerNameProperty = typeof(Customer).GetProperty("CustomerName");
        }

        [TestMethod]
        public void ThenObjectPropertiesAreHiddenFromIntellisense()
        {
            typeof(IFluentInterface).IsAssignableFrom(typeof(IMapBuilderContext<Customer>));
        }

        [TestMethod]
        public void ThenCanMapByNamePassingProperty()
        {
            IMapBuilderContext<Customer> context = builder.MapByName(x => x.CustomerName);
        }

        [TestMethod]
        public void ThenCreatedMapperContains0PropertyMappings()
        {
            ReflectionRowMapper<Customer> mapper = builder.BuildMapper() as ReflectionRowMapper<Customer>;

            var mappings = mapper.GetPropertyMappings();
            Assert.AreEqual(0, mappings.Count());
        }

        [TestMethod]
        public void ThenMapByNameMatchCreatesPropertyMappingWithAppropriateColumnName()
        {
            MapBuilder<Customer> context = (MapBuilder<Customer>)builder.MapByName(x => x.CustomerName);
            ReflectionRowMapper<Customer> mapper = context.BuildMapper() as ReflectionRowMapper<Customer>;

            var mappings = mapper.GetPropertyMappings().OfType<ColumnNameMapping>();

            Assert.AreNotEqual(0, mappings.Count());
            Assert.AreSame(CustomerNameProperty, mappings.First().Property);
            Assert.AreEqual("CustomerName", mappings.First().ColumnName);
        }

        [TestMethod]
        public void ThenMapWithFuncCreatesPropertyMappingWithFunc()
        {
            MapBuilder<Customer> context = (MapBuilder<Customer>)builder
                .Map(x => x.CustomerName).WithFunc(x => "value");

            ReflectionRowMapper<Customer> mapper = context.BuildMapper() as ReflectionRowMapper<Customer>;

            var mappings = mapper.GetPropertyMappings().OfType<FuncMapping>();
            Assert.AreNotEqual(0, mappings.Count());
            
            var propertyMapping = mappings.First();
            Assert.AreSame(CustomerNameProperty, propertyMapping.Property);
            Assert.IsNotNull(propertyMapping.Func);
        }

        [TestMethod]
        public void ThenMapToColumnCreatesPropertyMappingWithColumnName()
        {
            MapBuilder<Customer> context = (MapBuilder<Customer>)builder
                .Map(x => x.CustomerName).ToColumn("columnname");

            ReflectionRowMapper<Customer> mapper = context.BuildMapper() as ReflectionRowMapper<Customer>;

            var mappings = mapper.GetPropertyMappings().OfType<ColumnNameMapping>();
            Assert.AreNotEqual(0, mappings.Count());

            var mapping = mappings.First();
            Assert.AreSame(CustomerNameProperty, mapping.Property);
            Assert.AreEqual("columnname", mapping.ColumnName);
        }

        [TestMethod]
        public void ThenDoNotMapCreatesPropertyMappingWithIgnoreIsTrue()
        {
            MapBuilder<Customer> context = (MapBuilder<Customer>)builder
                .DoNotMap(x => x.CustomerName);

            ReflectionRowMapper<Customer> mapper = context.BuildMapper() as ReflectionRowMapper<Customer>;

            var mappings = mapper.GetPropertyMappings().OfType<IgnoreMapping>();
            Assert.AreNotEqual(0, mappings.Count());

            var mapping = mappings.First();
            Assert.AreSame(CustomerNameProperty, mapping.Property);
        }

        [TestMethod]
        public void ThenSubsequentMappingOnSamePropertyReplacesOldMapping()
        {
            MapBuilder<Customer> context = (MapBuilder<Customer>)builder
                .DoNotMap(x => x.CustomerName)
                .MapByName(x => x.CustomerName);

            ReflectionRowMapper<Customer> mapper = context.BuildMapper() as ReflectionRowMapper<Customer>;

            var mappings = mapper.GetPropertyMappings().OfType<ColumnNameMapping>();
            Assert.AreNotEqual(0, mappings.Count());

            var mapping = mappings.First();
            Assert.AreSame(CustomerNameProperty, mapping.Property);
            Assert.AreEqual("CustomerName", mapping.ColumnName);
        }

        public class Customer
        {
            public string CustomerName { get; set; }
        }
    }

}
