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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Tests.Configuration.Unity
{
	[TestClass]
	public class CachingStoreProviderPolicyCreatorFixture
	{
		private IUnityContainer container;
        IIdentity identity;

		[TestInitialize]
		public void SetUp()
		{
			container = new UnityContainer();
            identity = new GenericIdentity("zman", "testAuthType");
		}

		[TestCleanup]
		public void TearDown()
		{
			container.Dispose();
		}

		[TestMethod]
		public void CreatedPoliciesAllowSaveIdentityWithTokenFromPreviousCachedItem()
		{
			container.AddExtension(new EnterpriseLibraryCoreExtension());
			container.AddExtension(new SecurityBlockExtension());
			container.AddExtension(new CachingBlockExtension());

			Assert.IsNotNull(container.Resolve<ISecurityCacheProvider>());

			string[] roles = new string[] { "admin", "manager" };
			IPrincipal principal = new GenericPrincipal(identity, roles);

			IToken token = container.Resolve<ISecurityCacheProvider>().SavePrincipal(principal);
			Assert.IsNotNull(token);

			container.Resolve<ISecurityCacheProvider>().SaveIdentity(identity, token);
			Assert.IsNotNull(token);
			Assert.IsNotNull(token.Value);

			IIdentity cachedIdentity = container.Resolve<ISecurityCacheProvider>().GetIdentity(token);
			Assert.IsNotNull(cachedIdentity);
			Assert.AreEqual(cachedIdentity.Name, "zman");
		}
	}
}
