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

using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Tests.Configuration.Unity
{
	[TestClass]
	public class CachingStoreProviderInstantiationFixture
	{
		private IServiceLocator container;
        IIdentity identity;

		[TestInitialize]
		public void SetUp()
		{
            container = EnterpriseLibraryContainer.Current;
            identity = new GenericIdentity("zman", "testAuthType");
		}

		[TestCleanup]
		public void TearDown()
		{
			
		}

        [TestMethod]
		public void CreatedPoliciesAllowSaveIdentityWithTokenFromPreviousCachedItem()
		{
            Assert.IsNotNull(container.GetInstance<ISecurityCacheProvider>());

			string[] roles = new string[] { "admin", "manager" };
			IPrincipal principal = new GenericPrincipal(identity, roles);

            IToken token = container.GetInstance<ISecurityCacheProvider>().SavePrincipal(principal);
			Assert.IsNotNull(token);

            container.GetInstance<ISecurityCacheProvider>().SaveIdentity(identity, token);
			Assert.IsNotNull(token);
			Assert.IsNotNull(token.Value);

            IIdentity cachedIdentity = container.GetInstance<ISecurityCacheProvider>().GetIdentity(token);
			Assert.IsNotNull(cachedIdentity);
			Assert.AreEqual(cachedIdentity.Name, "zman");
		}
	}
}
