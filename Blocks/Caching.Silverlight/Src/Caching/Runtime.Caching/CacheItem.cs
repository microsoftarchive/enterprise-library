namespace Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching
{
    /// <summary>
    /// Represents an individual cache entry in the cache.
    /// </summary>
    public class CacheItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheItem"/> class.
        /// </summary>
        /// <param name="key">A unique identifier for a CacheItem entry.</param>
        public CacheItem(string key)
        {
            Key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheItem"/> class.
        /// </summary>
        /// <param name="key">A unique identifier for a CacheItem entry.</param>
        /// <param name="value">The data for a CacheItem entry.</param>
        public CacheItem(string key, object value)
            : this(key)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheItem"/> class.
        /// </summary>
        /// <param name="key">A unique identifier for a CacheItem entry.</param>
        /// <param name="value">The data for a CacheItem entry.</param>
        /// <param name="regionName">The name of a region in the cache that will contain the CacheItem entry.</param>
        public CacheItem(string key, object value, string regionName)
            : this(key, value)
        {
            RegionName = regionName;
        }

        /// <summary>
        /// Gets or sets a unique identifier for a CacheItem instance.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the data for a CacheItem instance.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the name of a region in the cache that contains a CacheItem entry.
        /// </summary>
        public string RegionName { get; set; }
    }
}
