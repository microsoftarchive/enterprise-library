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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{

    [TestClass]
    public class WhenCreatingReflectionRowMapper : ArrangeActAssert
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenPassingNullMappingsThrows()
        {
            new ReflectionRowMapper<int>(null);
        }
    }

    public abstract class ReflectionRowMapperContext<T> : ArrangeActAssert
        where T : new()
    {
        protected IMapBuilderContext<T> mapBuilder;

        protected override void Arrange()
        {
            mapBuilder = MapBuilder<T>.MapNoProperties();
        }
    }

    public abstract class ReflectionRowMapperWithReaderContext<T> : ReflectionRowMapperContext<T>
        where T : new()
    {
        protected ReflectionRowMapper<T> defaultMapper;
        protected IEnumerable<PropertyMapping> propertyMappings;
        protected string CommandText = "SELECT 'ALKI' as CustomerID, 'Violet street 23' as ShipAddress, 12.6 as freight, null SqlNull";
        protected string ConnectionString = @"server=(localdb)\v11.0;database=Northwind;Integrated Security=true";
        protected IDataReader reader;

        protected override void Arrange()
        {
            base.Arrange();

            defaultMapper = (ReflectionRowMapper<T>)BuildMapper();

            SqlDatabase database = new SqlDatabase(ConnectionString);
            reader = database.ExecuteReader(CommandType.Text, CommandText);
            reader.Read();
        }

        protected virtual IRowMapper<T> BuildMapper()
        {
            return MapBuilder<T>.BuildAllProperties();
        }
    }

    [TestClass]
    public class WhenReflectionMapperMapsRowToType : ReflectionRowMapperWithReaderContext<Order>
    {
        protected override IRowMapper<Order> BuildMapper()
        {
            return MapBuilder<Order>.MapAllProperties().MapByName(x => x.CustomerID)
                                          .Map(x => x.Freight).ToColumn("SqlNull")
                                          .Map(x => x.ShipAddress).ToColumn("SqlNull")
                                          .Map(x => x.ShippedDate).WithFunc(dr => new DateTime(10000000))
                                          .DoNotMap(x => x.NotInSource)
                                          .DoNotMap(x => x.OrderDate)
                                          .DoNotMap(x => x.ShipCounty)
                                          .DoNotMap(x => x.ShipRegion)
                                          .Build();
        }

        [TestMethod]
        public void ThenMatchingColumnsAreMapped()
        {
            var order = defaultMapper.MapRow(reader);

            Assert.AreEqual("ALKI", order.CustomerID);
            Assert.AreEqual(new DateTime(10000000), order.ShippedDate);
        }

        [TestMethod]
        public void ThenIgnoredPropertiesAreNotSet()
        {
            var order = defaultMapper.MapRow(reader);

            Assert.AreEqual(Order.DefaultIntValue, order.NotInSource);
            Assert.AreEqual(Order.DefaultDateTimeValue, order.OrderDate);
            Assert.AreEqual(Order.DefaultStringValue, order.ShipCounty);
            Assert.AreEqual(Order.DefaultStringValue, order.ShipRegion);
        }

        [TestMethod]
        public void ThenDBNullValuesAreConvertedToNullString()
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

        [TestMethod]
        public void ThenCollectionPropertiesAreIgnored()
        {
            var order = defaultMapper.MapRow(reader);

            Assert.AreSame(Order.DefaultListValue, order.Details);
        }

        [TestMethod]
        public void ThenNonPublicPropertiesAreIgnored()
        {
            var order = defaultMapper.MapRow(reader);

            Assert.AreSame(Order.DefaultStringValue, order.ProtectedProperty);
        }

        [TestMethod]
        public void ThenStaticPropertiesAreIgnored()
        {
            var order = defaultMapper.MapRow(reader);

            Assert.AreSame(Order.DefaultStringValue, Order.StaticProperty);
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
            return MapBuilder<Order>.MapNoProperties().Map(x => x.CustomerID).ToColumn("NonExistingColumn").Build();
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
            return MapBuilder<ValueTypeTest>.MapAllProperties()
                                   .DoNotMap(x => x.NullableDateTime)
                                   .DoNotMap(x => x.NullableDouble)
                                   .DoNotMap(x => x.NullableGuid)
                                   .DoNotMap(x => x.NullableInt)
                                   .DoNotMap(x => x.NullableLong)
                                   .DoNotMap(x => x.NullableShort)
                                   .Build();
        }

        [TestMethod]
        public void ThenReflectionMapperAssignsDefaultValue()
        {
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
            return MapBuilder<ValueTypeTest>.MapAllProperties()
                                   .DoNotMap(x => x.DateTime)
                                   .DoNotMap(x => x.Double)
                                   .DoNotMap(x => x.Guid)
                                   .DoNotMap(x => x.Int)
                                   .DoNotMap(x => x.Long)
                                   .DoNotMap(x => x.Short)
                                   .Build();
        }

        [TestMethod]
        public void ThenReflectionMapperAssignsDefaultValue()
        {
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
            return MapBuilder<ValueTypeTest>.MapNoProperties().MapByName(x => x.Guid).Build();
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
            return MapBuilder<ValueTypeTest>.MapNoProperties()
                .Map(x => x.Guid).ToColumn("gUiD")
                .Build();
        }

        [TestMethod]
        public void ThenMapperMatchesPropertyToColumn()
        {
            ValueTypeTest typeTest = defaultMapper.MapRow(reader);
            Assert.AreNotEqual(default(Guid), typeTest.Guid);
        }
    }

    [TestClass]
    public class WhenReflectionMapperFailsToConvertType : ReflectionRowMapperWithReaderContext<ValueTypeTest>
    {
        protected override void Arrange()
        {
            CommandText = "SELECT newid() as [GUID]";
            base.Arrange();
        }

        protected override IRowMapper<ValueTypeTest> BuildMapper()
        {
            return MapBuilder<ValueTypeTest>.MapNoProperties()
                .Map(x => x.Int).ToColumn("GUID")
                .Build();
        }

        [TestMethod]
        public void ThenExceptionHasMeaningfullException()
        {
            try
            {
                defaultMapper.MapRow(reader);
                Assert.Fail();
            }
            catch (InvalidCastException ice)
            {
                Assert.IsTrue(ice.Message.Contains("Int"));
                Assert.IsTrue(ice.Message.Contains("GUID"));
            }
        }
    }

    [TestClass]
    public class WhenReflectionMapperFailsToConvertTypeAndThrowsFormatException : ReflectionRowMapperWithReaderContext<ValueTypeTest>
    {
        protected override void Arrange()
        {
            CommandText = "SELECT 'a' as Int";
            base.Arrange();
        }

        protected override IRowMapper<ValueTypeTest> BuildMapper()
        {
            return MapBuilder<ValueTypeTest>.MapNoProperties()
                .Map(x => x.Int).ToColumn("Int")
                .Build();
        }

        [TestMethod]
        public void ThenExceptionHasMeaningfullException()
        {
            try
            {
                defaultMapper.MapRow(reader);
                Assert.Fail();
            }
            catch (InvalidCastException ice)
            {
                Assert.IsTrue(ice.Message.Contains("Int"));
            }
        }

    }

    public class Order
    {
        public const int DefaultIntValue = 1000000;
        public const string DefaultStringValue = "default value";
        public static readonly DateTime DefaultDateTimeValue = DateTime.MaxValue;
        public const double DefaultDoubleValue = double.PositiveInfinity;
        public static readonly List<OrderDetail> DefaultListValue = new List<OrderDetail>();

        public Order()
        {
            ProtectedProperty = DefaultStringValue;
            StaticProperty = DefaultStringValue;

            NotInSource = DefaultIntValue;

            CustomerID = DefaultStringValue;
            OrderDate = DefaultDateTimeValue;
            ShippedDate = DefaultDateTimeValue;

            ShipAddress = DefaultStringValue;
            ShipRegion = DefaultStringValue;
            ShipCounty = DefaultStringValue;

            Freight = DefaultDoubleValue;

            Details = DefaultListValue;
        }

        protected internal string ProtectedProperty { get; set; }
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
