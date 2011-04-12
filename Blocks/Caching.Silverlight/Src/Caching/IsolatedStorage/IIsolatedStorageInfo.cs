namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    /// <summary>
    /// Provides information about the isolated storage.
    /// </summary>
    public interface IIsolatedStorageInfo
    {
        /// <summary>
        /// Gets the available free space.
        /// </summary>
        long AvailableFreeSpace { get; }
    }
}
