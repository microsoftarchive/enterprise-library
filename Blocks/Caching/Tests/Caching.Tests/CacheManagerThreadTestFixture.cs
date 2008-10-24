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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestClass]
    public class CacheManagerThreadTestFixture
    {
        public static int callbackCount;
        public static CacheItemRemovedReason callbackReason;
        public static string exceptionMessage;
        public static object sharedLock;

        static CacheManager cacheManager;

        [TestInitialize]
        public void StartCacheProcesses()
        {
            sharedLock = new object();
            callbackCount = 0;
            callbackReason = CacheItemRemovedReason.Unknown;
            exceptionMessage = "";

            cacheManager = MockCacheManagerFactory.Create("ShortInMemoryPersistence", TestConfigurationSource.GenerateConfiguration());
        }

        [TestCleanup]
        public void StopCacheProcesses()
        {
            cacheManager.Dispose();
        }

        [TestMethod]
        public void CanCreateSystem()
        {
            Thread.Sleep(500);
            lock (sharedLock)
            {
                Monitor.Pulse(sharedLock);
            }
            Thread.Sleep(100);

            string noExceptionExpectedMessage = "";
            Assert.AreEqual(noExceptionExpectedMessage, exceptionMessage);
        }

        [TestMethod]
        public void CanAddItemBackIntoCacheAsItIsBeingExpired()
        {
            cacheManager.Add("key1", "value1", CacheItemPriority.Normal, null, new AlwaysExpired());
            Thread.Sleep(1500);

            lock (sharedLock)
            {
                cacheManager.Add("key1", "value2", CacheItemPriority.Normal, null, new NeverExpired());
                Monitor.Pulse(sharedLock);
            }

            Thread.Sleep(100);

            Assert.AreEqual("value2", cacheManager.GetData("key1"));
        }

        [TestMethod]
        public void ItemRemovedFromCacheDuringExpirationOnlyCausesRemovedCallBack()
        {
            cacheManager.Add("key1", "value1", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired());
            Thread.Sleep(1500);

            lock (sharedLock)
            {
                cacheManager.Remove("key1");
                Monitor.Pulse(sharedLock);
            }

            Thread.Sleep(10);

            Assert.AreEqual(1, callbackCount);
            Assert.AreEqual(CacheItemRemovedReason.Removed, callbackReason);
        }

        [TestMethod]
        public void GetDataForExpiredItemRemovesItemFromCacheAndPreventsExpirationOfItemAsWell()
        {
            cacheManager.Add("key1", "value1", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired());
            cacheManager.GetData("key1");

            Thread.Sleep(1500);

            Assert.AreEqual(1, callbackCount);
            Assert.AreEqual(CacheItemRemovedReason.Expired, callbackReason);

            lock (sharedLock)
            {
                Monitor.Pulse(sharedLock);
            }

            Thread.Sleep(500);

            Assert.AreEqual(1, callbackCount);
            Assert.AreEqual(CacheItemRemovedReason.Expired, callbackReason);
        }

        [TestMethod]
        public void FlushDoesNotCauseCallbacksInCache()
        {
            cacheManager.Add("key1", "value1", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired());
            Thread.Sleep(1500);

            lock (sharedLock)
            {
                cacheManager.Flush();
                Monitor.Pulse(sharedLock);
            }

            Thread.Sleep(100);

            Assert.AreEqual(0, callbackCount, "Should never be called back if flush called during expiration");
        }

        [TestMethod]
        public void GetDataShouldCauseItemToExpireImmediatelyAndCauseExpirationCallbackToHappen()
        {
            cacheManager.Add("key1", "value1", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired());
            Thread.Sleep(1500);

            lock (sharedLock)
            {
                cacheManager.GetData("key1");
                Monitor.Pulse(sharedLock);
            }

            Thread.Sleep(100);

            Assert.AreEqual(1, callbackCount);
            Assert.AreEqual(CacheItemRemovedReason.Expired, callbackReason);
            Assert.IsNull(cacheManager.GetData("key1"), "GetData should have expired the item immediately");
        }

        [TestMethod]
        public void TwoThreadsExecutingOneInAddAndOneInFlushShouldNotCauseDeadlock()
        {
            ThreadStart addingThreadProc = new ThreadStart(AddToCache);
            ThreadStart flushingThreadProc = new ThreadStart(FlushCache);

            Thread addingThread = new Thread(addingThreadProc);
            Thread flushingThread = new Thread(flushingThreadProc);

            addingThread.Start();
            flushingThread.Start();

            addingThread.Join();
            flushingThread.Join();

            Assert.IsTrue(true, "We finished and didn't deadlock");
        }

        void FlushCache()
        {
            cacheManager.Flush();
        }

        void AddToCache()
        {
            cacheManager.Add("key1", "value1");
        }

        class RaceConditionSimulatingExpirationTask : ExpirationTask
        {
            public RaceConditionSimulatingExpirationTask(ICacheOperations cacheOperations,
                                                         CachingInstrumentationProvider instrumentationProvider)
                : base(cacheOperations, instrumentationProvider) {}

            public override void PrepareForSweep()
            {
                lock (sharedLock)
                {
                    try
                    {
                        Monitor.Wait(sharedLock);
                    }
                    catch (Exception e)
                    {
                        exceptionMessage = e.Message;
                    }
                }
            }
        }

        static class MockCacheManagerFactory
        {
            public static CacheManager Create(string name,
                                              IConfigurationSource configurationSource)
            {
                CachingConfigurationView configurationView = new CachingConfigurationView(configurationSource);
                CacheManagerData objectConfiguration = (CacheManagerData)configurationView.GetCacheManagerData(name);

                IBackingStore backingStore =
                    EnterpriseLibraryFactory.BuildUp<IBackingStore>(objectConfiguration.CacheStorage, configurationSource);

                return new MockCacheManagerFactoryHelper().BuildCacheManager(
                    name,
                    backingStore,
                    objectConfiguration.MaximumElementsInCacheBeforeScavenging,
                    objectConfiguration.NumberToRemoveWhenScavenging,
                    objectConfiguration.ExpirationPollFrequencyInSeconds,
                    new CachingInstrumentationProvider());
            }
        }

        class MockCacheManagerFactoryHelper : CacheManagerFactoryHelper
        {
            public override ExpirationTask CreateExpirationTask(ICacheOperations cacheOperations,
                                                                            CachingInstrumentationProvider instrumentationProvider)
            {
                return new RaceConditionSimulatingExpirationTask(cacheOperations, instrumentationProvider);
            }
        }

        class MockRefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string removedKey,
                                object expiredValue,
                                CacheItemRemovedReason removalReason)
            {
                callbackCount++;
                callbackReason = removalReason;
            }
        }
    }
}
