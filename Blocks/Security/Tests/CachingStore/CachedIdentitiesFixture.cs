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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Tests
{
    [TestClass]
    public class CachedIdentitiesFixture
    {
        IIdentity identity;

        [TestInitialize]
        public void SetUp()
        {
            identity = new GenericIdentity("zman", "testAuthType");
        }

        [TestCleanup]
        public void TearDown() {}

        [TestMethod]
        public void GetValidSecurityCache()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();

            Assert.IsNotNull(securityCache);
        }

        [TestMethod]
        public void SaveIdentityWithDefaultExpiration()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SaveIdentity(identity);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);
        }

        [TestMethod]
        public void SaveIdentityWithTokenFromPreviousCachedItem()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();
            Assert.IsNotNull(securityCache);

            string[] roles = new string[] { "admin", "manager" };
            IPrincipal principal = new GenericPrincipal(identity, roles);

            IToken token = securityCache.SavePrincipal(principal);
            Assert.IsNotNull(token);

            securityCache.SaveIdentity(identity, token);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            IIdentity cachedIdentity = securityCache.GetIdentity(token);
            Assert.IsNotNull(cachedIdentity);
            Assert.AreEqual(cachedIdentity.Name, "zman");
        }

        [TestMethod]
        public void ExplicitlyExpireIdentity()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SaveIdentity(identity);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            securityCache.ExpireIdentity(token);

            IIdentity cachedIdentity = securityCache.GetIdentity(token);
            Assert.IsNull(cachedIdentity);
        }

        [TestMethod]
        public void RetreiveCachedIdentity()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SaveIdentity(identity);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            IIdentity cachedIdentity = securityCache.GetIdentity(token);
            Assert.IsNotNull(cachedIdentity);
            Assert.AreEqual(cachedIdentity.Name, "zman");
        }

        [TestMethod]
        public void RetreiveIdentityNotInCache()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();
            Assert.IsNotNull(securityCache);

            IToken token = new GuidToken() as IToken;
            Assert.IsNotNull(token);

            IIdentity cachedIdentity = securityCache.GetIdentity(token);
            Assert.IsNull(cachedIdentity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InMemorySaveWithNullIdentityTestFixture()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();
            IToken token = securityCache.SaveIdentity(null);
            if (token != null)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RetrieveCachedIdentityFiresWmiEvent()
        {
            SecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider() as SecurityCacheProvider;
            SecurityCacheProviderInstrumentationListener listener = new SecurityCacheProviderInstrumentationListener("foo", false, false, true, "fooApplicationInstanceName");

            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(securityCache.GetInstrumentationEventProvider(), listener);

            using (WmiEventWatcher eventWatcher = new WmiEventWatcher(1))
            {
                IToken token = new GuidToken();
                object profile = securityCache.GetIdentity(token);
                eventWatcher.WaitForEvents();

                Assert.AreEqual(1, eventWatcher.EventsReceived.Count);
                Assert.AreEqual("SecurityCacheReadPerformedEvent", eventWatcher.EventsReceived[0].ClassPath.ClassName);
                Assert.AreEqual(SecurityEntityType.Identity.ToString(), eventWatcher.EventsReceived[0].Properties["EntityType"].Value);
                Assert.AreEqual("foo", eventWatcher.EventsReceived[0].Properties["InstanceName"].Value);
                Assert.AreEqual(token.Value, eventWatcher.EventsReceived[0].Properties["TokenUsed"].Value);
            }
        }
    }
}
