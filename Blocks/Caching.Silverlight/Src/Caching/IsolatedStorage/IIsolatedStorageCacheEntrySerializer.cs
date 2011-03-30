namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public struct EntryUpdate
    {
        public byte[] Bytes;
        public int Offset;
    }

    public interface IIsolatedStorageCacheEntrySerializer
    {
        byte[] Serialize(IsolatedStorageCacheEntry entry);

        IsolatedStorageCacheEntry Deserialize(byte[] bytes);

        EntryUpdate GetUpdateForLastUpdateTime(IsolatedStorageCacheEntry entry);
    }
}
