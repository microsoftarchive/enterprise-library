using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// Configuration object for an <see cref="IsolatedStorageCache"/>.
    /// </summary>
    public class IsolatedStorageCacheData : CacheData
    {
        private readonly AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageCacheData"/> class.
        /// </summary>
        public IsolatedStorageCacheData()
        {
            this.MaxSizeInKilobytes = 1024;
            this.PercentOfQuotaUsedBeforeScavenging = 80;
            this.PercentOfQuotaUsedAfterScavenging = 60;
            this.ExpirationPollingInterval = TimeSpan.FromMinutes(2);
            this.SerializerType = typeof(IsolatedStorageCacheEntrySerializer);
        }

        /// <summary>
        /// Returns the <see cref="TypeRegistration"/> entries for this configuration object.
        /// </summary>
        /// <param name="configurationSource"></param>
        /// <returns>
        /// A set of registry entries.
        /// </returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            var type = this.SerializerType;
            if (type == null || !typeof(IIsolatedStorageCacheEntrySerializer).IsAssignableFrom(type))
                throw new InvalidOperationException("SerializerType value must be a type that implements IIsolatedStorageCacheEntrySerializer and has a default constructor.");

            IIsolatedStorageCacheEntrySerializer serializer = (IIsolatedStorageCacheEntrySerializer)Activator.CreateInstance(type);

            var cacheManagerRegistration =
                new TypeRegistration<ObjectCache>(() =>
                    new IsolatedStorageCache(this.Name, this.MaxSizeInKilobytes, this.PercentOfQuotaUsedBeforeScavenging, this.PercentOfQuotaUsedAfterScavenging, this.ExpirationPollingInterval, serializer))
                {
                    Name = this.Name,
                    IsPublicName = true
                };

            return new TypeRegistration[] { cacheManagerRegistration };
        }

        /// <summary>
        /// Gets or sets the maximum size in kilobytes for the cache.
        /// </summary>
        public int MaxSizeInKilobytes { get; set; }

        /// <summary>
        /// Gets or sets the percentage of quota before scavenging of entries needs to take place.
        /// </summary>
        public int PercentOfQuotaUsedBeforeScavenging { get; set; }

        /// <summary>
        /// Gets or sets the percentage of quota after scavenging has taken place.
        /// </summary>
        public int PercentOfQuotaUsedAfterScavenging { get; set; }

        /// <summary>
        /// Gets or sets the frequency of expiration polling cycle.
        /// </summary>
        public TimeSpan ExpirationPollingInterval { get; set; }

        /// <summary>
        /// Gets or sets name of the type used for serializing and deserializing the cache entry.
        /// </summary>
        public string SerializerTypeName { get; set; }

        /// <summary>
        /// Gets or sets the type used for serializing and deserializing the cache entry.
        /// </summary>
        public Type SerializerType
        {
            get { return (Type)typeConverter.ConvertFrom(this.SerializerTypeName); }
            set { this.SerializerTypeName = typeConverter.ConvertToString(value); }
        }
    }
}
