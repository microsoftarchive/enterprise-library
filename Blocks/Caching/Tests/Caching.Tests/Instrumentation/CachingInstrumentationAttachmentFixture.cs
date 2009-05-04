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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation.Tests
{
    [TestClass]
    public class CachingInstrumentationAttachmentFixture
    {
        MockCachingInstrumentationListener instrumentationListener;
        CachingInstrumentationProvider instrumentationProvider;
        MockCacheManagerFactoryHelper factoryHelper;
        IBackingStore backingStore;

        const string instanceName = "cache";
        const string key1 = "key1";
        const string key2 = "key2";
        const string key3 = "key3";

        [TestInitialize]
        public void SetUp()
        {
            instrumentationProvider = new CachingInstrumentationProvider();
            instrumentationListener = new MockCachingInstrumentationListener();

            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(instrumentationProvider, instrumentationListener);

            backingStore = new NullBackingStore();
            factoryHelper = new MockCacheManagerFactoryHelper();
        }

        [TestMethod]
        public void CacheInitializationDoesFireEvents()
        {
            using (CacheManager cacheMgr
                = factoryHelper.BuildCacheManager(instanceName, backingStore, 10, 1, 1, instrumentationProvider)) { }

            Assert.AreEqual(1, instrumentationListener.eventArgs.Count);
            Assert.AreSame(typeof(CacheUpdatedEventArgs), instrumentationListener.eventArgs[0].GetType());
        }

        [TestMethod]
        public void CacheAdditionsFireEvents()
        {
            factoryHelper.doExpirations = false;

            using (CacheManager cacheMgr
                = factoryHelper.BuildCacheManager(instanceName, backingStore, 100, 1, 1, instrumentationProvider))
            {
                for (int i = 0; i < 50; i++)
                {
                    cacheMgr.Add(i.ToString(), new object(), CacheItemPriority.NotRemovable, new NullCallback());
                }
            }

            Assert.AreEqual(51, instrumentationListener.eventArgs.Count);
            for (int i = 0; i <= 50; i++)
            {
                Assert.AreSame(typeof(CacheUpdatedEventArgs), instrumentationListener.eventArgs[0].GetType());
            }
        }

        [TestMethod]
        public void CacheMissDoesFireEvents()
        {
            factoryHelper.doExpirations = false;

            using (CacheManager cacheMgr
                = factoryHelper.BuildCacheManager(instanceName, backingStore, 100, 1, 1, instrumentationProvider))
            {
                instrumentationListener.eventArgs.Clear();

                cacheMgr.GetData(key1);
            }

            Assert.AreEqual(1, instrumentationListener.eventArgs.Count);
            Assert.AreSame(typeof(CacheAccessedEventArgs), instrumentationListener.eventArgs[0].GetType());
            Assert.IsFalse(((CacheAccessedEventArgs)instrumentationListener.eventArgs[0]).Hit);
        }

        [TestMethod]
        public void CacheHitDoesFireEvents()
        {
            factoryHelper.doExpirations = false;

            CacheManager cacheMgr
                = factoryHelper.BuildCacheManager(instanceName, backingStore, 100, 1, 1, instrumentationProvider);
            cacheMgr.Add(key1, new object(), CacheItemPriority.NotRemovable, new NullCallback(), new NeverExpired());
            instrumentationListener.eventArgs.Clear();

            cacheMgr.GetData(key1);
            Assert.AreEqual(1, instrumentationListener.eventArgs.Count);
            Assert.AreSame(typeof(CacheAccessedEventArgs), instrumentationListener.eventArgs[0].GetType());
            Assert.IsTrue(((CacheAccessedEventArgs)instrumentationListener.eventArgs[0]).Hit);
        }

        [TestMethod]
        public void CacheHitForExpiredItemDoesFireEvents()
        {
            factoryHelper.doExpirations = false;

            using (CacheManager cacheMgr
                = factoryHelper.BuildCacheManager(instanceName, backingStore, 100, 1, 1, instrumentationProvider))
            {
                cacheMgr.Add(key1, new object(), CacheItemPriority.NotRemovable, new NullCallback(), new AlwaysExpired());
                instrumentationListener.eventArgs.Clear();

                cacheMgr.GetData(key1);
            }

            Assert.AreEqual(3, instrumentationListener.eventArgs.Count);
            IList<CacheAccessedEventArgs> accesses = FilterEventArgs<CacheAccessedEventArgs>(instrumentationListener.eventArgs);
            IList<CacheExpiredEventArgs> expiries = FilterEventArgs<CacheExpiredEventArgs>(instrumentationListener.eventArgs);
            IList<CacheUpdatedEventArgs> updates = FilterEventArgs<CacheUpdatedEventArgs>(instrumentationListener.eventArgs);
            Assert.AreEqual(1, accesses.Count);
            Assert.AreEqual(1, expiries.Count);
            Assert.AreEqual(1, updates.Count);
            Assert.IsFalse(accesses[0].Hit);
            Assert.AreEqual(1L, expiries[0].ItemsExpired);
            Assert.AreEqual(1L, updates[0].UpdatedEntriesCount);
        }

        [TestMethod]
        public void RemovalOfExistingItemDoesFireEvents()
        {
            factoryHelper.doExpirations = false;

            using (CacheManager cacheMgr
                = factoryHelper.BuildCacheManager(instanceName, backingStore, 100, 1, 1, instrumentationProvider))
            {
                cacheMgr.Add(key1, new object(), CacheItemPriority.NotRemovable, new NullCallback(), new NeverExpired());
                cacheMgr.Add(key2, new object(), CacheItemPriority.NotRemovable, new NullCallback(), new NeverExpired());
                cacheMgr.Add(key3, new object(), CacheItemPriority.NotRemovable, new NullCallback(), new NeverExpired());
                instrumentationListener.eventArgs.Clear();

                cacheMgr.Remove(key1);
            }

            Assert.AreEqual(1, instrumentationListener.eventArgs.Count);
            Assert.AreSame(typeof(CacheUpdatedEventArgs), instrumentationListener.eventArgs[0].GetType());
            Assert.AreEqual(2L, ((CacheUpdatedEventArgs)instrumentationListener.eventArgs[0]).TotalEntriesCount);
            Assert.AreEqual(1L, ((CacheUpdatedEventArgs)instrumentationListener.eventArgs[0]).UpdatedEntriesCount);
        }

        [TestMethod]
        public void RemovalOfNonExistingItemDoesNotFireEvents()
        {
            factoryHelper.doExpirations = false;

            using (CacheManager cacheMgr
                = factoryHelper.BuildCacheManager(instanceName, backingStore, 100, 1, 1, instrumentationProvider))
            {
                cacheMgr.Add(key2, new object(), CacheItemPriority.NotRemovable, new NullCallback(), new NeverExpired());
                cacheMgr.Add(key3, new object(), CacheItemPriority.NotRemovable, new NullCallback(), new NeverExpired());
                instrumentationListener.eventArgs.Clear();

                cacheMgr.Remove(key1);
            }
            Assert.AreEqual(0, instrumentationListener.eventArgs.Count);
        }

        [TestMethod]
        public void CacheFlushDoesFireEvents()
        {
            factoryHelper.doExpirations = false;

            using (CacheManager cacheMgr
                = factoryHelper.BuildCacheManager(instanceName, backingStore, 100, 1, 1, instrumentationProvider))
            {
                cacheMgr.Add(key1, new object(), CacheItemPriority.NotRemovable, new NullCallback(), new NeverExpired());
                cacheMgr.Add(key2, new object(), CacheItemPriority.NotRemovable, new NullCallback(), new NeverExpired());
                cacheMgr.Add(key3, new object(), CacheItemPriority.NotRemovable, new NullCallback(), new NeverExpired());
                instrumentationListener.eventArgs.Clear();

                cacheMgr.Flush();
            }

            Assert.AreEqual(1, instrumentationListener.eventArgs.Count);
            Assert.AreSame(typeof(CacheUpdatedEventArgs), instrumentationListener.eventArgs[0].GetType());
            Assert.AreEqual(0L, ((CacheUpdatedEventArgs)instrumentationListener.eventArgs[0]).TotalEntriesCount);
            Assert.AreEqual(3L, ((CacheUpdatedEventArgs)instrumentationListener.eventArgs[0]).UpdatedEntriesCount);
        }

        [TestMethod]
        // time dependent
        public void CallbackFailureDoesFireEvents()
        {
            CacheItem removedItem = new CacheItem(key1, new object(), CacheItemPriority.NotRemovable, new ExceptionThrowingCallback());
            RefreshActionInvoker.InvokeRefreshAction(removedItem, CacheItemRemovedReason.Removed, instrumentationProvider);

            Thread.Sleep(100);

            Assert.AreEqual(1, instrumentationListener.eventArgs.Count);
            IList<CacheCallbackFailureEventArgs> failures = FilterEventArgs<CacheCallbackFailureEventArgs>(instrumentationListener.eventArgs);

            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual(key1, failures[0].Key);
        }

        [TestMethod]
        public void CacheExpirationDoesFireEvents()
        {
            ICacheOperations cacheOperations = new MockCacheOperations();
            ExpirationTask expiration = new ExpirationTask(cacheOperations, instrumentationProvider);

            CacheItem expiredItem1 = new CacheItem(key1, new object(), CacheItemPriority.NotRemovable, null, new AlwaysExpired());
            cacheOperations.CurrentCacheState.Add(key1, expiredItem1);
            CacheItem expiredItem2 = new CacheItem(key2, new object(), CacheItemPriority.NotRemovable, null, new AlwaysExpired());
            cacheOperations.CurrentCacheState.Add(key2, expiredItem2);

            expiration.DoExpirations();

            Assert.AreEqual(1, instrumentationListener.eventArgs.Count);
            IList<CacheExpiredEventArgs> expiries = FilterEventArgs<CacheExpiredEventArgs>(instrumentationListener.eventArgs);
            Assert.AreEqual(1, expiries.Count);
            Assert.AreEqual(2L, expiries[0].ItemsExpired);
        }

        [TestMethod]
        public void CacheExpirationUnsuccessfulDoesNotFireEvents()
        {
            ICacheOperations cacheOperations = new MockCacheOperations();
            ExpirationTask expiration = new ExpirationTask(cacheOperations, instrumentationProvider);

            CacheItem expiredItem1 = new CacheItem(key1, new object(), CacheItemPriority.NotRemovable, null, new NeverExpired());
            cacheOperations.CurrentCacheState.Add(key1, expiredItem1);
            CacheItem expiredItem2 = new CacheItem(key2, new object(), CacheItemPriority.NotRemovable, null, new NeverExpired());
            cacheOperations.CurrentCacheState.Add(key2, expiredItem2);

            expiration.DoExpirations();

            Assert.AreEqual(0, instrumentationListener.eventArgs.Count);
        }

        [TestMethod]
        public void RemovalFailureDuringCacheExpirationDoesFireEvents()
        {
            ICacheOperations cacheOperations = new FailingOnRemovalCacheOperations();
            ExpirationTask expiration = new ExpirationTask(cacheOperations, instrumentationProvider);

            CacheItem expiredItem1 = new CacheItem(key1, new object(), CacheItemPriority.NotRemovable, new ExceptionThrowingCallback(), new AlwaysExpired());
            cacheOperations.CurrentCacheState.Add(key1, expiredItem1);
            expiredItem1.WillBeExpired = true;

            expiration.SweepExpiredItemsFromCache(cacheOperations.CurrentCacheState);

            Assert.AreEqual(1, instrumentationListener.eventArgs.Count);
            Assert.AreSame(typeof(CacheFailureEventArgs), instrumentationListener.eventArgs[0].GetType());
        }

        [TestMethod]
        public void CacheScavengeDoesFireEvents()
        {
            ICacheOperations cacheOperations = new MockCacheOperations();
            CacheItem item1 = new CacheItem(key1, new object(), CacheItemPriority.Low, null);
            cacheOperations.CurrentCacheState.Add(key1, item1);
            CacheItem item2 = new CacheItem(key2, new object(), CacheItemPriority.Low, null);
            cacheOperations.CurrentCacheState.Add(key2, item2);
            CacheItem item3 = new CacheItem(key3, new object(), CacheItemPriority.NotRemovable, null);
            cacheOperations.CurrentCacheState.Add(key3, item3);

            ScavengerTask scavenging = new ScavengerTask(2, new CacheCapacityScavengingPolicy(2), cacheOperations, instrumentationProvider);

            scavenging.DoScavenging();
            Assert.AreEqual(1, instrumentationListener.eventArgs.Count);
            IList<CacheScavengedEventArgs> scavenges = FilterEventArgs<CacheScavengedEventArgs>(instrumentationListener.eventArgs);
            Assert.AreEqual(1, scavenges.Count);
            Assert.AreEqual(2L, scavenges[0].ItemsScavenged);
        }

        [TestMethod]
        public void CacheScavengeUnsuccessfulDoesFireEvents()
        {
            ICacheOperations cacheOperations = new FailingOnRemovalCacheOperations();
            CacheItem item1 = new CacheItem(key1, new object(), CacheItemPriority.NotRemovable, null);
            cacheOperations.CurrentCacheState.Add(key1, item1);
            CacheItem item2 = new CacheItem(key2, new object(), CacheItemPriority.NotRemovable, null);
            cacheOperations.CurrentCacheState.Add(key2, item2);
            CacheItem item3 = new CacheItem(key3, new object(), CacheItemPriority.NotRemovable, null);
            cacheOperations.CurrentCacheState.Add(key3, item3);

            ScavengerTask scavenging = new ScavengerTask(2, new CacheCapacityScavengingPolicy(2), cacheOperations, instrumentationProvider);

            scavenging.DoScavenging();

            Assert.AreEqual(1, instrumentationListener.eventArgs.Count);
            IList<CacheScavengedEventArgs> scavenges = FilterEventArgs<CacheScavengedEventArgs>(instrumentationListener.eventArgs);
            Assert.AreEqual(1, scavenges.Count);
            Assert.AreEqual(0L, scavenges[0].ItemsScavenged);
        }

        [TestMethod]
        public void RemovalFailureDuringCacheScavengingDoesFireEvents()
        {
            ICacheOperations cacheOperations = new FailingOnRemovalCacheOperations();
            CacheItem item = new CacheItem(key1, new object(), CacheItemPriority.Low, null);
            cacheOperations.CurrentCacheState.Add(key1, item);
            cacheOperations.CurrentCacheState.Add("differentKey", item);

            ScavengerTask scavenging = new ScavengerTask(1, new CacheCapacityScavengingPolicy(1), cacheOperations, instrumentationProvider);

            scavenging.DoScavenging();

            // two events for each of the items failing to be scavenged, and one for the announcement
            // that scavenging is finished with 0 items scavenged.
            Assert.AreEqual(3, instrumentationListener.eventArgs.Count);

            IList<CacheScavengedEventArgs> scavenges = FilterEventArgs<CacheScavengedEventArgs>(instrumentationListener.eventArgs);
            IList<CacheFailureEventArgs> failures = FilterEventArgs<CacheFailureEventArgs>(instrumentationListener.eventArgs);
            Assert.AreEqual(1, scavenges.Count);
            Assert.AreEqual(2, failures.Count);
        }

        IList<TArgs> FilterEventArgs<TArgs>(IEnumerable<EventArgs> sourceList)
            where TArgs : EventArgs
        {
            IList<TArgs> result = new List<TArgs>();

            foreach (EventArgs arg in sourceList)
            {
                if (arg is TArgs)
                    result.Add(arg as TArgs);
            }

            return result;
        }

        class MockCacheManagerFactoryHelper : CacheManagerFactoryHelper
        {
            public bool doExpirations = true;

            public override ExpirationTask CreateExpirationTask(ICacheOperations cacheOperations,
                                                                CachingInstrumentationProvider instrumentationProvider)
            {
                return doExpirations
                           ? base.CreateExpirationTask(cacheOperations, instrumentationProvider)
                           : new NullExpirationTask(cacheOperations, instrumentationProvider);
            }
        }

        class NullExpirationTask : ExpirationTask
        {
            public NullExpirationTask(ICacheOperations cacheOperations,
                                      CachingInstrumentationProvider instrumentationProvider)
                : base(cacheOperations, instrumentationProvider) { }

            public override int MarkAsExpired(Hashtable liveCacheRepresentation)
            {
                // do nothing
                return 0;
            }

            public override int SweepExpiredItemsFromCache(Hashtable liveCacheRepresentation)
            {
                // do nothing
                return 0;
            }
        }

        public class MockCachingInstrumentationListener
        {
            public IList<EventArgs> eventArgs = new List<EventArgs>();

            [InstrumentationConsumer("CacheAccessed")]
            public void CacheAccessed(object sender,
                                      CacheAccessedEventArgs args)
            {
                SaveEventArgs(args);
            }

            [InstrumentationConsumer("CacheCallbackFailed")]
            public void CacheCallbackFailed(object sender,
                                            CacheCallbackFailureEventArgs args)
            {
                SaveEventArgs(args);
            }

            [InstrumentationConsumer("CacheExpired")]
            public void CacheExpired(object sender,
                                     CacheExpiredEventArgs args)
            {
                SaveEventArgs(args);
            }

            [InstrumentationConsumer("CacheFailed")]
            public void CacheFailed(object sender,
                                    CacheFailureEventArgs args)
            {
                SaveEventArgs(args);
            }

            [InstrumentationConsumer("CacheScavenged")]
            public void CacheScavenged(object sender,
                                       CacheScavengedEventArgs args)
            {
                SaveEventArgs(args);
            }

            [InstrumentationConsumer("CacheUpdated")]
            public void CacheUpdated(object sender,
                                     CacheUpdatedEventArgs args)
            {
                SaveEventArgs(args);
            }

            void SaveEventArgs(EventArgs args)
            {
                lock (eventArgs)
                {
                    eventArgs.Add(args);
                }
            }
        }

        class NullCallback : ICacheItemRefreshAction
        {
            public void Refresh(string removedKey,
                                object expiredValue,
                                CacheItemRemovedReason removalReason)
            {
                // do nothing
            }
        }

        class ExceptionThrowingCallback : ICacheItemRefreshAction
        {
            public void Refresh(string removedKey,
                                object expiredValue,
                                CacheItemRemovedReason removalReason)
            {
                throw new Exception("Exception during callback");
            }
        }

        class MockCacheOperations : ICacheOperations
        {
            Hashtable currentCacheState = new Hashtable();

            public Hashtable CurrentCacheState
            {
                get { return currentCacheState; }
            }

            public virtual void RemoveItemFromCache(string key,
                                                    CacheItemRemovedReason removalReason) { }
        }

        class FailingOnRemovalCacheOperations : MockCacheOperations
        {
            public override void RemoveItemFromCache(string key,
                                                     CacheItemRemovedReason removalReason)
            {
                throw new Exception("Explicit failure");
            }
        }
    }
}
