using System;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class IsolatedStorageCacheEntrySerializerFixture
    {
        private const string TestStorageName = "TestStorage";

        [TestInitialize]
        [TestCleanup]
        public void Cleanup()
        {
            BlockStorage.DeleteStorage(TestStorageName);
        }

        [TestMethod]
        public void WhenSerializesEntry_ThenCanDeserializeEntry()
        {
            var lastAccessTime = new DateTimeOffset(2011, 1, 1, 0, 0, 0, TimeSpan.FromMinutes(10)); ;
            var expiration = new DateTimeOffset(2011, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var payload = new Customer { Name = "test user", Age = 21 };

            var entry =
                new IsolatedStorageCacheEntry(
                    "test", 
                    payload, 
                    
                        new CacheItemPolicy
                        {
                            AbsoluteExpiration = expiration,
                            Priority = EnterpriseLibrary.Caching.Runtime.Caching.CacheItemPriority.NotRemovable
                        })
                            {
                                LastAccessTime = lastAccessTime,
                            };

            var serializer = new IsolatedStorageCacheEntrySerializer(null, Encoding.UTF8);

            var bytes = serializer.Serialize(entry);

            var actualEntry = serializer.Deserialize(bytes);

            Assert.AreEqual("test", actualEntry.Key);
            Assert.AreEqual(lastAccessTime, actualEntry.LastAccessTime);
            Assert.AreEqual(expiration, ((DefaultExtendedCacheItemPolicy)actualEntry.Policy).Policy.AbsoluteExpiration);
            Assert.AreEqual(EnterpriseLibrary.Caching.Runtime.Caching.CacheItemPriority.NotRemovable, actualEntry.Policy.Priority);
            Assert.AreEqual("test user", ((Customer)actualEntry.Value).Name);
            Assert.AreEqual(21, ((Customer)actualEntry.Value).Age);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void WhenSerializedEntryDoesNotContainTimestamp_ThenThrows()
        {
            using (var storage = new BlockStorage(TestStorageName, 64, 512 * 64))
            {
                var serializer = new IsolatedStorageCacheEntrySerializer(storage, Encoding.UTF8);

                var serializedEntry = serializer.Serialize(CreateEntry("test"));

                serializer.Deserialize(CropByteArray(serializedEntry, 2));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void WhenSerializedEntryDoesNotContainEntireSerializedKey_ThenThrows()
        {
            using (var storage = new BlockStorage(TestStorageName, 64, 512 * 64))
            {
                var serializer = new IsolatedStorageCacheEntrySerializer(storage, Encoding.UTF8);

                var serializedEntry = serializer.Serialize(CreateEntry("test"));

                serializer.Deserialize(CropByteArray(serializedEntry, 14));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void WhenSerializedEntryDoesNotContainPayloadLength_ThenThrows()
        {
            using (var storage = new BlockStorage(TestStorageName, 64, 512 * 64))
            {
                var serializer = new IsolatedStorageCacheEntrySerializer(storage, Encoding.UTF8);

                var serializedEntry = serializer.Serialize(CreateEntry("test"));

                serializer.Deserialize(CropByteArray(serializedEntry, 18));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void WhenSerializedEntryIsLongerThanExpected_ThenThrows()
        {
            using (var storage = new BlockStorage(TestStorageName, 64, 512 * 64))
            {
                var serializer = new IsolatedStorageCacheEntrySerializer(storage, Encoding.UTF8);

                var serializedEntry = serializer.Serialize(CreateEntry("test"));

                serializer.Deserialize(serializedEntry.Concat(new byte[2]).ToArray());
            }
        }

        [TestMethod]
        public void WhenSerializesEntryWithNoPolicy_ThenCanDeserializeEntry()
        {
            var lastAccessTime = DateTimeOffset.UtcNow;

            var payload = new Customer { Name = "test user", Age = 21 };

            var entry =
                new IsolatedStorageCacheEntry("test", payload, (IExtendedCacheItemPolicy)null)
                {
                    LastAccessTime = lastAccessTime
                };

            var encoding = Encoding.UTF8;

            var serializer = new IsolatedStorageCacheEntrySerializer(null, encoding);

            var bytes = serializer.Serialize(entry);

            var actualEntry = serializer.Deserialize(bytes);

            Assert.AreEqual("test", actualEntry.Key);
            Assert.AreEqual(lastAccessTime, actualEntry.LastAccessTime);
            Assert.IsNull(actualEntry.Policy);
            Assert.AreEqual("test user", ((Customer)actualEntry.Value).Name);
            Assert.AreEqual(21, ((Customer)actualEntry.Value).Age);
        }

        [TestMethod]
        public void WhenAddingEntry_ThenAllocatesStorageAndSavesEntryToStorage()
        {
            using (var storage = new BlockStorage(TestStorageName, 64, 512 * 64))
            {
                var serializer = new IsolatedStorageCacheEntrySerializer(storage, Encoding.UTF8);

                var lastAccessTime = DateTimeOffset.UtcNow;
                var payload = new Customer { Name = "test user", Age = 21 };
                var entry =
                    new IsolatedStorageCacheEntry("test", payload, (IExtendedCacheItemPolicy)null)
                    {
                        LastAccessTime = lastAccessTime
                    };

                serializer.Add(entry);
                Assert.IsNotNull(entry.StorageId);

                var id = entry.StorageId.Value;

                var ids = storage.GetIds();

                CollectionAssert.AreEqual(new[] { id }, ids.ToArray());

                var entryBytes = storage.Read(id);

                var actualEntry = serializer.Deserialize(entryBytes);

                Assert.AreEqual("test", actualEntry.Key);
                Assert.AreEqual(lastAccessTime, actualEntry.LastAccessTime);
                Assert.IsNull(actualEntry.Policy);
                Assert.AreEqual("test user", ((Customer)actualEntry.Value).Name);
                Assert.AreEqual(21, ((Customer)actualEntry.Value).Age);
            }
        }

        [TestMethod]
        public void WhenRemovingAnEntry_ThenRemovesItFromBlockStorage()
        {
            using (var storage = new BlockStorage(TestStorageName, 64, 512 * 64))
            {
                var serializer = new IsolatedStorageCacheEntrySerializer(storage, Encoding.UTF8);

                var lastAccessTime = DateTimeOffset.UtcNow;
                var payload = new Customer { Name = "test user", Age = 21 };
                var entry =
                       new IsolatedStorageCacheEntry("test", payload, (IExtendedCacheItemPolicy)null)
                       {
                           LastAccessTime = lastAccessTime
                       };

                serializer.Add(entry);

                serializer.Remove(entry);

                Assert.AreEqual(0, storage.GetIds().Count());
            }
        }

        [TestMethod]
        public void WhenCreatingSerializerFromPreviouslyUsedStorage_ThenCanGetStoredEntries()
        {
            int entry1Id, entry2Id;

            using (var storage = new BlockStorage(TestStorageName, 64, 512 * 64))
            {
                var serializer = new IsolatedStorageCacheEntrySerializer(storage, Encoding.UTF8);

                var entry1 = CreateEntry("entry1");
                var entry2 = CreateEntry("entry2");

                serializer.Add(entry1);
                serializer.Add(entry2);

                entry1Id = entry1.StorageId.Value;
                entry2Id = entry2.StorageId.Value;
            }

            using (var storage = new BlockStorage(TestStorageName, 64, 512 * 64))
            {
                var serializer = new IsolatedStorageCacheEntrySerializer(storage, Encoding.UTF8);

                var entries = serializer.GetSerializedEntries();

                Assert.AreEqual(2, entries.Count());
                Assert.AreEqual("entry1", entries.Single(e => e.StorageId == entry1Id).Key);
                Assert.AreEqual("entry2", entries.Single(e => e.StorageId == entry2Id).Key);
            }
        }

        [TestMethod]
        public void WhenUpdatingTheLastAccessTimeOnAnEntry_ThenUpdatesStorage()
        {
            int entry1Id, entry2Id;
            DateTimeOffset entry1LastAccessTime;

            using (var storage = new BlockStorage(TestStorageName, 64, 512 * 64))
            {
                var serializer = new IsolatedStorageCacheEntrySerializer(storage, Encoding.UTF8);

                var entry1 = CreateEntry("entry1");
                var entry2 = CreateEntry("entry2");

                serializer.Add(entry1);
                serializer.Add(entry2);

                entry1Id = entry1.StorageId.Value;
                entry2Id = entry2.StorageId.Value;

                entry1.LastAccessTime = entry1.LastAccessTime + TimeSpan.FromMinutes(10d);
                entry1LastAccessTime = entry1.LastAccessTime;
                serializer.UpdateLastUpdateTime(entry1);
            }

            using (var storage = new BlockStorage(TestStorageName, 64, 512 * 64))
            {
                var serializer = new IsolatedStorageCacheEntrySerializer(storage, Encoding.UTF8);

                var actualEntry = serializer.GetSerializedEntries().Single(e => e.StorageId == entry1Id);

                Assert.AreEqual(entry1LastAccessTime, actualEntry.LastAccessTime);
            }
        }

        private static IsolatedStorageCacheEntry CreateEntry(string key)
        {
            var lastAccessTime = DateTimeOffset.UtcNow;
            var expiration = new DateTimeOffset(2011, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var payload = new Customer { Name = "test user", Age = 21 };

            var entry =
                new IsolatedStorageCacheEntry(
                    key, 
                    payload,
                    new CacheItemPolicy
                        {
                            AbsoluteExpiration = expiration,
                            Priority = EnterpriseLibrary.Caching.Runtime.Caching.CacheItemPriority.NotRemovable
                        })
                            {
                                LastAccessTime = lastAccessTime
                            };

            return entry;
        }

        private byte[] CropByteArray(byte[] bytes, int newSize)
        {
            var result = new byte[newSize];

            Array.Copy(bytes, result, newSize);

            return result;
        }
    }

    public class Customer
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
