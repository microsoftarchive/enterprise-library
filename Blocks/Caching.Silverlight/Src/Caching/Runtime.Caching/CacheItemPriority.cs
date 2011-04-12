namespace Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching
{
    /// <summary>
    /// Specifies a priority setting that is used to decide whether to evict a cache entry.
    /// </summary>
    public enum CacheItemPriority
    {
        /// <summary>
        /// Indicates that there is no priority for removing the cache entry.
        /// </summary>
        Default,

        /// <summary>
        /// Indicates that a cache entry should never be removed from the cache.
        /// </summary>
        NotRemovable
    }
}
