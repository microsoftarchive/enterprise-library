//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenPriorityFilterData
    {
        private LogFilterData filterData;

        [TestInitialize]
        public void Setup()
        {
            filterData = new PriorityFilterData("filter", 100) { MaximumPriority = 200 };
        }

        [TestMethod]
        public void WhenBuildingFilter_ThenCreatesPriorityFilter()
        {
            var filter = (PriorityFilter)filterData.BuildFilter();

            Assert.AreEqual("filter", filter.Name);
            Assert.AreEqual(100, filter.MinimumPriority);
            Assert.AreEqual(200, filter.MaximumPriority);
        }
    }

    [TestClass]
    public class GivenCategoryFilterData
    {
        private LogFilterData filterData;

        [TestInitialize]
        public void Setup()
        {
            filterData =
                new CategoryFilterData(
                    "filter",
                    new NamedElementCollection<CategoryFilterEntry> { 
                        new CategoryFilterEntry("category 1"), 
                        new CategoryFilterEntry("category 2") },
                    CategoryFilterMode.DenyAllExceptAllowed);
        }

        [TestMethod]
        public void WhenBuildingFilter_ThenCreatesCategoryFilter()
        {
            var filter = (CategoryFilter)filterData.BuildFilter();

            Assert.AreEqual("filter", filter.Name);
            Assert.AreEqual(2, filter.CategoryFilters.Count);
            Assert.IsTrue(filter.CategoryFilters.Contains("category 1"));
            Assert.IsTrue(filter.CategoryFilters.Contains("category 2"));
            Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, filter.CategoryFilterMode);
        }
    }

    [TestClass]
    public class GivenLogEnabledFilterData
    {
        private LogFilterData filterData;

        [TestInitialize]
        public void Setup()
        {
            filterData = new LogEnabledFilterData("filter", true);
        }

        [TestMethod]
        public void WhenBuildingFilter_ThenCreatesEnabledFilter()
        {
            var filter = (LogEnabledFilter)filterData.BuildFilter();

            Assert.AreEqual("filter", filter.Name);
            Assert.IsTrue(filter.Enabled);
        }
    }

    [TestClass]
    public class GivenCustomFilterDataForLogFilterType
    {
        private LogFilterData filterData;

        [TestInitialize]
        public void Setup()
        {
            filterData =
                new CustomLogFilterData("filter", typeof(MockCustomLogFilter))
                {
                    Attributes = { { MockCustomLogFilter.AttributeKey, "bar" } }
                };
        }

        [TestMethod]
        public void WhenBuildingFilter_ThenCreatesCustomFilter()
        {
            var filter = (MockCustomLogFilter)filterData.BuildFilter();

            Assert.AreEqual("bar", filter.customValue);
        }
    }

    [TestClass]
    public class GivenCustomFilterDataWithEmptyTypeName
    {
        private LogFilterData filterData;

        [TestInitialize]
        public void Setup()
        {
            filterData =
                new CustomLogFilterData()
                {
                    Name = "filter",
                    Attributes = { { MockCustomLogFilter.AttributeKey, "bar" } }
                };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenBuildingFilter_ThenThrows()
        {
            this.filterData.BuildFilter();
        }
    }

    [TestClass]
    public class GivenCustomFilterDataWithInvalidTypeName
    {
        private LogFilterData filterData;

        [TestInitialize]
        public void Setup()
        {
            filterData =
                new CustomLogFilterData()
                {
                    TypeName = "some invalid name",
                    Name = "filter",
                    Attributes = { { MockCustomLogFilter.AttributeKey, "bar" } }
                };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenBuildingFilter_ThenThrows()
        {
            this.filterData.BuildFilter();
        }
    }

    [TestClass]
    public class GivenCustomFilterDataWithNonFilterTypeName
    {
        private LogFilterData filterData;

        [TestInitialize]
        public void Setup()
        {
            filterData =
                new CustomLogFilterData()
                {
                    Type = typeof(string),
                    Name = "filter",
                    Attributes = { { MockCustomLogFilter.AttributeKey, "bar" } }
                };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenBuildingFilter_ThenThrows()
        {
            this.filterData.BuildFilter();
        }
    }

    [TestClass]
    public class GivenCustomFilterDataWithFilterTypeWithoutExpectedConstructor
    {
        private LogFilterData filterData;

        [TestInitialize]
        public void Setup()
        {
            filterData =
                new CustomLogFilterData()
                {
                    Type = typeof(MockCustomLogFilterWithoutConstructor),
                    Name = "filter",
                    Attributes = { { MockCustomLogFilter.AttributeKey, "bar" } }
                };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenBuildingFilter_ThenThrows()
        {
            this.filterData.BuildFilter();
        }

        public class MockCustomLogFilterWithoutConstructor : ILogFilter
        {
            public bool Filter(LogEntry log)
            {
                throw new NotImplementedException();
            }

            public string Name
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}
