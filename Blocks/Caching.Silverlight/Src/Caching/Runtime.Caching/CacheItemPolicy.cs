using System;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching
{
    public class CacheItemPolicy
    {
        public CacheItemPolicy()
        {
            AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
            SlidingExpiration = ObjectCache.NoSlidingExpiration;
            Priority = CacheItemPriority.Default;
        }

        public DateTimeOffset AbsoluteExpiration { get; set; }
        public TimeSpan SlidingExpiration { get; set; }
        public CacheItemPriority Priority { get; set; }
    }
}
