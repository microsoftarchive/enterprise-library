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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage.given_an_iso_storage_with_data
{
    public class Context : ArrangeActAssert
    {
        public class NonDeserializable
        {
            public NonDeserializable(string property)
            {
                this.Property = property;
            }

            public string Property { get; set; }
        }

        protected ObjectCache Cache;
        protected const string CacheName = "initialized_cache";

        protected virtual int MaxSize
        {
            get { return 64; }
        }
        protected const int QuotaUsedBeforeScavenging = 80;
        protected const int QuotaUsedAfterScavenging = 80;
        protected TimeSpan PollingInterval = TimeSpan.FromMinutes(1);

        protected override void Arrange()
        {
            base.Arrange();

            IsolatedStorageCache.DeleteCache(CacheName);
            this.RefreshCache();

            Cache.Add("key", "value", new CacheItemPolicy
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });

            Cache["largeData"] = Enumerable.Range(0, 5000).ToList();

            Cache["notDeserializable"] = new NonDeserializable("value");

            this.RefreshCache();
        }

        protected virtual void RefreshCache()
        {
            using (Cache as IDisposable) { }
            Cache = new IsolatedStorageCache(CacheName, MaxSize, QuotaUsedBeforeScavenging, QuotaUsedAfterScavenging, PollingInterval, new IsolatedStorageCacheEntrySerializer());
        }

        protected override void Teardown()
        {
            base.Teardown();

            using (Cache as IDisposable) { }
            Cache = null;
            IsolatedStorageCache.DeleteCache(CacheName);
        }
    }
}
