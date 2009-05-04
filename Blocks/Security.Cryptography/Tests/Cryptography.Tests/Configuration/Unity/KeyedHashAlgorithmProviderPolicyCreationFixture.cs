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
	public class KeyedHashAlgorithmProviderPolicyCreationFixture
	{
		const string hashInstance = "hmac1";
		const string keyedHashKeyFile = "KeyedHashKey.file";
		static readonly byte[] plainTextBytes = new byte[] { 0, 1, 2, 3 };

		private IUnityContainer container;

		[TestInitialize]
		public void SetUp()
		{
			container = new UnityContainer();
			container.AddExtension(new EnterpriseLibraryCoreExtension());
			CryptographerFixture.CreateKeyFile(keyedHashKeyFile);
		}

		[TestCleanup]
		public void TearDown()
		{
			File.Delete(keyedHashKeyFile);
			container.Dispose();
		}

		[TestMethod]
		public void CanCreatePoliciesTo_CreateAndCompareHashBytes()
		{
			container.AddExtension(new CryptographyBlockExtension());
			Assert.IsInstanceOfType(container.Resolve<IHashProvider>(hashInstance), typeof(KeyedHashAlgorithmProvider));

			byte[] hash = container.Resolve<IHashProvider>(hashInstance).CreateHash(plainTextBytes);
			bool result = container.Resolve<IHashProvider>(hashInstance).CompareHash(plainTextBytes, hash);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void CanCreatePoliciesTo_CreateAndCompareInvalidHashBytes()
		{
			container.AddExtension(new CryptographyBlockExtension());
			Assert.IsInstanceOfType(container.Resolve<IHashProvider>(hashInstance), typeof(KeyedHashAlgorithmProvider));

			byte[] hash = container.Resolve<IHashProvider>(hashInstance).CreateHash(plainTextBytes);

			byte[] badPlainText = new byte[] { 2, 1, 0 };
			bool result = container.Resolve<IHashProvider>(hashInstance).CompareHash(badPlainText, hash);

			Assert.IsFalse(result);
		}
	}
}
