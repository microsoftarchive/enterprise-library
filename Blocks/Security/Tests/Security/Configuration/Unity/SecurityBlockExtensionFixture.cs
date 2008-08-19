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
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests.Configuration.Unity
{
	[TestClass]
	public class SecurityBlockExtensionFixture
	{
		[TestMethod]
		public void CanCreateAuthorizationRuleProvider()
		{
			IUnityContainer container = new UnityContainer();
			container.AddNewExtension<EnterpriseLibraryCoreExtension>();
			container.AddNewExtension<SecurityBlockExtension>();

			IAuthorizationProvider createdObject = container.Resolve<IAuthorizationProvider>("RuleProvider");

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
			IUnityContainer container = new UnityContainer();
			container.AddNewExtension<EnterpriseLibraryCoreExtension>();
			container.AddNewExtension<SecurityBlockExtension>();

			IAuthorizationProvider createdObject = container.Resolve<IAuthorizationProvider>("custom");

			Assert.IsNotNull(createdObject);
			Assert.IsInstanceOfType(createdObject, typeof (MockCustomAuthorizationProvider));
			Assert.AreEqual("value1", ((MockCustomAuthorizationProvider) createdObject).customValue);
		}

		[TestMethod]
		public void CanCreateDefaultAuthorizationProvider()
		{
			IUnityContainer container = new UnityContainer();
			container.AddNewExtension<EnterpriseLibraryCoreExtension>();
			container.AddNewExtension<SecurityBlockExtension>();

			IAuthorizationProvider createdObject = container.Resolve<IAuthorizationProvider>();

			Assert.IsNotNull(createdObject);
			Assert.IsInstanceOfType(createdObject, typeof (MockAuthorizationProvider));
		}

		[TestMethod]
		public void CanCreateCustomSecurityCacheProvider()
		{
			IUnityContainer container = new UnityContainer();
			container.AddNewExtension<EnterpriseLibraryCoreExtension>();
			container.AddNewExtension<SecurityBlockExtension>();

			ISecurityCacheProvider createdObject = container.Resolve<ISecurityCacheProvider>("custom");

			Assert.IsNotNull(createdObject);
			Assert.IsInstanceOfType(createdObject, typeof(MockCustomSecurityCacheProvider));
			Assert.AreEqual("value1", ((MockCustomSecurityCacheProvider)createdObject).customValue);
		}

		[TestMethod]
		public void CanCreateDefaultSecurityCacheProvider()
		{
			IUnityContainer container = new UnityContainer();
			container.AddNewExtension<EnterpriseLibraryCoreExtension>();
			container.AddNewExtension<SecurityBlockExtension>();

			ISecurityCacheProvider createdObject = container.Resolve<ISecurityCacheProvider>();

			Assert.IsNotNull(createdObject);
			Assert.IsInstanceOfType(createdObject, typeof(MockSecurityCacheProvider));
		}
	}
}