using System.IO.IsolatedStorage;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public class IsolatedStorageInfo : IIsolatedStorageInfo
    {
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
