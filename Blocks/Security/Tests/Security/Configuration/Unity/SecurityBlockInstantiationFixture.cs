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
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests.Configuration.Unity
{
	[TestClass]
    public class SecurityBlockInstantiationFixture
	{
		[TestMethod]
		public void CanCreateAuthorizationRuleProvider()
		{
			IAuthorizationProvider createdObject = EnterpriseLibraryContainer.Current.GetInstance<IAuthorizationProvider>("RuleProvider");

			Assert.IsNotNull(createdObject);
			Assert.IsInstanceOfType(createdObject, typeof (AuthorizationRuleProvider));
			Assert.IsTrue(
				createdObject.Authorize(
					new GenericPrincipal(new GenericIdentity("TestUser"), new string[] {"Admin"}),
					"rule1"));
		}

		[TestMethod]
		public void CanCreateCustomAuthorizationProvider()
		{
            IAuthorizationProvider createdObject = EnterpriseLibraryContainer.Current.GetInstance<IAuthorizationProvider>("custom");

			Assert.IsNotNull(createdObject);
			Assert.IsInstanceOfType(createdObject, typeof (MockCustomAuthorizationProvider));
			Assert.AreEqual("value1", ((MockCustomAuthorizationProvider) createdObject).customValue);
		}

		[TestMethod]
		public void CanCreateDefaultAuthorizationProvider()
		{
            IAuthorizationProvider createdObject = EnterpriseLibraryContainer.Current.GetInstance<IAuthorizationProvider>();

			Assert.IsNotNull(createdObject);
			Assert.IsInstanceOfType(createdObject, typeof (MockAuthorizationProvider));
		}

		[TestMethod]
		public void CanCreateCustomSecurityCacheProvider()
		{
            ISecurityCacheProvider createdObject = EnterpriseLibraryContainer.Current.GetInstance<ISecurityCacheProvider>("custom");

			Assert.IsNotNull(createdObject);
			Assert.IsInstanceOfType(createdObject, typeof(MockCustomSecurityCacheProvider));
			Assert.AreEqual("value1", ((MockCustomSecurityCacheProvider)createdObject).customValue);
		}

		[TestMethod]
		public void CanCreateDefaultSecurityCacheProvider()
		{
            ISecurityCacheProvider createdObject = EnterpriseLibraryContainer.Current.GetInstance<ISecurityCacheProvider>();

			Assert.IsNotNull(createdObject);
			Assert.IsInstanceOfType(createdObject, typeof(MockSecurityCacheProvider));
		}

        [TestMethod]
        public void CanCreateDefaultSecurityEventLogger()
        {
            DefaultSecurityEventLogger createdObject = EnterpriseLibraryContainer.Current.GetInstance<DefaultSecurityEventLogger>();

            Assert.IsNotNull(createdObject);
        }
	}
}
