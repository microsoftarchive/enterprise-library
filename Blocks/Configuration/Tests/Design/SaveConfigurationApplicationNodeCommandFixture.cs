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
    public class SaveConfigurationApplicationNodeCommandFixture
    {
        bool saveCalled;
        IServiceProvider serviceProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            serviceProvider = ServiceBuilder.Build();
        }

        [TestMethod]
        public void SaveCommandSavesTheHierarchy()
        {
            ConfigurationApplicationNode node = new ConfigurationApplicationNode(ConfigurationApplicationFile.FromCurrentAppDomain());
            IConfigurationUIHierarchy hierarchy = new ConfigurationUIHierarchy(node, serviceProvider);
            IConfigurationUIHierarchyService service = (IConfigurationUIHierarchyService)serviceProvider.GetService(typeof(IConfigurationUIHierarchyService));
            service.AddHierarchy(hierarchy);
            hierarchy.Saved += new EventHandler<HierarchySavedEventArgs>(OnHierarchySaved);
            SaveConfigurationApplicationNodeCommand cmd = new SaveConfigurationApplicationNodeCommand(serviceProvider);
            cmd.Execute(node);

            Assert.IsTrue(cmd.SaveSucceeded);
            Assert.IsTrue(saveCalled);
        }

        void OnHierarchySaved(object sender,
                              HierarchySavedEventArgs e)
        {
            saveCalled = true;
        }
    }
}
