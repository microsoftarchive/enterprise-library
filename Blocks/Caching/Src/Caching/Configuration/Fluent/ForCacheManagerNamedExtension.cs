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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="ICachingConfiguration"/> extensions to support configuring <see cref="CacheManager"/> intances.
    /// </summary>
    public static class ForCacheManagerNamedExtension
    {
        /// <summary>
        /// Adds a new <see cref="CacheManager"/> to the caching configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="cacheManagerName">The name of the <see cref="CacheManager"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CacheManagerData"/>. </returns>
        /// <seealso cref="CacheManager"/>
        /// <seealso cref="CacheManagerData"/>
        public static ICachingConfigurationCacheManager ForCacheManagerNamed(this ICachingConfiguration context, string cacheManagerName)
        {
            if (string.IsNullOrEmpty(cacheManagerName)) 
                throw new ArgumentException(Resources.EmptyParameterName, "cacheManagerName");

            return new ForCacheManagerNamedBuilder(context, cacheManagerName);
        }

        private class ForCacheManagerNamedBuilder : CacheManagerSettingsExtension, ICachingConfigurationCacheManager, ICachingConfigurationCacheManagerOptions, ICachingConfigurationCacheManagerExtension
        {
            CacheManagerData cacheManagerData;
            public ForCacheManagerNamedBuilder(ICachingConfiguration context, string cacheManagerName)
                :base(context)
            {
                cacheManagerData = new CacheManagerData
                {
                    Name = cacheManagerName
                };

                base.CachingSettings.CacheManagers.Add(cacheManagerData);
            }

            public ICachingConfigurationCacheManager UseAsDefaultCache()
            {
                base.CachingSettings.DefaultCacheManager = cacheManagerData.Name;
                
                return this;
            }

            public ICachingConfigurationCacheManagerOptions PollWhetherItemsAreExpiredIntervalSeconds(int pollExperitionSeconds)
            {
                cacheManagerData.ExpirationPollFrequencyInSeconds = pollExperitionSeconds;

                return this;
            }

            public ICachingConfigurationCacheManagerOptions StartScavengingAfterItemCount(int numberOfElementsBeforeScavenging)
            {
                cacheManagerData.MaximumElementsInCacheBeforeScavenging = numberOfElementsBeforeScavenging;

                return this;
            }

            public ICachingConfigurationCacheManagerOptions WhenScavengingRemoveItemCount(int numberOfElementsToRemoveBeforeScavenging)
            {
                cacheManagerData.NumberToRemoveWhenScavenging = numberOfElementsToRemoveBeforeScavenging;

                return this;
            }


            public ICachingConfiguration StoreInSharedBackingStore(string backingStoreName)
            {
                cacheManagerData.CacheStorage = backingStoreName;

                return this;
            }

            public ICachingConfiguration StoreInMemory()
            {
                var nullBackingStore = CachingSettings.BackingStores.Where(x => x.Type == typeof(NullBackingStore)).FirstOrDefault();
                if (nullBackingStore == null)
                {
                    nullBackingStore = new CacheStorageData("Null Backing Store", typeof(NullBackingStore));
                    CachingSettings.BackingStores.Add(nullBackingStore);
                }

                return StoreInSharedBackingStore(nullBackingStore.Name);
            }

            CacheManagerData ICachingConfigurationCacheManagerExtension.CacheManager
            {
                get { return cacheManagerData; }
            }

            CacheManagerSettings ICachingConfigurationCacheManagerExtension.CachingSettings
            {
                get { return base.CachingSettings; }
            }


            public ICachingConfigurationCacheManagerOptions WithOptions
            {
                get { return this; }
            }

            
        }
    }
}
