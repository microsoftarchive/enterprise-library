using System;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.ContainerModelScenarios.given_configuration_source_with_cache_settings
{
    public abstract class Context : ArrangeActAssert
    {
        protected IConfigurationSource configurationSource;

        protected override void Arrange()
        {
            base.Arrange();

            var resources = new ResourceDictionary();

            resources.Add(
                BlockSectionNames.Caching,
                new CachingSettings
                {
                    DefaultCache = "cache2",
                    Caches =
                    {
                        new InMemoryCacheData
                        {
                            Name = "cache1",
                            ItemsLeftAfterScavenging = 400,
                            MaxItemsBeforeScavenging = 500,
                            ExpirationPollingInterval = TimeSpan.FromMinutes(1)
                        },
                        new IsolatedStorageCacheData
                        {
                            Name = "cache2",
                            MaxSizeInKiloBytes = 5
                        }
                    }
                });

            this.configurationSource = new ResourceDictionaryConfigurationSource(resources);
        }
    }
}
