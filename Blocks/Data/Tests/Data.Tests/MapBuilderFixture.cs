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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var mappings = ((IMapBuilderContextTest<Customer>)builder).GetPropertyMappings();
            Assert.AreEqual(0, mappings.Count());
        }

        [TestMethod]
        public void ThenMapByNameMatchCreatesPropertyMappingWithAppropriateColumnName()
        {
            IMapBuilderContext<Customer> context = builder.MapByName(x => x.CustomerName);

            var mappings = ((IMapBuilderContextTest<Customer>)builder).GetPropertyMappings().OfType<ColumnNameMapping>();

            Assert.AreNotEqual(0, mappings.Count());
            Assert.AreSame(CustomerNameProperty, mappings.First().Property);
            Assert.AreEqual("CustomerName", mappings.First().ColumnName);
        }

        [TestMethod]
        public void ThenMapWithFuncCreatesPropertyMappingWithFunc()
        {
            IMapBuilderContext<Customer> context = MapBuilder<Customer>.MapNoProperties()
                .Map(x => x.CustomerName).WithFunc(x => "value");

            var mappings = ((IMapBuilderContextTest<Customer>)context).GetPropertyMappings().OfType<FuncMapping>();
            Assert.AreNotEqual(0, mappings.Count());

            var propertyMapping = mappings.First();
            Assert.AreSame(CustomerNameProperty, propertyMapping.Property);
            Assert.IsNotNull(propertyMapping.Func);
        }

        [TestMethod]
        public void ThenMapWithNullFuncThrows()
        {
            var map = MapBuilder<Customer>.MapNoProperties().Map(x => x.CustomerName);

            try
            {
                map.WithFunc(null);
                Assert.Fail("should have thrown");
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void ThenMapToColumnCreatesPropertyMappingWithColumnName()
        {
            IMapBuilderContext<Customer> context = MapBuilder<Customer>.MapNoProperties()
                .Map(x => x.CustomerName).ToColumn("columnname");

            var mappings = ((IMapBuilderContextTest<Customer>)context).GetPropertyMappings().OfType<ColumnNameMapping>();
            Assert.AreNotEqual(0, mappings.Count());

            var mapping = mappings.First();
            Assert.AreSame(CustomerNameProperty, mapping.Property);
            Assert.AreEqual("columnname", mapping.ColumnName);
        }

        [TestMethod]
        public void ThenDoNotMapRemovesPropertyMapping()
        {
            IMapBuilderContext<Customer> context = MapBuilder<Customer>.MapAllProperties()
                .DoNotMap(x => x.CustomerName);

            var mappings =
                ((IMapBuilderContextTest<Customer>)context).GetPropertyMappings().Where(pm => pm.Property == CustomerNameProperty);
            Assert.AreEqual(0, mappings.Count());
        }

        [TestMethod]
        public void ThenSubsequentMappingOnSamePropertyReplacesOldMapping()
        {
            IMapBuilderContext<Customer> context = MapBuilder<Customer>.MapNoProperties()
                .DoNotMap(x => x.CustomerName)
                .MapByName(x => x.CustomerName);

            var mappings = ((IMapBuilderContextTest<Customer>)context).GetPropertyMappings().OfType<ColumnNameMapping>();
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

            var context = MapBuilder<Customer>.MapAllProperties()
                .DoNotMap(maleOrFemale) as IMapBuilderContextTest<Customer>;

            Assert.AreEqual(1, context.GetPropertyMappings().Count());
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

            var context = MapBuilder<Customer>.MapAllProperties()
                .MapByName(maleOrFemale) as IMapBuilderContextTest<Customer>;

            Assert.AreEqual(2, context.GetPropertyMappings().Count());
        }

        [TestMethod]
        public void MapToColumnWillNormalizeInputProperty()
        {
            PropertyInfo maleOrFemale = typeof(Person).GetProperty("MaleOrFemale");

            var context = MapBuilder<Customer>.MapAllProperties()
                .Map(maleOrFemale).ToColumn("sourceColum") as IMapBuilderContextTest<Customer>;

            Assert.AreEqual(2, context.GetPropertyMappings().Count());
        }


        [TestMethod]
        public void MapWithFuncWillNormalizeInputProperty()
        {
            PropertyInfo maleOrFemale = typeof(Person).GetProperty("MaleOrFemale");

            var context = MapBuilder<Customer>.MapAllProperties()
                .Map(maleOrFemale).WithFunc(x => 'm') as IMapBuilderContextTest<Customer>;

            Assert.AreEqual(2, context.GetPropertyMappings().Count());
        }


        [TestMethod]
        public void ThenCanOverwritePropertyInfoFromBaseClass()
        {
            var context = MapBuilder<Customer>.MapAllProperties()
                .DoNotMap(x => x.MaleOrFemale) as IMapBuilderContextTest<Customer>;

            Assert.AreEqual(1, context.GetPropertyMappings().Count());
        }


        [TestMethod]
        public void ThenNoMappingsAreMadeForIndexedProperty()
        {
            var context =
                MapBuilder<ContainsProperrtyWithIndexer>.MapAllProperties() as IMapBuilderContextTest<ContainsProperrtyWithIndexer>;

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

    [TestClass]
    public class WhenMapperIsBuiltForTypeWithCollection : ArrangeActAssert
    {
        private IMapBuilderContextTest<Order> context;

        public class Order
        {
            public int OrderId { get; set; }
            public List<int> OrderLines { get; set; }
        }

        [TestMethod]
        public void ThenBuildAllPropertiesIgnoresCollectionProperties()
        {
            context = (IMapBuilderContextTest<Order>)MapBuilder<Order>.MapAllProperties();
            Assert.AreEqual(1, context.GetPropertyMappings().Count());
            Assert.AreEqual("OrderId", context.GetPropertyMappings().First().Property.Name);
        }

    }

}
