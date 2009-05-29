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
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
	[TestClass]
	public class CacheExceptionHandlingFixture
	{
		[TestMethod]
		public void ExceptionThrownDuringAddResultsInObjectBeingRemovedFromCacheCompletely()
		{
			MockBackingStore backingStore = new MockBackingStore();
			ICachingInstrumentationProvider instrumentationProvider = new NullCachingInstrumentationProvider();

			Cache cache = new Cache(backingStore, instrumentationProvider);

            try
			{
				cache.Add("foo", "bar");
				Assert.Fail("Should have thrown exception thrown internally to Cache.Add");
			}
			catch (Exception)
			{
				Assert.IsFalse(cache.Contains("foo"));
				Assert.AreEqual(1, backingStore.removalCount);
			}
		}

		[TestMethod]
		public void ExceptionThrownDuringAddIntoIsolatedStorageAllowsItemToBeReaddedLater()
		{
			using (IsolatedStorageBackingStore backingStore = new IsolatedStorageBackingStore("foo", null))
			{
				ICachingInstrumentationProvider instrumentationProvider = new NullCachingInstrumentationProvider();

				Cache cache = new Cache(backingStore, instrumentationProvider);

				try
				{
					try
					{
						cache.Add("my", new NonSerializableClass());
						Assert.Fail("Should have thrown exception internally to Cache.Add");
					}
					catch (Exception)
					{
						cache.Add("my", new SerializableClass());
						Assert.IsTrue(cache.Contains("my"));
					}
				}
				finally
				{
					backingStore.Flush();
				}
			}
		}

		[TestMethod]
		public void ItemAddedPreviousToFailedAddIsRemovedCompletelyIfSecondAddFails()
		{
			using (IsolatedStorageBackingStore backingStore = new IsolatedStorageBackingStore("foo", null))
			{
				ICachingInstrumentationProvider instrumentationProvider = new NullCachingInstrumentationProvider();

				Cache cache = new Cache(backingStore, instrumentationProvider);

				cache.Add("my", new SerializableClass());

				try
				{
					try
					{
						cache.Add("my", new NonSerializableClass());
						Assert.Fail("Should have thrown exception internally to Cache.Add");
					}
					catch (Exception)
					{
						Assert.IsFalse(cache.Contains("my"));
						Assert.AreEqual(0, backingStore.Count);

						Hashtable isolatedStorageContents = backingStore.Load();
						Assert.AreEqual(0, isolatedStorageContents.Count);
					}
				}
				finally
				{
					backingStore.Flush();
				}
			}
		}

		class NonSerializableClass { }

		[Serializable]
		class SerializableClass { }

		public class MockBackingStore : IBackingStore
		{
			public int removalCount = 0;

			public int Count
			{
				get { return 0; }
			}

			public string CurrentCacheManager
			{
				get { return string.Empty; }
				set { }
			}

			public void Add(CacheItem newCacheItem)
			{
				throw new Exception();
			}

			public void Dispose() { }

			public void Flush() { }

			public Hashtable Load()
			{
				return new Hashtable();
			}

			public void Remove(string key)
			{
				removalCount++;
			}

			public void UpdateLastAccessedTime(string key,
											   DateTime timestamp) { }
		}
	}

    class NullCachingInstrumentationProvider : ICachingInstrumentationProvider
    {
        public void FireCacheUpdated(long updatedEntriesCount, long totalEntriesCount)
        {
        }

        public void FireCacheAccessed(string key, bool hit)
        {
        }

        public void FireCacheExpired(long itemsExpired)
        {
        }

        public void FireCacheScavenged(long itemsScavenged)
        {
        }

        public void FireCacheCallbackFailed(string key, Exception exception)
        {
        }

        public void FireCacheFailed(string errorMessage, Exception exception)
        {
        }
    }
}
