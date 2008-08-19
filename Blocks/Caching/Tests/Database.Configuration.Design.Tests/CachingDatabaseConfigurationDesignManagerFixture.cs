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
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.Tests
{
    [TestClass]
    public class CachingDatabaseConfigurationDesignManagerFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void OpenAndSaveConfiguration()
        {
            using (new ConfigFileSnapshot())
            {
                ApplicationNode.Hierarchy.Load();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
                ApplicationNode.Hierarchy.Open();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

                CacheManagerSettingsNode rootNode = (CacheManagerSettingsNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(CacheManagerSettingsNode));
                Assert.IsNotNull(rootNode);
                Assert.AreEqual("ShortInMemoryPersistence", rootNode.DefaultCacheManager.Name);

                Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(CacheManagerSettingsNode)).Count);
                Assert.AreEqual(2, Hierarchy.FindNodesByType(ApplicationNode, typeof(CacheManagerNode)).Count);
                Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(CacheStorageNode)).Count);

                Hierarchy.Load();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
                Hierarchy.Open();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
                CacheManagerCollectionNode cacheManagerCollectionNode = (CacheManagerCollectionNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(CacheManagerCollectionNode));

                CacheManagerNode cacheManagerNode = new CacheManagerNode();
                cacheManagerCollectionNode.AddNode(cacheManagerNode);
                AddDataCacheStorageCommand cmd = new AddDataCacheStorageCommand(ServiceProvider);
                cmd.Execute(cacheManagerNode);
                DataCacheStorageNode dataNode = (DataCacheStorageNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(DataCacheStorageNode));
                ConnectionStringSettingsNode connectNode = (ConnectionStringSettingsNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(ConnectionStringSettingsNode));
                dataNode.DatabaseInstance = connectNode;
                dataNode.PartitionName = "foo";

                Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(DatabaseSectionNode)).Count);

                Hierarchy.Save();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

                Hierarchy.Load();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
                Hierarchy.Open();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

                Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(DataCacheStorageNode)).Count);

                dataNode = (DataCacheStorageNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(DataCacheStorageNode));
                Assert.AreEqual(dataNode.DatabaseInstance.Name, connectNode.Name);
                Assert.AreEqual(dataNode.PartitionName, "foo");

                DatabaseSectionNode databaseSectionNode = (DatabaseSectionNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(DatabaseSectionNode));
                databaseSectionNode.Remove();

                dataNode.Remove();
                Hierarchy.Save();
                Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            }
        }

        [TestMethod]
        public void EnsureDatabaseSettingsAreAddedOnNewNode()
        {
            AddCacheManagerSettingsNodeCommand cacheCmd = new AddCacheManagerSettingsNodeCommand(ServiceProvider);
            cacheCmd.Execute(Hierarchy.RootNode);

            CacheManagerCollectionNode cacheManagerCollectionNode = (CacheManagerCollectionNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(CacheManagerCollectionNode));
            CacheManagerNode cacheManagerNode = new CacheManagerNode();
            cacheManagerCollectionNode.AddNode(cacheManagerNode);
            AddDataCacheStorageCommand cmd = new AddDataCacheStorageCommand(ServiceProvider);
            cmd.Execute(cacheManagerNode);

            Assert.IsNotNull(Hierarchy.FindNodeByType(ApplicationNode, typeof(DatabaseSectionNode)));
        }
    }

    class ConfigFileSnapshot : IDisposable
    {
        string contents;

        public ConfigFileSnapshot()
        {
            using (StreamReader rdr = new StreamReader(GetConfigFilePath()))
            {
                contents = rdr.ReadToEnd();
            }
        }

        public void Dispose()
        {
            if (contents != null)
            {
                using (StreamWriter writer = new StreamWriter(GetConfigFilePath(), false))
                {
                    writer.Write(contents);
                }
                contents = null;
            }
        }

        string GetConfigFilePath()
        {
            return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
        }
    }
}