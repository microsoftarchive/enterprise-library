//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class AddConfigurationApplicationNodeCommadFixture : ConfigurationDesignHost
    {
        AddConfigurationApplicationNodeCommand addConfigurationApplicationNodeCommand;
        bool hierarchyAdded;
        ConfigurationNode nodeAdded;

        protected override void InitializeCore()
        {
            addConfigurationApplicationNodeCommand = new AddConfigurationApplicationNodeCommand(ServiceProvider);
            HiearchyService.HierarchyAdded += new EventHandler<HierarchyAddedEventArgs>(OnHierarchyAdded);
        }

        protected override void CleanupCore()
        {
            hierarchyAdded = false;
            addConfigurationApplicationNodeCommand.Dispose();
        }

        [TestMethod]
        public void CreateAConfigurationApplicationNodeCreatesAHierarchyAndAddsNode()
        {
            addConfigurationApplicationNodeCommand.Execute(null);

            Assert.IsTrue(hierarchyAdded);
            Assert.AreEqual(typeof(ConfigurationApplicationNode), nodeAdded.GetType());
        }

        void OnHierarchyAdded(object sender,
                              HierarchyAddedEventArgs args)
        {
            hierarchyAdded = true;
            nodeAdded = args.UIHierarchy.RootNode;
        }
    }
}
