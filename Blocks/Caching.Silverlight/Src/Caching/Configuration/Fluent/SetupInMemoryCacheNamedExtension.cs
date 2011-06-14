//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigureCaching"/> extensions to support configuring <see cref="InMemoryCache"/> instances.
    /// </summary>
    /// <seealso cref="InMemoryCache"/>
    /// <seealso cref="InMemoryCacheData"/>
    public static class SetupInMemoryCacheNamedExtension
    {
        /// <summary>
        /// Adds a <see cref="InMemoryCache"/> to the caching configuration settings.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="cacheName">The name of the <see cref="InMemoryCache"/> that should be configured.</param>
        /// <returns>Fluent interface to further configure the created <see cref="InMemoryCacheData"/>.</returns>
        /// <seealso cref="InMemoryCache"/>
        /// <seealso cref="InMemoryCacheData"/>
        public static ISetupInMemoryCacheNamed SetupInMemoryCacheNamed(this IConfigureCaching context, string cacheName)
        {
            if (string.IsNullOrEmpty(cacheName)) 
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "cacheName");

            return new SetupInMemoryCacheNamedBuilder(context, cacheName);
        }

        private class SetupInMemoryCacheNamedBuilder : ConfigureCachingExtension, ISetupInMemoryCacheNamed, ISetupInMemoryCacheNamedOptions
        {
            InMemoryCacheData providerData;

            public SetupInMemoryCacheNamedBuilder(IConfigureCaching context, string cacheName)
                : base(context)
            {
                providerData = new InMemoryCacheData
                {
                    Name = cacheName,
                };

                base.CachingSettings.Caches.Add(providerData);
            }

            ISetupInMemoryCacheNamed ISetupInMemoryCacheNamed.SetAsDefault()
            {
                base.CachingSettings.DefaultCache = providerData.Name;

                return this;
            }

            ISetupInMemoryCacheNamedOptions ISetupInMemoryCacheNamed.WithOptions
            {
                get { return this; }
            }

            ISetupInMemoryCacheNamedOptions ISetupInMemoryCacheNamedOptions.WithScavengingThresholds(int maxItemsBeforeScavenging, int itemsLeftAfterScavenging)
            {
                this.providerData.MaxItemsBeforeScavenging = maxItemsBeforeScavenging;
                this.providerData.ItemsLeftAfterScavenging = itemsLeftAfterScavenging;
                
                return this;
            }

            ISetupInMemoryCacheNamedOptions ISetupInMemoryCacheNamedOptions.UsingExpirationPollingInterval(TimeSpan interval)
            {
                this.providerData.ExpirationPollingInterval = interval;

                return this;
            }
        }
    }
}
