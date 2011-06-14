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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    /// <summary>
    /// Manages serialization of entries.
    /// </summary>
    public interface IIsolatedStorageCacheEntrySerializer
    {
        /// <summary>
        /// Serializes <paramref name="entry"/> as an array of bytes.
        /// </summary>
        /// <param name="entry">The entry to serialize.</param>
        /// <returns>An array of bytes.</returns>
        byte[] Serialize(IsolatedStorageCacheEntry entry);

        /// <summary>
        /// Deserializes an entry from an array of bytes.
        /// </summary>
        /// <param name="serializedEntry">An array of bytes representing an entry.</param>
        /// <returns>The represented entry.</returns>
        IsolatedStorageCacheEntry Deserialize(byte[] serializedEntry);

        /// <summary>
        /// Generates an <see cref="EntryUpdate"/> representing an update to the last access time in an entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>The update to the serialized bytes.</returns>
        EntryUpdate GetUpdateForLastUpdateTime(IsolatedStorageCacheEntry entry);
    }

    /// <summary>
    /// Represents an update to an entry saved to isolated storage.
    /// </summary>
    public class EntryUpdate
    {
        private readonly byte[] value;

        private readonly int offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntryUpdate"/> type.
        /// </summary>
        /// <param name="value">The bytes to update.</param>
        /// <param name="offset">The offset for the update.</param>
        public EntryUpdate(byte[] value, int offset)
        {
            this.value = value;
            this.offset = offset;
        }

        /// <summary>
        /// The bytes to update.
        /// </summary>
        public byte[] GetValue()
        {
            return this.value;
        }

        /// <summary>
        /// The offset for the update.
        /// </summary>
        public int Offset
        {
            get { return this.offset; }
        }
    }
}
