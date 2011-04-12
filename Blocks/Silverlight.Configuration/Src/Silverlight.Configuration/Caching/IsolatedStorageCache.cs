using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// Represent the stand-in class for the real IsolatedStorageCache class in Microsoft.Practices.EnterpriseLibrary.Caching.Silverlight
    /// </summary>
    [ConfigurationElementType(typeof(IsolatedStorageCacheData))]
    public class IsolatedStorageCache
    {
    }
}
