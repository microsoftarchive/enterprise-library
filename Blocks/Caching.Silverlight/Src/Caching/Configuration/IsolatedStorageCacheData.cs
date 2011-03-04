using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    public class IsolatedStorageCacheData : CacheData
    {
        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            var cacheManagerRegistration =
                new TypeRegistration<ObjectCache>(() =>
                    new IsolatedStorageCache(this.Name, this.MaxSize))
                {
                    Name = this.Name,
                    IsPublicName = true
                };

            return new TypeRegistration[] { cacheManagerRegistration };
        }

        public long MaxSize { get; set; }
    }
}
