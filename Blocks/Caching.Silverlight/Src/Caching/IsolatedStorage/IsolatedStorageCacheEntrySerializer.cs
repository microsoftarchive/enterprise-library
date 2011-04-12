using System;
using System.IO;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    /// <summary>
    /// Manages the serialization and deserialization of entries.
    /// </summary>
    public class IsolatedStorageCacheEntrySerializer : IIsolatedStorageCacheEntrySerializer
    {
        private const int lastAccessTicksOffset = 0;
        private const int lastAccessOffsetTicksOffset = sizeof(long);
        private const int keyLengthOffset = lastAccessOffsetTicksOffset + sizeof(long);
        private const int policyLengthOffset = keyLengthOffset + sizeof(int);
        private const int valueLengthOffset = policyLengthOffset + sizeof(int);
        private const int keyOffset = valueLengthOffset + sizeof(int);

        private readonly Encoding encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageCacheEntrySerializer"/> class.
        /// </summary>
        public IsolatedStorageCacheEntrySerializer()
            : this(Encoding.UTF8)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageCacheEntrySerializer"/> class.
        /// </summary>
        /// <param name="encoding">The encoding to use when serializing text.</param>
        public IsolatedStorageCacheEntrySerializer(Encoding encoding)
        {
            this.encoding = encoding;
        }

        /// <summary>
        /// Generates an <see cref="EntryUpdate"/> representing an update to the last access time in an entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>
        /// The update to the serialized bytes.
        /// </returns>
        public EntryUpdate GetUpdateForLastUpdateTime(IsolatedStorageCacheEntry entry)
        {
            EntryUpdate update = new EntryUpdate();
            update.Bytes = GetDateTimeOffsetBytes(entry.LastAccessTime);
            update.Offset = lastAccessTicksOffset;

            return update;
        }

        /// <summary>
        /// Serializes <paramref name="entry"/> as an array of bytes.
        /// </summary>
        /// <param name="entry">The entry to serialize.</param>
        /// <returns>
        /// An array of bytes.
        /// </returns>
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

        /// <summary>
        /// Deserializes an entry from an array of bytes.
        /// </summary>
        /// <param name="bytes">An array of bytes representing an entry.</param>
        /// <returns>
        /// The represented entry.
        /// </returns>
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

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>An array of bytes representing <paramref name="value"/>.</returns>
        protected virtual byte[] SerializeObject(object value)
        {
            return SerializationUtility.ToBytes(value);
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="bytes">The bytes representing the object.</param>
        /// <returns>The represented object.</returns>
        protected virtual object DeserializeObject(byte[] bytes)
        {
            return SerializationUtility.ToObject(bytes);
        }
    }
}
