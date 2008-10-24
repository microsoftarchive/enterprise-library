//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Security.Principal;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Tests
{
    [TestClass]
    public class CachedProfilesFixture
    {
        object profile;
        const string defaultInstance = "provider1";

        [TestInitialize]
        public void SetUp()
        {
            profile = "testprofile";
        }

        [TestCleanup]
        public void TearDown() {}

        [TestMethod]
        public void GetValidSecurityCache()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider(defaultInstance);

            Assert.IsNotNull(securityCache);
        }

        [TestMethod]
        public void SaveProfileWithDefaultExpiration()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider(defaultInstance);
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SaveProfile(profile);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);
        }

        [TestMethod]
        public void SaveProfileWithTokenFromPreviouslyCachedItem()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider(defaultInstance);
            Assert.IsNotNull(securityCache);

            IIdentity identity = new GenericIdentity("zman", "testauthtype");

            IToken token = securityCache.SaveIdentity(identity);
            Assert.IsNotNull(token);

            securityCache.SaveProfile(profile, token);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            object tmpProfile = securityCache.GetProfile(token);
            Assert.IsNotNull(tmpProfile);
            Assert.AreEqual(tmpProfile.ToString(), "testprofile");
        }

        [TestMethod]
        public void ExplicitlyExpireProfile()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider(defaultInstance);
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SaveProfile(profile);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            securityCache.ExpireProfile(token);

            object tmpProfile = securityCache.GetProfile(token);
            Assert.IsNull(tmpProfile);
        }

        [TestMethod]
        public void RetreiveCachedProfile()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider(defaultInstance);
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SaveProfile(profile);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            object cachedProfile = securityCache.GetProfile(token);
            Assert.IsNotNull(cachedProfile);
            Assert.AreEqual(cachedProfile.ToString(), "testprofile");
        }

        [TestMethod]
        public void RetreiveProfileNotInCache()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();
            Assert.IsNotNull(securityCache);

            IToken token = new GuidToken() as IToken;
            Assert.IsNotNull(token);

            object cachedProfile = securityCache.GetProfile(token);
            Assert.IsNull(cachedProfile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InMemorySaveWithNullProfileTestFixture()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();

            IToken token = securityCache.SaveProfile(null);
            if (token != null)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RetrieveCachedProfileFiresWmiEvent()
        {
            SecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider() as SecurityCacheProvider;
            SecurityCacheProviderInstrumentationListener listener = new SecurityCacheProviderInstrumentationListener("foo", false, false, true, "fooApplicationInstanceName");

            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(securityCache.GetInstrumentationEventProvider(), listener);

            using (WmiEventWatcher eventWatcher = new WmiEventWatcher(1))
            {
                IToken token = new GuidToken();
                object profile = securityCache.GetProfile(token);
                eventWatcher.WaitForEvents();

                Thread.Sleep(500);
                Assert.AreEqual(1, eventWatcher.EventsReceived.Count);
                Assert.AreEqual("SecurityCacheReadPerformedEvent", eventWatcher.EventsReceived[0].ClassPath.ClassName);
                Assert.AreEqual(SecurityEntityType.Profile.ToString(), eventWatcher.EventsReceived[0].Properties["EntityType"].Value);
                Assert.AreEqual("foo", eventWatcher.EventsReceived[0].Properties["InstanceName"].Value);
                Assert.AreEqual(token.Value, eventWatcher.EventsReceived[0].Properties["TokenUsed"].Value);
            }
        }
    }
}
