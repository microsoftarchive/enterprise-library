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
    public class ConfigurationUIHiearchyServiceFixture
    {
        int addEventCount;
        int removedEventCount;
        IConfigurationUIHierarchy eventHierarchy;

        [TestCleanup]
        public void TestCleanup()
        {
            addEventCount = 0;
            removedEventCount = 0;
            eventHierarchy = null;
        }

        [TestMethod]
        public void EnsureAddHierarchyEventFired()
        {
            using (IConfigurationUIHierarchyService hierarchyService = new ConfigurationUIHierarchyService())
            {
                hierarchyService.HierarchyAdded += new EventHandler<HierarchyAddedEventArgs>(OnHierarchyAdded);
                IConfigurationUIHierarchy hierarchy = CreateHierarchy();
                hierarchyService.AddHierarchy(hierarchy);
                hierarchyService.HierarchyAdded -= new EventHandler<HierarchyAddedEventArgs>(OnHierarchyAdded);

                Assert.AreEqual(1, addEventCount);
                Assert.AreSame(hierarchy, eventHierarchy);
            }
        }

        [TestMethod]
        public void EnsureAllHierarchiesCanBeRetrieved()
        {
            using (IConfigurationUIHierarchyService hierarchyService = new ConfigurationUIHierarchyService())
            {
                IConfigurationUIHierarchy hierarchy = CreateHierarchy();
                IConfigurationUIHierarchy hierarchy2 = CreateHierarchy();
                hierarchyService.AddHierarchy(hierarchy);
                hierarchyService.AddHierarchy(hierarchy2);
                IConfigurationUIHierarchy[] hierarchies = hierarchyService.GetAllHierarchies();

                Assert.AreEqual(2, hierarchies.Length);
                Assert.AreSame(hierarchy2, hierarchies[1]);
            }
        }

        [TestMethod]
        public void CanFindHierarchyById()
        {
            using (IConfigurationUIHierarchyService hierarchyService = new ConfigurationUIHierarchyService())
            {
                ConfigurationApplicationNode appNode = new ConfigurationApplicationNode(ConfigurationApplicationFile.FromCurrentAppDomain());
                IConfigurationUIHierarchy hierarchy = new ConfigurationUIHierarchy(appNode, ServiceBuilder.Build());
                hierarchyService.AddHierarchy(hierarchy);
                IConfigurationUIHierarchy foundHierarchy = hierarchyService.GetHierarchy(appNode.Id);
                Assert.AreSame(hierarchy, foundHierarchy);
            }
        }

        [TestMethod]
        public void EnsureRemoveHierarchyAndRemoveEventFired()
        {
            using (IConfigurationUIHierarchyService hierarchyService = new ConfigurationUIHierarchyService())
            {
                hierarchyService.HierarchyRemoved += new EventHandler<HierarchyRemovedEventArgs>(OnHierarchyRemoved);
                ConfigurationApplicationNode appNode = new ConfigurationApplicationNode(ConfigurationApplicationFile.FromCurrentAppDomain());
                IConfigurationUIHierarchy hierarchy = new ConfigurationUIHierarchy(appNode, ServiceBuilder.Build());
                hierarchyService.AddHierarchy(hierarchy);
                hierarchyService.RemoveHierarchy(appNode.Id);
                IConfigurationUIHierarchy foundHierarchy = hierarchyService.GetHierarchy(appNode.Id);
                hierarchyService.HierarchyRemoved -= new EventHandler<HierarchyRemovedEventArgs>(OnHierarchyRemoved);

                Assert.AreEqual(1, removedEventCount);
                Assert.AreSame(hierarchy, eventHierarchy);
                Assert.IsNull(foundHierarchy);
            }
        }

        static IConfigurationUIHierarchy CreateHierarchy()
        {
            ConfigurationApplicationNode appNode = new ConfigurationApplicationNode(ConfigurationApplicationFile.FromCurrentAppDomain());
            return new ConfigurationUIHierarchy(appNode, ServiceBuilder.Build());
        }

        void OnHierarchyAdded(object sender,
                              HierarchyAddedEventArgs args)
        {
            addEventCount++;
            eventHierarchy = args.UIHierarchy;
        }

        void OnHierarchyRemoved(object sender,
                                HierarchyRemovedEventArgs args)
        {
            removedEventCount++;
            eventHierarchy = args.UIHierarchy;
        }
    }
}
