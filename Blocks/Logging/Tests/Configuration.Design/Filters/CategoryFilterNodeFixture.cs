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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class CategoryFilterNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInCategoryFilterNodeThrows()
        {
            new CategoryFilterNode(null);
        }

        [TestMethod]
        public void CategoryFilterNodeDefaults()
        {
            CategoryFilterNode categoryFilter = new CategoryFilterNode();

            Assert.AreEqual("Category Filter", categoryFilter.Name);
            Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, categoryFilter.CategoryFilterExpression.CategoryFilterMode);
            Assert.AreEqual(0, categoryFilter.CategoryFilterExpression.CategoryFilters.Count);
        }

        [TestMethod]
        public void CategoryFilterDataTest()
        {
            string name = "some name";
            CategoryFilterMode filterMode = CategoryFilterMode.AllowAllExceptDenied;
            string filterName = "some filter";

            CategoryFilterData data = new CategoryFilterData();
            data.Name = name;
            data.CategoryFilterMode = filterMode;
            data.CategoryFilters.Add(new CategoryFilterEntry(filterName));

            CategoryFilterNode node = new CategoryFilterNode(data);

            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(filterMode, node.CategoryFilterExpression.CategoryFilterMode);
            Assert.AreEqual(1, node.CategoryFilterExpression.CategoryFilters.Count);
            Assert.AreEqual(filterName, node.CategoryFilterExpression.CategoryFilters.Get(0).Name);
        }

        [TestMethod]
        public void CategoryFilterNodeTest()
        {
            string name = "some name";
            CategoryFilterMode filterMode = CategoryFilterMode.AllowAllExceptDenied;
            string filterName = "some filter";

            CategoryFilterSettings filterSettings = new CategoryFilterSettings(filterMode, new NamedElementCollection<CategoryFilterEntry>());
            filterSettings.CategoryFilters.Add(new CategoryFilterEntry(filterName));

            CategoryFilterNode node = new CategoryFilterNode();
            node.Name = name;
            node.CategoryFilterExpression = filterSettings;

            CategoryFilterData nodeData = (CategoryFilterData)node.LogFilterData;

            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(filterMode, nodeData.CategoryFilterMode);
            Assert.AreEqual(1, nodeData.CategoryFilters.Count);
            Assert.AreEqual(filterName, nodeData.CategoryFilters.Get(0).Name);
        }
    }
}