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

using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests.Configuration.Unity
{
	[TestClass]
	public class SymmetricAlgorithmProviderPolicyCreationFixture
	{
		private IUnityContainer container;
		const string symmetricAlgorithm1 = "symmetricAlgorithm1";
		const string symmetricKeyFile = "ProtectedKey.file";
		const string plainTextString = "secret";

		[TestInitialize]
		public void SetUp()
		{
			container = new UnityContainer();
			container.AddExtension(new EnterpriseLibraryCoreExtension());
			CryptographerFixture.CreateKeyFile(symmetricKeyFile);
		}

		[TestCleanup]
		public void TearDown()
		{
			File.Delete(symmetricKeyFile);
			container.Dispose();
		}

		[TestMethod]
		public void CanCreatePoliciesTo_EncryptAndDecryptStringWithASymmetricAlgorithm()
		{
			Assert.IsInstanceOfType(container.Resolve<ISymmetricCryptoProvider>(symmetricAlgorithm1),
				typeof(SymmetricAlgorithmProvider));

			byte[] megabyte = new byte[1024 * 1024];
			CryptographyUtility.GetRandomBytes(megabyte);

			byte[] encrypted = container.Resolve<ISymmetricCryptoProvider>(symmetricAlgorithm1).Encrypt(megabyte);
			Assert.IsFalse(CryptographyUtility.CompareBytes(megabyte, encrypted));

			byte[] decrypted = container.Resolve<ISymmetricCryptoProvider>(symmetricAlgorithm1).Decrypt(encrypted);
			Assert.IsTrue(CryptographyUtility.CompareBytes(megabyte, decrypted));
		}
	}
}
