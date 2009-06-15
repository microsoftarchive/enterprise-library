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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public abstract class ReflectionRowMapperContext<T> : ArrangeActAssert
        where T : new() 
    {
        protected MapBuilder<T> mapBuilder;

        protected override void Arrange()
        {
            mapBuilder = new MapBuilder<T>();
        }
    }

    public abstract class ReflectionRowMapperWithReaderContext<T> : ReflectionRowMapperContext<T>
        where T : new()
    {
        protected ReflectionRowMapper<T> defaultMapper;
        protected IEnumerable<PropertyMapping> propertyMappings;
        protected string CommandText = "SELECT 'ALKI' as CustomerID, 'Violet street 23' as ShipAddress, 12.6 as freight, null SqlNull";
        protected string ConnectionString = @"server=(local)\SQLEXPRESS;database=Northwind;Integrated Security=true";
        protected IDataReader reader;

        protected override void Arrange()
        {
            base.Arrange();

            defaultMapper = (ReflectionRowMapper<T>)BuildMapper();
            propertyMappings = defaultMapper.GetPropertyMappings();

            SqlDatabase database = new SqlDatabase(ConnectionString);
            reader = database.ExecuteReader(CommandType.Text, CommandText);
            reader.Read();
        }

        protected virtual IRowMapper<T> BuildMapper()
        {
            return new MapBuilder<T>().CreateDefault().BuildMapper();
        }
    }

    [TestClass]
    public class WhenMapBuilderCreatesDefaultReflectionMapper : ReflectionRowMapperContext<Order>
    {
        ReflectionRowMapper<Order> defaultMapper;
        IEnumerable<PropertyMapping> propertyMappings;

        protected override void Arrange()
        {
            base.Arrange();

            defaultMapper = (ReflectionRowMapper<Order>)mapBuilder.CreateDefault().BuildMapper();
            propertyMappings = defaultMapper.GetPropertyMappings();
        }

        [TestMethod]
        public void ThenMapperDoesntContainsMappingsForStaticProperties()
        {
            Assert.IsFalse(propertyMappings.Any(x => x.Property.Name == "StaticProperty"));
        }

        [TestMethod]
        public void ThenMapperDoesntContainsMappingsForNonPublicProperties()
        {
            Assert.IsFalse(propertyMappings.Any(x => x.Property.Name == "ProtectedProperty"));
        }

        [TestMethod]
        public void ThenMapperContainsMappingsForAllPublicProperties()
        {
            Assert.IsTrue(propertyMappings.Any(x => x.Property.Name == "CustomerID"));
            Assert.IsTrue(propertyMappings.Any(x => x.Property.Name == "NotInSource"));
            Assert.IsTrue(propertyMappings.Any(x => x.Property.Name == "OrderDate"));
            Assert.IsTrue(propertyMappings.Any(x => x.Property.Name == "ShippedDate"));
            Assert.IsTrue(propertyMappings.Any(x => x.Property.Name == "ShipAddress"));
            Assert.IsTrue(propertyMappings.Any(x => x.Property.Name == "ShipRegion"));
            Assert.IsTrue(propertyMappings.Any(x => x.Property.Name == "ShipRegion"));
            Assert.IsTrue(propertyMappings.Any(x => x.Property.Name == "Freight"));

        }
    }

    [TestClass]
    public class WhenReflectionMapperMapsRowToType : ReflectionRowMapperWithReaderContext<Order>
    {
        protected override IRowMapper<Order> BuildMapper()
        {
            return new MapBuilder<Order>().MapByName(x => x.CustomerID)
                                          .Map(x => x.Freight).ToColumn("SqlNull")
                                          .Map(x => x.ShipAddress).ToColumn("SqlNull")
                                          .DoNotMap(x => x.NotInSource).BuildMapper();
        }

        [TestMethod]
        public void ThenMatchingColumnsAreMapped()
        {
            var order = defaultMapper.MapRow(reader);
            Assert.AreEqual("ALKI", order.CustomerID);
        }

        [TestMethod]
        public void ThenIgnoredPropertiesAreNotSet()
        {
            var order = defaultMapper.MapRow(reader);
            Assert.AreEqual(0, order.NotInSource);
        }

        [TestMethod]
        public void ThenDBNullValuesAreConvertedToEmptyString()
        {
            var order = defaultMapper.MapRow(reader);
            Assert.IsNull(order.ShipAddress);
        }

        [TestMethod]
        public void ThenDBNullValuesAreConvertedToDefaultValueType()
        {
            var order = defaultMapper.MapRow(reader);
            Assert.AreEqual(0d, order.Freight);
        }


        protected override void Teardown()
        {
            reader.Close();
        }
    }

    [TestClass]
    public class WhenReflectionMapperMapsPropertyToNonExistingColumn : ReflectionRowMapperWithReaderContext<Order>
    {
        protected override IRowMapper<Order> BuildMapper()
        {
            return new MapBuilder<Order>().Map(x => x.CustomerID).ToColumn("NonExistingColumn").BuildMapper();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void ThenMapRowThrows()
        {
            Order typeTest = defaultMapper.MapRow(reader);
        }

    }

    [TestClass]
    public class WhenReflectionMapperMapsDBNullToValueType : ReflectionRowMapperWithReaderContext<ValueTypeTest>
    {
        protected override void Arrange()
        {
            CommandText = @"SELECT null as [Int], 
                                   null as [Double],
                                   null as [Guid],
                                   null as [DateTime],
                                   null as [Long],
                                   null as [Short]";
            base.Arrange();
        }

        protected override IRowMapper<ValueTypeTest> BuildMapper()
        {
            return new MapBuilder<ValueTypeTest>().CreateDefault()
                                   .DoNotMap(x => x.NullableDateTime)
                                   .DoNotMap(x => x.NullableDouble)
                                   .DoNotMap(x => x.NullableGuid)
                                   .DoNotMap(x => x.NullableInt)
                                   .DoNotMap(x => x.NullableLong)
                                   .DoNotMap(x => x.NullableShort)
                                   .BuildMapper();
        }

        [TestMethod]
        public void ThenReflectionMapperAssignsDefaultValue()
        {
            Assert.AreEqual(6, defaultMapper.GetPropertyMappings().OfType<ColumnNameMapping>().Count());
            
            ValueTypeTest typeTest = defaultMapper.MapRow(reader);

            Assert.AreEqual(default(int), typeTest.Int);
            Assert.AreEqual(default(double), typeTest.Double);
            Assert.AreEqual(default(Guid), typeTest.Guid);
            Assert.AreEqual(default(DateTime), typeTest.DateTime);
            Assert.AreEqual(default(long), typeTest.Long);
            Assert.AreEqual(default(short), typeTest.Short);
            
        }
    }

    [TestClass]
    public class WhenReflectionMapperMapsDBNullableValueTypes : ReflectionRowMapperWithReaderContext<ValueTypeTest>
    {
        protected override void Arrange()
        {
            CommandText = @"SELECT null as [NullableInt], 
                                   null as [NullableDouble],
                                   null as [NullableGuid],
                                   null as [NullableDateTime],
                                   null as [NullableLong],
                                   null as [NullableShort]";
            base.Arrange();
        }


        protected override IRowMapper<ValueTypeTest> BuildMapper()
        {
            return new MapBuilder<ValueTypeTest>().CreateDefault()
                                   .DoNotMap(x => x.DateTime)
                                   .DoNotMap(x => x.Double)
                                   .DoNotMap(x => x.Guid)
                                   .DoNotMap(x => x.Int)
                                   .DoNotMap(x => x.Long)
                                   .DoNotMap(x => x.Short)
                                   .BuildMapper() ;
        }

        [TestMethod]
        public void ThenReflectionMapperAssignsDefaultValue()
        {
            Assert.AreEqual(6, defaultMapper.GetPropertyMappings().OfType<ColumnNameMapping>().Count());

            ValueTypeTest typeTest = defaultMapper.MapRow(reader);

            Assert.IsNull(typeTest.NullableInt);
            Assert.IsNull(typeTest.NullableDouble);
            Assert.IsNull(typeTest.NullableGuid);
            Assert.IsNull(typeTest.NullableDateTime);
            Assert.IsNull(typeTest.NullableLong);
            Assert.IsNull(typeTest.NullableShort);

        }
    }

    [TestClass]
    public class WhenReflectionMapperMapsValueToGuid : ReflectionRowMapperWithReaderContext<ValueTypeTest>
    {
        protected override void Arrange()
        {
            CommandText = "SELECT newid() as [GUID]";
            base.Arrange();
        }

        protected override IRowMapper<ValueTypeTest> BuildMapper()
        {
            return new MapBuilder<ValueTypeTest>().MapByName(x=>x.Guid).BuildMapper();
        }

        [TestMethod]
        public void ThenGuidIsSetOnTargetType()
        {
            ValueTypeTest typeTest = defaultMapper.MapRow(reader);
            Assert.AreNotEqual(default(Guid), typeTest.Guid);
        }
    }
    
    [TestClass]
    public class WhenReflectionMapperMapsPropertyToColumnWithWrongCasing : ReflectionRowMapperWithReaderContext<ValueTypeTest>
    {
        protected override void Arrange()
        {
            CommandText = "SELECT newid() as [GUID]";
            base.Arrange();
        }

        protected override IRowMapper<ValueTypeTest> BuildMapper()
        {
            return new MapBuilder<ValueTypeTest>()
                .Map(x => x.Guid).ToColumn("gUiD")
                .BuildMapper();
        }

        [TestMethod]
        public void ThenMapperMatchesPropertyToColumn()
        {
            ValueTypeTest typeTest = defaultMapper.MapRow(reader);
            Assert.AreNotEqual(default(Guid), typeTest.Guid);
        }
    }

    public class Order
    {
        protected string ProtectedProperty {get;set;}
        public static string StaticProperty { get; set; }

        public int NotInSource { get; set; }

        public string CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippedDate { get; set; }

        public string ShipAddress { get; set; }
        public string ShipRegion { get; set; }
        public string ShipCounty { get; set; }

        public double Freight { get; set; }

        public List<OrderDetail> Details { get; set; }
    }

    public class OrderDetail
    {
        public int ProductID { get; set; }
        public double UnitPrice { get; set; }
        public double Discount { get; set; }
        public int Quantity { get; set; }
    }

    public class ValueTypeTest
    {

        public int? NullableInt { get; set; }
        public double? NullableDouble { get; set; }
        public long? NullableLong { get; set; }
        public short? NullableShort { get; set; }
        public Guid? NullableGuid { get; set; }
        public DateTime? NullableDateTime { get; set; }

        public int Int { get; set; }
        public double Double { get; set; }
        public long Long { get; set; }
        public short Short { get; set; }
        public Guid Guid { get; set; }
        public DateTime DateTime { get; set; }

    }


}
