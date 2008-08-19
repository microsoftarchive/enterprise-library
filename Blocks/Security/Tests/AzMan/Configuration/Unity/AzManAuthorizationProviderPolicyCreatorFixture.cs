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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Tests.Configuration.Unity
{
	[TestClass]
	public class AzManAuthorizationProviderPolicyCreatorFixture
	{
		[TestMethod]
		public void CanCreateAzManAuthorizationProvider()
		{
			IUnityContainer container = new UnityContainer();
			container.AddNewExtension<EnterpriseLibraryCoreExtension>();
			container.AddNewExtension<SecurityBlockExtension>();

			AzManAuthorizationProvider createdObject =
				(AzManAuthorizationProvider)container.Resolve<IAuthorizationProvider>("DefaultAzManProvider");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual(@"Enterprise Library Unit Test", createdObject.ApplicationName);
			Assert.AreEqual("myAuditId", createdObject.AuditIdentifierPrefix);
			Assert.AreEqual("", createdObject.ScopeName);
			Assert.AreEqual(AzManAuthorizationProvider.GetStoreLocationPath(@"msxml://{currentPath}/testAzStore.xml"),
							createdObject.StoreLocation);
		}
	}
}