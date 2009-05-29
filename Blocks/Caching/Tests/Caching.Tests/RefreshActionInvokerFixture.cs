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

using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestClass]
    public class RefreshActionInvokerFixture
    {
        static bool callbackHappened;
        static string removedKey;
        static object removedValue;
        static CacheItemRemovedReason callbackReason;
        public ICachingInstrumentationProvider instrumentationProvider;

        [TestInitialize]
        public void Reset()
        {
            removedValue = "Known bad value";
            callbackHappened = false;
            callbackReason = CacheItemRemovedReason.Unknown;
            removedKey = null;
            instrumentationProvider = new NullCachingInstrumentationProvider();
        }

        [TestMethod]
        public void NoCallHappensIfRefreshActionIsNull()
        {
            CacheItem emptyCacheItem = new CacheItem("key", "value", CacheItemPriority.Low, null);
            RefreshActionInvoker.InvokeRefreshAction(emptyCacheItem, CacheItemRemovedReason.Expired, instrumentationProvider);
            Assert.IsFalse(callbackHappened);
        }

        [TestMethod]
        public void CallbackDoesHappenIfRefreshActionIsSet()
        {
            CacheItem emptyCacheItem = new CacheItem("key", "value", CacheItemPriority.Low, new MockRefreshAction());
            RefreshActionInvoker.InvokeRefreshAction(emptyCacheItem, CacheItemRemovedReason.Expired, instrumentationProvider);
            Thread.Sleep(100);
            Assert.IsTrue(callbackHappened);
        }

        [TestMethod]
        public void RemovedValueIsGivenToCallbackMethod()
        {
            CacheItem emptyCacheItem = new CacheItem("key", "value", CacheItemPriority.Low, new MockRefreshAction());
            RefreshActionInvoker.InvokeRefreshAction(emptyCacheItem, CacheItemRemovedReason.Expired, instrumentationProvider);
            Thread.Sleep(100);
            Assert.AreEqual("value", removedValue);
        }

        [TestMethod]
        public void RemovedValueCanBeNullDuringCallback()
        {
            CacheItem emptyCacheItem = new CacheItem("key", null, CacheItemPriority.Low, new MockRefreshAction());
            RefreshActionInvoker.InvokeRefreshAction(emptyCacheItem, CacheItemRemovedReason.Expired, instrumentationProvider);
            Thread.Sleep(100);
            Assert.IsNull(removedValue);
        }

        [TestMethod]
        public void CallbackReasonIsGivenToCallbackMethod()
        {
            CacheItem emptyCacheItem = new CacheItem("key", null, CacheItemPriority.Low, new MockRefreshAction());
            RefreshActionInvoker.InvokeRefreshAction(emptyCacheItem, CacheItemRemovedReason.Expired, instrumentationProvider);
            Thread.Sleep(100);
            Assert.AreEqual(CacheItemRemovedReason.Expired, callbackReason);
        }

        [TestMethod]
        public void CallbackReasonScavengedIsGivenToCallbackMethod()
        {
            CacheItem emptyCacheItem = new CacheItem("key", null, CacheItemPriority.Low, new MockRefreshAction());
            RefreshActionInvoker.InvokeRefreshAction(emptyCacheItem, CacheItemRemovedReason.Scavenged, instrumentationProvider);
            Thread.Sleep(100);
            Assert.AreEqual(CacheItemRemovedReason.Scavenged, callbackReason);
        }

        [TestMethod]
        public void KeyIsGivenToCallbackMethod()
        {
            CacheItem emptyCacheItem = new CacheItem("key", null, CacheItemPriority.Low, new MockRefreshAction());
            RefreshActionInvoker.InvokeRefreshAction(emptyCacheItem, CacheItemRemovedReason.Scavenged, instrumentationProvider);
            Thread.Sleep(100);
            Assert.AreEqual("key", removedKey);
        }

        class MockRefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string key,
                                object expiredValue,
                                CacheItemRemovedReason removalReason)
            {
                callbackHappened = true;
                removedValue = expiredValue;
                callbackReason = removalReason;
                removedKey = key;
            }
        }
    }
}
