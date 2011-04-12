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
        /// <param name="bytes">An array of bytes representing an entry.</param>
        /// <returns>The represented entry.</returns>
        IsolatedStorageCacheEntry Deserialize(byte[] bytes);

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
    public struct EntryUpdate
    {
        /// <summary>
        /// The bytes to update.
        /// </summary>
        public byte[] Bytes;

        /// <summary>
        /// The offset for the update.
        /// </summary>
        public int Offset;
    }
}
