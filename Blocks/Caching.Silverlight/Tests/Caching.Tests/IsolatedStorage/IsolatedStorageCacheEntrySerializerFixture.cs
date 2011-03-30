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

            var serializer = new IsolatedStorageCacheEntrySerializer();

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
        [ExpectedException(typeof(InvalidDataException))]
        public void WhenSerializedEntryDoesNotContainTimestamp_ThenThrows()
        {
            var serializer = new IsolatedStorageCacheEntrySerializer();

            var serializedEntry = serializer.Serialize(CreateEntry("test"));

            serializer.Deserialize(CropByteArray(serializedEntry, 2));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void WhenSerializedEntryDoesNotContainEntireSerializedKey_ThenThrows()
        {
            var serializer = new IsolatedStorageCacheEntrySerializer();

            var serializedEntry = serializer.Serialize(CreateEntry("test"));

            serializer.Deserialize(CropByteArray(serializedEntry, 14));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void WhenSerializedEntryDoesNotContainPayloadLength_ThenThrows()
        {
            var serializer = new IsolatedStorageCacheEntrySerializer();

            var serializedEntry = serializer.Serialize(CreateEntry("test"));

            serializer.Deserialize(CropByteArray(serializedEntry, 18));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void WhenSerializedEntryIsLongerThanExpected_ThenThrows()
        {
            var serializer = new IsolatedStorageCacheEntrySerializer();

            var serializedEntry = serializer.Serialize(CreateEntry("test"));

            serializer.Deserialize(serializedEntry.Concat(new byte[2]).ToArray());
        }

        [TestMethod]
        public void WhenSerializesEntryWithNoPolicy_ThenCanDeserializeEntry()
        {
            var lastAccessTime = DateTimeOffset.UtcNow;

            var payload = new Customer { Name = "test user", Age = 21 };

            var entry =
                new IsolatedStorageCacheEntry("test", payload, null)
                {
                    LastAccessTime = lastAccessTime
                };

            var serializer = new IsolatedStorageCacheEntrySerializer();

            var bytes = serializer.Serialize(entry);

            var actualEntry = serializer.Deserialize(bytes);

            Assert.AreEqual("test", actualEntry.Key);
            Assert.AreEqual(lastAccessTime, actualEntry.LastAccessTime);
            Assert.IsNull(actualEntry.Policy);
            Assert.AreEqual("test user", ((Customer)actualEntry.Value).Name);
            Assert.AreEqual(21, ((Customer)actualEntry.Value).Age);
        }

        [TestMethod]
        public void WhenGettingUpdateForLastAccessTime_ThenCanApplyUpdateAndDeserialize()
        {
            var serializer = new IsolatedStorageCacheEntrySerializer();

            var entry = CreateEntry("entry");
            var serializedEntry = serializer.Serialize(entry);

            entry.LastAccessTime = entry.LastAccessTime + TimeSpan.FromMinutes(10d);
            DateTimeOffset entry1LastAccessTime = entry.LastAccessTime;
            
            var update = serializer.GetUpdateForLastUpdateTime(entry);

            Array.Copy(update.Bytes, 0, serializedEntry, update.Offset, update.Bytes.Length);

            var actualEntry = serializer.Deserialize(serializedEntry);

            Assert.AreEqual(entry1LastAccessTime, actualEntry.LastAccessTime);
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
