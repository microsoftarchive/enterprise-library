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
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using CommonResources = Microsoft.Practices.EnterpriseLibrary.Common.Properties.Resources;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigureCaching"/> extensions to support configuring <see cref="IsolatedStorageCache"/> instances.
    /// </summary>
    /// <seealso cref="IsolatedStorageCache"/>
    /// <seealso cref="IsolatedStorageCacheData"/>
    public static class SetupIsolatedStorageCacheNamedExtension
    {
        /// <summary>
        /// Adds a <see cref="IsolatedStorageCache"/> to the caching configuration settings.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="cacheName">The name of the <see cref="IsolatedStorageCache"/> that should be configured.</param>
        /// <returns>Fluent interface to further configure the created <see cref="IsolatedStorageCacheData"/>.</returns>
        /// <seealso cref="IsolatedStorageCache"/>
        /// <seealso cref="IsolatedStorageCacheData"/>
        public static ISetupIsolatedStorageCacheNamed SetupIsolatedStorageCacheNamed(this IConfigureCaching context, string cacheName)
        {
            if (string.IsNullOrEmpty(cacheName))
                throw new ArgumentException(CommonResources.ExceptionStringNullOrEmpty, "cacheName");

            return new SetupIsolatedStorageCacheNamedBuilder(context, cacheName);
        }

        private class SetupIsolatedStorageCacheNamedBuilder : ConfigureCachingExtension, ISetupIsolatedStorageCacheNamed, ISetupIsolatedStorageCacheNamedOptions
        {
            IsolatedStorageCacheData providerData;

            public SetupIsolatedStorageCacheNamedBuilder(IConfigureCaching context, string cacheName)
                : base(context)
            {
                providerData = new IsolatedStorageCacheData
                {
                    Name = cacheName,
                };

                base.CachingSettings.Caches.Add(providerData);
            }

            ISetupIsolatedStorageCacheNamed ISetupIsolatedStorageCacheNamed.SetAsDefault()
            {
                base.CachingSettings.DefaultCache = providerData.Name;

                return this;
            }

            ISetupIsolatedStorageCacheNamedOptions ISetupIsolatedStorageCacheNamed.WithOptions
            {
                get { return this; }
            }

            ISetupIsolatedStorageCacheNamedOptions ISetupIsolatedStorageCacheNamedOptions.UsingExpirationPollingInterval(TimeSpan interval)
            {
                this.providerData.ExpirationPollingInterval = interval;

                return this;
            }

            ISetupIsolatedStorageCacheNamedOptions ISetupIsolatedStorageCacheNamedOptions.WithScavengingThresholds(int percentOfQuotaUsedBeforeScavenging, int percentOfQuotaUsedAfterScavenging)
            {
                this.providerData.PercentOfQuotaUsedBeforeScavenging = percentOfQuotaUsedBeforeScavenging;
                this.providerData.PercentOfQuotaUsedAfterScavenging = percentOfQuotaUsedAfterScavenging;

                return this;
            }

            ISetupIsolatedStorageCacheNamedOptions ISetupIsolatedStorageCacheNamedOptions.WithMaxSizeInKilobytes(int maxSizeInKilobytes)
            {
                this.providerData.MaxSizeInKilobytes = maxSizeInKilobytes;

                return this;
            }

            ISetupIsolatedStorageCacheNamedOptions ISetupIsolatedStorageCacheNamedOptions.UsingSerializerOfType(Type serializerType)
            {
                if (serializerType == null) throw new ArgumentNullException("serializerType");

                if (!typeof(IIsolatedStorageCacheEntrySerializer).IsAssignableFrom(serializerType)
                    || serializerType.GetConstructor(Type.EmptyTypes) == null)
                    throw new ArgumentException(Resources.SerializerType_DerivedTypeNotCorrect);

                this.providerData.SerializerType = serializerType;

                return this;
            }
        }
    }
}
