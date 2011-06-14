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

using System.Windows.Markup;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration
{
    [TestClass]
    public class SerializationFixture
    {
        [TestMethod]
        public void CanDeserializeInMemoryCacheData()
        {
            var xaml = @"<el:InMemoryCacheData Name='memory' MaxItemsBeforeScavenging='10' ItemsLeftAfterScavenging='5' ExpirationPollingInterval='0:0:5' xmlns:el='http://schemas.microsoft.com/practices/2011/entlib'/>";

            var data = (InMemoryCacheData)XamlReader.Load(xaml);

            Assert.AreEqual("memory", data.Name);
            Assert.AreEqual(10, data.MaxItemsBeforeScavenging);
            Assert.AreEqual(5, data.ItemsLeftAfterScavenging);
            Assert.AreEqual(TimeSpan.FromSeconds(5), data.ExpirationPollingInterval);
        }

        [TestMethod]
        public void CanDeserializeIsolatedStorageCacheData()
        {
            var xaml = @"<el:IsolatedStorageCacheData Name='isolated' MaxSizeInKilobytes='25' PercentOfQuotaUsedBeforeScavenging='50' PercentOfQuotaUsedAfterScavenging='10' ExpirationPollingInterval='0:0:5' xmlns:el='http://schemas.microsoft.com/practices/2011/entlib'/>";

            var data = (IsolatedStorageCacheData)XamlReader.Load(xaml);

            Assert.AreEqual("isolated", data.Name);
            Assert.AreEqual(25, data.MaxSizeInKilobytes);
            Assert.AreEqual(50, data.PercentOfQuotaUsedBeforeScavenging);
            Assert.AreEqual(10, data.PercentOfQuotaUsedAfterScavenging);
            Assert.AreEqual(TimeSpan.FromSeconds(5), data.ExpirationPollingInterval);
        }
    }
}
