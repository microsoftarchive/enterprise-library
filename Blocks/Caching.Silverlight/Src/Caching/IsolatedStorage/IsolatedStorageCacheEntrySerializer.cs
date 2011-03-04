using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.InMemory;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public class IsolatedStorageCacheEntrySerializer
    {
        private readonly BlockStorage blockStorage;
        private readonly Encoding encoding;

        public IsolatedStorageCacheEntrySerializer(BlockStorage blockStorage, Encoding encoding)
        {
            this.blockStorage = blockStorage;
            this.encoding = encoding;
        }

        public void Add(IsolatedStorageCacheEntry entry)
        {
            var bytes = Serialize(entry);
            entry.StorageId = this.blockStorage.Save(bytes);
        }

        public void Remove(IsolatedStorageCacheEntry entry)
        {
            this.blockStorage.Remove(entry.StorageId.Value);
            entry.StorageId = null;
        }

        public void UpdateLastUpdateTime(IsolatedStorageCacheEntry entry)
        {
            if (!entry.StorageId.HasValue)
            {
                throw new ArgumentException("Entry is not persisted.");
            }

            var lastAccessTimeBytes = GetDateTimeOffsetBytes(entry.LastAccessTime);

            this.blockStorage.Overwrite(entry.StorageId.Value, lastAccessTimeBytes, lastAccessTicksOffset);
        }

        public IEnumerable<IsolatedStorageCacheEntry> GetSerializedEntries()
        {
            var entries = new List<IsolatedStorageCacheEntry>();
            foreach (var id in this.blockStorage.GetIds())
            {
                var entry = this.Deserialize(this.blockStorage.Read(id));
                entry.StorageId = id;
                entries.Add(entry);
            }

            return entries;
        }

        public byte[] Serialize(IsolatedStorageCacheEntry entry)
        {
            var stream = new MemoryStream();

            var lastAccessTimeBytes = GetDateTimeOffsetBytes(entry.LastAccessTime);

            stream.Write(lastAccessTimeBytes, 0, lastAccessTimeBytes.Length);

            var keyBytes = this.encoding.GetBytes(entry.Key);
            var policyBytes = SerializationUtility.ToBytes(entry.Policy) ?? new byte[0];
            var valueBytes = SerializationUtility.ToBytes(entry.Value);

            stream.Write(BitConverter.GetBytes(keyBytes.Length), 0, sizeof(int));
            stream.Write(BitConverter.GetBytes(policyBytes.Length), 0, sizeof(int));
            stream.Write(BitConverter.GetBytes(valueBytes.Length), 0, sizeof(int));
            stream.Write(keyBytes, 0, keyBytes.Length);
            stream.Write(policyBytes, 0, policyBytes.Length);
            stream.Write(valueBytes, 0, valueBytes.Length);

            return stream.ToArray();
        }

        private const int lastAccessTicksOffset = 0;
        private const int lastAccessOffsetTicksOffset = sizeof(long);
        private const int keyLengthOffset = lastAccessOffsetTicksOffset + sizeof(long);
        private const int policyLengthOffset = keyLengthOffset + sizeof(int);
        private const int valueLengthOffset = policyLengthOffset + sizeof(int);
        private const int keyOffset = valueLengthOffset + sizeof(int);

        public IsolatedStorageCacheEntry Deserialize(byte[] bytes)
        {
            if (bytes.Length < keyOffset)
            {
                throw new InvalidDataException("Serialized value is too short.");
            }

            var lastAccessTicks = BitConverter.ToInt64(bytes, lastAccessTicksOffset);
            var lastAccessOffsetTicks = BitConverter.ToInt64(bytes, lastAccessOffsetTicksOffset);
            var keyLength = BitConverter.ToInt32(bytes, keyLengthOffset);
            var policyLength = BitConverter.ToInt32(bytes, policyLengthOffset);
            var valueLength = BitConverter.ToInt32(bytes, valueLengthOffset);

            var expectedSize = keyOffset + keyLength + policyLength + valueLength;

            if (bytes.Length < expectedSize)
            {
                throw new InvalidDataException("Serialized value is shorter than the size expected from its metadata.");
            }
            else if (bytes.Length > expectedSize)
            {
                throw new InvalidDataException("Serialized value is longer than the size expected from its metadata.");
            }

            var key = this.encoding.GetString(bytes, keyOffset, keyLength);

            IExtendedCacheItemPolicy policy = null;
            if (policyLength != 0)
            {
                var policyBytes = new byte[policyLength];
                Array.Copy(bytes, keyOffset + keyLength, policyBytes, 0, policyLength);
                policy = (IExtendedCacheItemPolicy)SerializationUtility.ToObject(policyBytes);
            }

            var valueBytes = new byte[valueLength];
            Array.Copy(bytes, keyOffset + keyLength + policyLength, valueBytes, 0, valueLength);
            var value = SerializationUtility.ToObject(valueBytes);

            var entry =
                new IsolatedStorageCacheEntry(key, value, policy)
                {
                    LastAccessTime = new DateTimeOffset(lastAccessTicks, new TimeSpan(lastAccessOffsetTicks)),
                };

            return entry;
        }

        private static byte[] GetDateTimeOffsetBytes(DateTimeOffset dateTimeOffset)
        {
            var lastAccessTimeBytes = new byte[sizeof(long) * 2];

            Array.Copy(BitConverter.GetBytes(dateTimeOffset.Ticks), 0, lastAccessTimeBytes, 0, sizeof(long));
            Array.Copy(BitConverter.GetBytes(dateTimeOffset.Offset.Ticks), 0, lastAccessTimeBytes, sizeof(long), sizeof(long));

            return lastAccessTimeBytes;
        }
    }
}
