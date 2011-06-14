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
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// Configuration object for an <see cref="InMemoryCache"/>.
    /// </summary>
    public class InMemoryCacheData : CacheData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCacheData"/> class.
        /// </summary>
        public InMemoryCacheData()
        {
            this.MaxItemsBeforeScavenging = 200;
            this.ItemsLeftAfterScavenging = 80;

            this.ExpirationPollingInterval = TimeSpan.FromMinutes(2);
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

        /// <summary>
        /// Gets or sets the maximum number of items in cache before an add causes scavenging to take place.
        /// </summary>
        public int MaxItemsBeforeScavenging { get; set; }

        /// <summary>
        /// Gets or sets the number of items left in the cache after scavenging has taken place.
        /// </summary>
        public int ItemsLeftAfterScavenging { get; set; }

        /// <summary>
        /// Gets or sets the frequency of expiration polling cycle.
        /// </summary>
        public TimeSpan ExpirationPollingInterval { get; set; }
    }
}
