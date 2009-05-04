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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
    [TestClass]
    public class CacheManagerNodeFixture : ConfigurationDesignHost
    {
        DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

        protected override void InitializeCore()
        {
            SetDictionaryConfigurationSource(configurationSource);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInCacheManagerNodeThrows()
        {
            new CacheManagerNode(null);
        }

        [TestMethod]
        public void CacheManagerNodeTest()
        {
            int expirationPollFrequencyInSeconds = 30;
            int maximumElementsInCacheBeforeScavenging = 5;
            int numberToRemoveWhenScavenging = 8;
            CacheManagerNode node = new CacheManagerNode();

            node.ExpirationPollFrequencyInSeconds = expirationPollFrequencyInSeconds;
            Assert.AreEqual(expirationPollFrequencyInSeconds, node.ExpirationPollFrequencyInSeconds);

            node.MaximumElementsInCacheBeforeScavenging = maximumElementsInCacheBeforeScavenging;
            Assert.AreEqual(maximumElementsInCacheBeforeScavenging, node.MaximumElementsInCacheBeforeScavenging);

            node.NumberToRemoveWhenScavenging = numberToRemoveWhenScavenging;
            Assert.AreEqual(numberToRemoveWhenScavenging, node.NumberToRemoveWhenScavenging);

            CacheManagerData data = (CacheManagerData)node.CacheManagerData;
            Assert.AreEqual("Cache Manager", data.Name);
            Assert.AreEqual(expirationPollFrequencyInSeconds, data.ExpirationPollFrequencyInSeconds);
            Assert.AreEqual(maximumElementsInCacheBeforeScavenging, data.MaximumElementsInCacheBeforeScavenging);
            Assert.AreEqual(numberToRemoveWhenScavenging, data.NumberToRemoveWhenScavenging);
        }

        [TestMethod]
        public void EnsureCacheManagerPropertyCatoriesAndDescriptions()
        {
            Assert.IsTrue(SRAttributesHelper.AssertSRDescription(typeof(CacheManagerNode), "ExpirationPollFrequencyInSeconds", Resources.ExpirationPollFrequencyInSecondsDescription));
            Assert.IsTrue(SRAttributesHelper.AssertSRCategory(typeof(CacheManagerNode), "ExpirationPollFrequencyInSeconds"));

            Assert.IsTrue(SRAttributesHelper.AssertSRDescription(typeof(CacheManagerNode), "MaximumElementsInCacheBeforeScavenging", Resources.MaximumElementsInCacheBeforeScavengingDescription));
            Assert.IsTrue(SRAttributesHelper.AssertSRCategory(typeof(CacheManagerNode), "MaximumElementsInCacheBeforeScavenging"));

            Assert.IsTrue(SRAttributesHelper.AssertSRDescription(typeof(CacheManagerNode), "NumberToRemoveWhenScavenging", Resources.NumberToRemoveWhenScavengingDescription));
            Assert.IsTrue(SRAttributesHelper.AssertSRCategory(typeof(CacheManagerNode), "NumberToRemoveWhenScavenging"));
        }
    }
}
