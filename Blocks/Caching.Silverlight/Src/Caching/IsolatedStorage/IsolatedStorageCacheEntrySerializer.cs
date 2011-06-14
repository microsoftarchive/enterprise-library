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
using System.IO;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
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
            if (entry == null) throw new ArgumentNullException("entry"); 

            return new EntryUpdate(GetDateTimeOffsetBytes(entry.LastAccessTime), lastAccessTicksOffset);
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
            if (entry == null) throw new ArgumentNullException("entry");

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
        /// <param name="serializedEntry">An array of bytes representing an entry.</param>
        /// <returns>
        /// The represented entry.
        /// </returns>
        public IsolatedStorageCacheEntry Deserialize(byte[] serializedEntry)
        {
            if (serializedEntry == null) throw new ArgumentNullException("serializedEntry");

            if (serializedEntry.Length < keyOffset)
            {
                throw new InvalidDataException(Resources.Serializer_ValueTooShort);
            }

            var lastAccessTicks = BitConverter.ToInt64(serializedEntry, lastAccessTicksOffset);
            var lastAccessOffsetTicks = BitConverter.ToInt64(serializedEntry, lastAccessOffsetTicksOffset);
            var keyLength = BitConverter.ToInt32(serializedEntry, keyLengthOffset);
            var policyLength = BitConverter.ToInt32(serializedEntry, policyLengthOffset);
            var valueLength = BitConverter.ToInt32(serializedEntry, valueLengthOffset);

            var expectedSize = keyOffset + keyLength + policyLength + valueLength;

            if (serializedEntry.Length < expectedSize)
            {
                throw new InvalidDataException(Resources.Serializer_ValueLengthDoesNotMatchMetadata);
            }
            else if (serializedEntry.Length > expectedSize)
            {
                throw new InvalidDataException(Resources.Serializer_ValueLengthDoesNotMatchMetadata);
            }

            var key = this.encoding.GetString(serializedEntry, keyOffset, keyLength);

            CacheItemPolicy policy = null;
            if (policyLength != 0)
            {
                var policyBytes = new byte[policyLength];
                Array.Copy(serializedEntry, keyOffset + keyLength, policyBytes, 0, policyLength);
                policy = (CacheItemPolicy)DeserializeObject(policyBytes);
            }

            var valueBytes = new byte[valueLength];
            Array.Copy(serializedEntry, keyOffset + keyLength + policyLength, valueBytes, 0, valueLength);
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
        /// <param name="serializedObject">The bytes representing the object.</param>
        /// <returns>The represented object.</returns>
        protected virtual object DeserializeObject(byte[] serializedObject)
        {
            return SerializationUtility.ToObject(serializedObject);
        }
    }
}
