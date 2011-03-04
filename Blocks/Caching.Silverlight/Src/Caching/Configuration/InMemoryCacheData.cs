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
        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            var cacheManagerRegistration =
                new TypeRegistration<ObjectCache>(() =>
                    new InMemoryCache(
                        this.Name,
                        this.MaxItemsBeforeScavenging,
                        this.ItemsLeftAfterScavenging,
                        Container.Resolved<ScavengingScheduler>(),
                        Container.Resolved<ExpirationScheduler>(GetRecurringSchedulerName())))
                {
                    Name = this.Name,
                    IsPublicName = true
                };

            var expirationTimerRegistration =
                new TypeRegistration<IRecurringScheduledWork>(() => new ExpirationScheduler(this.PollInterval))
                {
                    Name = this.GetRecurringSchedulerName(),
                    IsPublicName = false
                };

            return new TypeRegistration[] { cacheManagerRegistration, expirationTimerRegistration };
        }

        private string GetRecurringSchedulerName()
        {
            return this.Name + "__scheduler__" + this.GetHashCode();
        }

        public int MaxItemsBeforeScavenging { get; set; }

        public int ItemsLeftAfterScavenging { get; set; }

        public TimeSpan PollInterval { get; set; }
    }
}
