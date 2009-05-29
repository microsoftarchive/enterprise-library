//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests.Configuration.Unity
{
	[TestClass]
	public class DpapiSymmetricCryptoProviderPolicyCreationFixture
	{
		const string symmInstance = "dpapiSymmetric1";

		private IUnityContainer container;

		[TestInitialize]
		public void SetUp()
		{
			container = new UnityContainer();
			container.AddExtension(new EnterpriseLibraryCoreExtension());
		}

		[TestCleanup]
		public void TearDown()
		{
			container.Dispose();
		}

		[TestMethod]
		public void CanCreatePoliciesTo_EncryptAndDecryptOneMegabyte()
		{
			Assert.IsInstanceOfType(container.Resolve<ISymmetricCryptoProvider>(symmInstance),
				typeof(DpapiSymmetricCryptoProvider));

			byte[] megabyte = new byte[1024 * 1024];
			CryptographyUtility.GetRandomBytes(megabyte);

			byte[] encrypted = container.Resolve<ISymmetricCryptoProvider>(symmInstance).Encrypt(megabyte);
			Assert.IsFalse(CryptographyUtility.CompareBytes(megabyte, encrypted));

			byte[] decrypted = container.Resolve<ISymmetricCryptoProvider>(symmInstance).Decrypt(encrypted);
			Assert.IsTrue(CryptographyUtility.CompareBytes(megabyte, decrypted));
		}
	}
}
