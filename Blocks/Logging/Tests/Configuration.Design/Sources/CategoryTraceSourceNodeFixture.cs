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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests.Sources
{
    [TestClass]
    public class CategoryTraceSourceNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInCategoryTraceSourceNodeThrows()
        {
            new CategoryTraceSourceNode(null);
        }

        [TestMethod]
        public void CategoryTraceSourceDefaults()
        {
            CategoryTraceSourceNode node = new CategoryTraceSourceNode();
            Assert.AreEqual("Category", node.Name);
            Assert.AreEqual(SourceLevels.All, node.SourceLevels);
        }

        [TestMethod]
        public void CategoryTraceSourceNamePropertyIsReadOnly()
        {
            Assert.AreEqual(false, CommonUtil.IsPropertyReadOnly(typeof(CategoryTraceSourceNode), "Name"));
        }

        [TestMethod]
        public void CategoryTraceSourceDefaultDataTest()
        {
            string name = "some name";

            NotProcessedTraceSourceNode categoryTraceSourcesNode = new NotProcessedTraceSourceNode(new TraceSourceData());
            categoryTraceSourcesNode.Name = name;
            ApplicationNode.AddNode(categoryTraceSourcesNode);

            Assert.AreEqual(name, categoryTraceSourcesNode.Name);
            Assert.AreEqual(0, categoryTraceSourcesNode.Nodes.Count);
        }

        [TestMethod]
        public void CategoryTraceSourceCommandTextEqualsCategory()
        {
            bool categoryTraceSourceCommandFound = false;

            IUICommandService commandService = ServiceHelper.GetUICommandService(ServiceProvider);
            commandService.ForEach(typeof(CategoryTraceSourceCollectionNode), delegate(ConfigurationUICommand command)
                                                                              {
                                                                                  if (command.Text == "Category")
                                                                                  {
                                                                                      categoryTraceSourceCommandFound = true;
                                                                                  }
                                                                              });

            Assert.IsTrue(categoryTraceSourceCommandFound);
        }
    }
}
