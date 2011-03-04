using System;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage.given_an_iso_storage_with_data
{
    public class Context : ArrangeActAssert
    {
        protected ObjectCache Cache;
        protected const string CacheName = "initialized_cache";

        protected virtual long MaxSize
        {
            get { return 64 * 1024; }
        }

        protected override void Arrange()
        {
            base.Arrange();

            IsolatedStorageCache.DeleteCache(CacheName);
            this.RefreshCache();

            Cache.Add("key", "value", new CacheItemPolicy
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });

            Cache["largeData"] = Enumerable.Range(0, 5000).ToList();

            this.RefreshCache();
        }

        protected virtual void RefreshCache()
        {
            using (Cache as IDisposable) { }
            Cache = new IsolatedStorageCache(CacheName, MaxSize);
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
