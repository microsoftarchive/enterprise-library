//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
    [TestClass]
    public class CachingConfigurationDesignManagerFixture : ConfigurationDesignHost
    {
        protected override void InitializeCore()
        {
            Type nodeType = typeof(CacheStorageEncryptionNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(ServiceProvider, nodeType), nodeType, typeof(MockStorageEncryptionProviderData), "Mock storage encryption");

            ServiceHelper.GetNodeCreationService(ServiceProvider).AddNodeCreationEntry(entry);
        }

        [TestMethod]
        public void RegisterTest()
        {
            CachingConfigurationDesignManager manager = new CachingConfigurationDesignManager();
            manager.Register(ServiceProvider);
        }

        [TestMethod]
        public void OpenAndSaveConfiguration()
        {
            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            CacheManagerSettingsNode rootNode = (CacheManagerSettingsNode)ApplicationNode.Hierarchy.FindNodeByType(typeof(CacheManagerSettingsNode));
            Assert.IsNotNull(rootNode);
            Assert.AreEqual("ShortInMemoryPersistence", rootNode.DefaultCacheManager.Name);

            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(CacheManagerSettingsNode)).Count);
            Assert.AreEqual(2, ApplicationNode.Hierarchy.FindNodesByType(typeof(CacheManagerNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(CacheStorageNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(MockCacheManagerDataNode)).Count);
            //Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(CacheStorageEncryptionNode)).Count);

            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            ApplicationNode.Hierarchy.Save();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(CacheManagerSettingsNode)).Count);
            Assert.AreEqual(2, ApplicationNode.Hierarchy.FindNodesByType(typeof(CacheManagerNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(CacheStorageNode)).Count);
        }

        [TestMethod]
        public void BuildContextTest()
        {
            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            IConfigurationSource source = ApplicationNode.Hierarchy.BuildConfigurationSource();
            Assert.IsNotNull(source.GetSection(CacheManagerSettings.SectionName));
        }
    }
}
