using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    public class InMemoryCacheData : CacheData
    {
        public InMemoryCacheData()
        {
            this.MaxItemsBeforeScavenging = 200;
            this.ItemsLeftAfterScavenging = 80;

            this.ExpirationPollingInterval = TimeSpan.FromMinutes(2);
        }

        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            var cacheManagerRegistration =
                new TypeRegistration<ObjectCache>(() =>
                    new InMemoryCache(
                        this.Name,
                        this.MaxItemsBeforeScavenging,
                        this.ItemsLeftAfterScavenging,
                        this.ExpirationPollingInterval))
                {
                    Name = this.Name,
                    IsPublicName = true
                };


            return new TypeRegistration[] { cacheManagerRegistration };
        }

        public int MaxItemsBeforeScavenging { get; set; }

        public int ItemsLeftAfterScavenging { get; set; }

        public TimeSpan ExpirationPollingInterval { get; set; }
    }
}
