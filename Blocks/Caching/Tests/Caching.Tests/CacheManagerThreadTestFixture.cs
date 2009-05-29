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
using System.Linq;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.Collections.Generic;

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

            var defaultProvider = TypeRegistrationsProvider.CreateDefaultProvider();

            var registrationProvider = new CompositeTypeRegistrationsProviderLocator(defaultProvider, new RaceConditionSimulatingExpirationTaskRegistrationProvider());
            
            IUnityContainer container = new UnityContainer();
            var configurator = new UnityContainerConfigurator(container);
            UnityServiceLocator unityServiceLocator = new UnityServiceLocator(container);

            EnterpriseLibraryContainer.ConfigureContainer(registrationProvider, configurator, TestConfigurationSource.GenerateConfiguration());

            cacheManager = (CacheManager)unityServiceLocator.GetInstance<ICacheManager>("ShortInMemoryPersistence");
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

        private class RaceConditionSimulatingExpirationTaskRegistrationProvider : TypeRegistrationsProvider
        {
            public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
            {
                return
                    new RaceConditionSimulationExpirationTaskTypeRegistrationProvider().GetRegistrations(
                        configurationSource);
            }

            private class RaceConditionSimulationExpirationTaskTypeRegistrationProvider : ITypeRegistrationsProvider
            {
                #region ITypeRegistrationsProvider Members

                public IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
                {
                    var registration = new TypeRegistration<ExpirationTask>( 
                        () => new RaceConditionSimulatingExpirationTask(Container.Resolved<Cache>("ShortInMemoryPersistence"),
                                                               Container.Resolved<ICachingInstrumentationProvider>("ShortInMemoryPersistence")))
                                                               {
                                                                   Name = "ShortInMemoryPersistence"
                                                               };

                    return new TypeRegistration[] { registration };
                }

                /// <summary>
                /// Return the <see cref="TypeRegistration"/> objects needed to reconfigure
                /// the container after a configuration source has changed.
                /// </summary>
                /// <remarks>If there are no reregistrations, return an empty sequence.</remarks>
                /// <param name="configurationSource">The <see cref="IConfigurationSource"/> containing
                /// the configuration information.</param>
                /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
                public IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
                {
                    return Enumerable.Empty<TypeRegistration>();
                }

                #endregion
            }
        }

        public class RaceConditionSimulatingExpirationTask : ExpirationTask
        {
            public RaceConditionSimulatingExpirationTask(ICacheOperations cacheOperations,
                                                         ICachingInstrumentationProvider instrumentationProvider)
                : base(cacheOperations, instrumentationProvider) { }

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
