using System.IO.IsolatedStorage;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    /// <summary>
    /// Provides information about the isolated storage.
    /// </summary>
    public class IsolatedStorageInfo : IIsolatedStorageInfo
    {
        /// <summary>
        /// Gets the available free space.
        /// </summary>
        public long AvailableFreeSpace
        {
            get
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    return store.AvailableFreeSpace;
                }
            }
        }
    }
}
