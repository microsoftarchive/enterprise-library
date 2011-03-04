using System;
using System.Linq;
using System.Text;
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
                new IsolatedStorageCacheEntry
                {
                    Key = "test",
                    LastAccessTime = lastAccessTime,
                    Policy =
                        new CacheItemPolicy
                        {
                            AbsoluteExpiration = expiration,
                            Priority = EnterpriseLibrary.Caching.Runtime.Caching.CacheItemPriority.NotRemovable
                        },
                    Value = payload
                };

            var serializer = new IsolatedStorageCacheEntrySerializer(null, Encoding.UTF8);

            var bytes = serializer.Serialize(entry);

            var actualEntry = serializer.Deserialize(bytes);

            Assert.AreEqual("test", actualEntry.Key);
            Assert.AreEqual(lastAccessTime, actualEntry.LastAccessTime);
            Assert.AreEqual(expiration, actualEntry.Policy.AbsoluteExpiration);
            Assert.AreEqual(EnterpriseLibrary.Caching.Runtime.Caching.CacheItemPriority.NotRemovable, actualEntry.Policy.Priority);
            Assert.AreEqual("test user", ((Customer)actualEntry.Value).Name);
            Assert.AreEqual(21, ((Customer)actualEntry.Value).Age);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
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
        [ExpectedException(typeof(ArgumentException))]
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
        [ExpectedException(typeof(ArgumentException))]
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
        [ExpectedException(typeof(ArgumentException))]
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
                new IsolatedStorageCacheEntry
                {
                    Key = "test",
                    LastAccessTime = lastAccessTime,
                    Policy = null,
                    Value = payload
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
                    new IsolatedStorageCacheEntry
                    {
                        Key = "test",
                        LastAccessTime = lastAccessTime,
                        Policy = null,
                        Value = payload
                    };

                var id = serializer.Add(entry);

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
                    new IsolatedStorageCacheEntry
                    {
                        Key = "test",
                        LastAccessTime = lastAccessTime,
                        Policy = null,
                        Value = payload
                    };

                var id = serializer.Add(entry);

                serializer.Remove(id);

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

                entry1Id = serializer.Add(entry1);
                entry2Id = serializer.Add(entry2);
            }

            using (var storage = new BlockStorage(TestStorageName, 64, 512 * 64))
            {
                var serializer = new IsolatedStorageCacheEntrySerializer(storage, Encoding.UTF8);

                var entries = serializer.GetSerializedEntries();

                Assert.AreEqual(2, entries.Count);
                Assert.AreEqual("entry1", entries[entry1Id].Key);
                Assert.AreEqual("entry2", entries[entry2Id].Key);
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

                entry1Id = serializer.Add(entry1);
                entry2Id = serializer.Add(entry2);

                entry1.LastAccessTime = entry1.LastAccessTime + TimeSpan.FromMinutes(10d);
                entry1LastAccessTime = entry1.LastAccessTime;
                serializer.UpdateLastUpdateTime(entry1, entry1Id);
            }

            using (var storage = new BlockStorage(TestStorageName, 64, 512 * 64))
            {
                var serializer = new IsolatedStorageCacheEntrySerializer(storage, Encoding.UTF8);

                var actualEntry = serializer.GetSerializedEntries()[entry1Id];

                Assert.AreEqual(entry1LastAccessTime, actualEntry.LastAccessTime);
            }
        }

        private static IsolatedStorageCacheEntry CreateEntry(string key)
        {
            var lastAccessTime = DateTimeOffset.UtcNow;
            var expiration = new DateTimeOffset(2011, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var payload = new Customer { Name = "test user", Age = 21 };

            var entry =
                new IsolatedStorageCacheEntry
                {
                    Key = key,
                    LastAccessTime = lastAccessTime,
                    Policy =
                        new CacheItemPolicy
                        {
                            AbsoluteExpiration = expiration,
                            Priority = EnterpriseLibrary.Caching.Runtime.Caching.CacheItemPriority.NotRemovable
                        },
                    Value = payload
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
