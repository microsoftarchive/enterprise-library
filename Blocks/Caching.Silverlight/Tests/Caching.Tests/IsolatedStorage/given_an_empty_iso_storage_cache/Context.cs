using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage.given_an_empty_iso_storage_cache
{
    public class Context : ArrangeActAssert
    {
        protected ObjectCache Cache;
        protected long MaxSize = 100;
        protected const int QuotaUsedBeforeScavenging = 80;
        protected const int QuotaUsedAfterScavenging = 80;
        protected TimeSpan PollingInterval = TimeSpan.FromMinutes(1);
        protected const string CacheName = "sample_cache_name";
        protected override void Arrange()
        {
            base.Arrange();

            IsolatedStorageCache.DeleteCache(CacheName);
            RefreshCache();
        }

        protected virtual void RefreshCache()
        {
            using (Cache as IDisposable) { }
            Cache = new IsolatedStorageCache(CacheName, MaxSize, QuotaUsedBeforeScavenging, QuotaUsedAfterScavenging, PollingInterval, new IsolatedStorageCacheEntrySerializer());
        }

        protected override void Teardown()
        {
            base.Teardown();

            using (Cache as IDisposable) { }
            Cache = null;
            IsolatedStorageCache.DeleteCache(CacheName);
        }
    }
}
