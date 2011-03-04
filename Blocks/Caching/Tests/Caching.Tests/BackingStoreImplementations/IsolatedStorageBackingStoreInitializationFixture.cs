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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests
{
    [TestClass]
    public class IsolatedStorageBackingStoreInitializationFixture
    {
        [TestMethod]
        public void CanInitializeBackingStoreFromConfigurationObjects()
        {
            IsolatedStorageBackingStore localStore = new IsolatedStorageBackingStore("Storage");

            localStore.Add(new CacheItem("key", "value", CacheItemPriority.Normal, null));
            localStore.Dispose();

            IsolatedStorageBackingStore testStore = new IsolatedStorageBackingStore("Storage");
            var loadedItems = testStore.Load();
            testStore.Flush();
            testStore.Dispose();

            Assert.AreEqual(1, loadedItems.Count, "One item should have been loaded into Storage backing store");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullIsolatedStorageAreaNameThrowsException()
        {
            IsolatedStorageBackingStore localStore = new IsolatedStorageBackingStore(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyIsolatedStorageAreaNameThrowsException()
        {
            IsolatedStorageBackingStore localStore = new IsolatedStorageBackingStore(string.Empty);
        }
    }
}
