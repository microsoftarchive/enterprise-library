//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.BlockSpecific.Caching
{
    [TestClass]
    public class DefaultConfigurationFixture
    {
        [TestMethod]
        public void InMemoryCacheDataHasProperDefaultValues()
        {
            var data = new InMemoryCacheData();
            Assert.AreEqual(typeof(InMemoryCache), data.Type);
            Assert.AreEqual(TimeSpan.FromMinutes(2), data.ExpirationPollingInterval);
            Assert.AreEqual(80, data.ItemsLeftAfterScavenging);
            Assert.AreEqual(200, data.MaxItemsBeforeScavenging);
        }

        [TestMethod]
        public void IsolatedStorageCacheDataHasProperDefaultValues()
        {
            var data = new IsolatedStorageCacheData();
            Assert.AreEqual(typeof(IsolatedStorageCache), data.Type);
            Assert.AreEqual(TimeSpan.FromMinutes(2), data.ExpirationPollingInterval);
            Assert.AreEqual(60, data.PercentOfQuotaUsedAfterScavenging);
            Assert.AreEqual(80, data.PercentOfQuotaUsedBeforeScavenging);
        }
    }
}
