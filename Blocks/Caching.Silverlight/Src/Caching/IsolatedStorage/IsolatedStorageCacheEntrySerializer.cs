using System;
using System.IO;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public class IsolatedStorageCacheEntrySerializer : IIsolatedStorageCacheEntrySerializer
    {
        private readonly Encoding encoding;

        public IsolatedStorageCacheEntrySerializer()
            : this (Encoding.UTF8)
        {
        }

        public IsolatedStorageCacheEntrySerializer(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public EntryUpdate GetUpdateForLastUpdateTime(IsolatedStorageCacheEntry entry)
        {
            EntryUpdate update = new EntryUpdate();
            update.Bytes = GetDateTimeOffsetBytes(entry.LastAccessTime);
            update.Offset = lastAccessTicksOffset;

            return update;
        }

        public byte[] Serialize(IsolatedStorageCacheEntry entry)
        {
            using (var stream = new MemoryStream())
            {
                var lastAccessTimeBytes = GetDateTimeOffsetBytes(entry.LastAccessTime);

                stream.Write(lastAccessTimeBytes, 0, lastAccessTimeBytes.Length);

                var keyBytes = this.encoding.GetBytes(entry.Key);
                var policyBytes = SerializeObject(entry.Policy) ?? new byte[0];
                var valueBytes = SerializeObject(entry.Value);

                stream.Write(BitConverter.GetBytes(keyBytes.Length), 0, sizeof(int));
                stream.Write(BitConverter.GetBytes(policyBytes.Length), 0, sizeof(int));
                stream.Write(BitConverter.GetBytes(valueBytes.Length), 0, sizeof(int));
                stream.Write(keyBytes, 0, keyBytes.Length);
                stream.Write(policyBytes, 0, policyBytes.Length);
                stream.Write(valueBytes, 0, valueBytes.Length);

                return stream.ToArray();
            }
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

            CacheItemPolicy policy = null;
            if (policyLength != 0)
            {
                var policyBytes = new byte[policyLength];
                Array.Copy(bytes, keyOffset + keyLength, policyBytes, 0, policyLength);
                policy = (CacheItemPolicy)DeserializeObject(policyBytes);
            }

            var valueBytes = new byte[valueLength];
            Array.Copy(bytes, keyOffset + keyLength + policyLength, valueBytes, 0, valueLength);
            var value = DeserializeObject(valueBytes);

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

        protected virtual byte[] SerializeObject(object value)
        {
            return SerializationUtility.ToBytes(value);
        }

        protected virtual object DeserializeObject(byte[] bytes)
        {
            return SerializationUtility.ToObject(bytes);
        }
    }
}
