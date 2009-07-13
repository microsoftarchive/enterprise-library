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
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public abstract class MapBuilderContext<TResult> : ArrangeActAssert
        where TResult : new()
    {
        protected IMapBuilderContext<TResult> builder;

        protected override void Arrange()
        {
            builder = MapBuilder<TResult>.MapNoProperties();
        }
    }

    [TestClass]
    public class WhenMapBuilderBuildsMapper : MapBuilderContext<WhenMapBuilderBuildsMapper.MapResult>
    {
        IRowMapper<MapResult> mapper;

        protected override void Arrange()
        {
            base.Arrange();
            mapper = builder.Build();
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
            ReflectionRowMapper<Customer> mapper = builder.Build() as ReflectionRowMapper<Customer>;

            var mappings = mapper.GetPropertyMappings();
            Assert.AreEqual(0, mappings.Count());
        }

        [TestMethod]
        public void ThenMapByNameMatchCreatesPropertyMappingWithAppropriateColumnName()
        {
            IMapBuilderContext<Customer> context = builder.MapByName(x => x.CustomerName);
            ReflectionRowMapper<Customer> mapper = context.Build() as ReflectionRowMapper<Customer>;

            var mappings = mapper.GetPropertyMappings().OfType<ColumnNameMapping>();

            Assert.AreNotEqual(0, mappings.Count());
            Assert.AreSame(CustomerNameProperty, mappings.First().Property);
            Assert.AreEqual("CustomerName", mappings.First().ColumnName);
        }

        [TestMethod]
        public void ThenMapWithFuncCreatesPropertyMappingWithFunc()
        {
            IMapBuilderContext<Customer> context = MapBuilder<Customer>.MapNoProperties()
                .Map(x => x.CustomerName).WithFunc(x => "value");

            ReflectionRowMapper<Customer> mapper = context.Build() as ReflectionRowMapper<Customer>;

            var mappings = mapper.GetPropertyMappings().OfType<FuncMapping>();
            Assert.AreNotEqual(0, mappings.Count());
            
            var propertyMapping = mappings.First();
            Assert.AreSame(CustomerNameProperty, propertyMapping.Property);
            Assert.IsNotNull(propertyMapping.Func);
        }

        [TestMethod]
        public void ThenMapToColumnCreatesPropertyMappingWithColumnName()
        {
            IMapBuilderContext<Customer> context = MapBuilder<Customer>.MapNoProperties()
                .Map(x => x.CustomerName).ToColumn("columnname");

            ReflectionRowMapper<Customer> mapper = context.Build() as ReflectionRowMapper<Customer>;

            var mappings = mapper.GetPropertyMappings().OfType<ColumnNameMapping>();
            Assert.AreNotEqual(0, mappings.Count());

            var mapping = mappings.First();
            Assert.AreSame(CustomerNameProperty, mapping.Property);
            Assert.AreEqual("columnname", mapping.ColumnName);
        }

        [TestMethod]
        public void ThenDoNotMapCreatesPropertyMappingWithIgnoreIsTrue()
        {
            IMapBuilderContext<Customer> context = MapBuilder<Customer>.MapNoProperties()
                .DoNotMap(x => x.CustomerName);

            ReflectionRowMapper<Customer> mapper = context.Build() as ReflectionRowMapper<Customer>;

            var mappings = mapper.GetPropertyMappings().OfType<IgnoreMapping>();
            Assert.AreNotEqual(0, mappings.Count());

            var mapping = mappings.First();
            Assert.AreSame(CustomerNameProperty, mapping.Property);
        }

        [TestMethod]
        public void ThenSubsequentMappingOnSamePropertyReplacesOldMapping()
        {
            IMapBuilderContext<Customer> context = MapBuilder<Customer>.MapNoProperties()
                .DoNotMap(x => x.CustomerName)
                .MapByName(x => x.CustomerName);

            ReflectionRowMapper<Customer> mapper = context.Build() as ReflectionRowMapper<Customer>;

            var mappings = mapper.GetPropertyMappings().OfType<ColumnNameMapping>();
            Assert.AreNotEqual(0, mappings.Count());

            var mapping = mappings.First();
            Assert.AreSame(CustomerNameProperty, mapping.Property);
            Assert.AreEqual("CustomerName", mapping.ColumnName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThenMappingToFieldThrows()
        {
            IMapBuilderContext<Customer> context = MapBuilder<Customer>.MapNoProperties()
                .MapByName(x => x.Field);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThenPassingCreationExpressionToMapThrows()
        {
            IMapBuilderContext<Customer> context = MapBuilder<Customer>.MapNoProperties()
                .MapByName(x => new string('c', 1));
        }

        [TestMethod]
        public void DoNotMapWillNormalizeInputProperty()
        {
            PropertyInfo maleOrFemale = typeof(Person).GetProperty("MaleOrFemale");

            ReflectionRowMapper<Customer> context = MapBuilder<Customer>.MapAllProperties()
                .DoNotMap(maleOrFemale).Build() as ReflectionRowMapper<Customer>;

            Assert.AreEqual(2, context.GetPropertyMappings().Count());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DoNotMapWithNullArgumentWillThrow()
        {
            MapBuilder<Customer>.MapAllProperties()
                .DoNotMap(null);
        }


        [TestMethod]
        public void MapByNameWillNormalizeInputProperty()
        {
            PropertyInfo maleOrFemale = typeof(Person).GetProperty("MaleOrFemale");

            ReflectionRowMapper<Customer> context = MapBuilder<Customer>.MapAllProperties()
                .MapByName(maleOrFemale).Build() as ReflectionRowMapper<Customer>;

            Assert.AreEqual(2, context.GetPropertyMappings().Count());
        }

        [TestMethod]
        public void MapToColumnWillNormalizeInputProperty()
        {
            PropertyInfo maleOrFemale = typeof(Person).GetProperty("MaleOrFemale");

            ReflectionRowMapper<Customer> context = MapBuilder<Customer>.MapAllProperties()
                .Map(maleOrFemale).ToColumn("sourceColum").Build() as ReflectionRowMapper<Customer>;

            Assert.AreEqual(2, context.GetPropertyMappings().Count());
        }


        [TestMethod]
        public void MapWithFuncWillNormalizeInputProperty()
        {
            PropertyInfo maleOrFemale = typeof(Person).GetProperty("MaleOrFemale");

            ReflectionRowMapper<Customer> context = MapBuilder<Customer>.MapAllProperties()
                .Map(maleOrFemale).WithFunc( x => 'm').Build() as ReflectionRowMapper<Customer>;

            Assert.AreEqual(2, context.GetPropertyMappings().Count());
        }


        [TestMethod]
        public void ThenCanOverwritePropertyInfoFromBaseClass()
        {
            ReflectionRowMapper<Customer> context = MapBuilder<Customer>.MapAllProperties()
                .DoNotMap(x => x.MaleOrFemale).Build() as ReflectionRowMapper<Customer>;

            Assert.AreEqual(2, context.GetPropertyMappings().Count());
        }


        [TestMethod]
        public void ThenNoMappingsAreMadeForIndexedProperty()
        {
            ReflectionRowMapper<ContainsProperrtyWithIndexer> context = MapBuilder<ContainsProperrtyWithIndexer>.BuildAllProperties() as ReflectionRowMapper<ContainsProperrtyWithIndexer>;

            Assert.AreEqual(1, context.GetPropertyMappings().Count());
        }

        public class Customer : Person
        {
            public string CustomerName { get; set; }

            public string Field;
        }

        public class Person
        {
            public char MaleOrFemale { get; set; }
        }

        public class ContainsProperrtyWithIndexer
        {
            public string NormalPropeerty { get; set; }

            public string this[int index]
            {
                get
                {
                    return index.ToString();
                }
                set
                {
                }

            }
        }
    }

}
