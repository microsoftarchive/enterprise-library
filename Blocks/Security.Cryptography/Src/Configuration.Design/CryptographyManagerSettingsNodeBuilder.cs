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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
	class CryptographyManagerSettingsNodeBuilder : NodeBuilder
	{
		private CryptographySettings cryptographySettings;
		private HashProviderNode defaultHashProviderNode;
		private SymmetricCryptoProviderNode defaultSymmetricProviderNode;

		public CryptographyManagerSettingsNodeBuilder(IServiceProvider serviceProvider, CryptographySettings cryptographySettings)
			: base(serviceProvider)
		{
			this.cryptographySettings = cryptographySettings;
		}

		public CryptographySettingsNode Build()
		{
			CryptographySettingsNode rootNode = new CryptographySettingsNode();

			HashProviderCollectionNode hashProviderCollectionNode = new HashProviderCollectionNode();
			foreach (HashProviderData hashProviderData in cryptographySettings.HashProviders)
			{
				CreateHashProviderNode(hashProviderCollectionNode, hashProviderData);
			}

			SymmetricCryptoProviderCollectionNode symmetricCryptoProviderCollectionNode = new SymmetricCryptoProviderCollectionNode();
			foreach (SymmetricProviderData symmetricCryptoProviderData in cryptographySettings.SymmetricCryptoProviders)
			{
				CreateSymmetricCryptoProviderNode(symmetricCryptoProviderCollectionNode, symmetricCryptoProviderData);
			}

			rootNode.AddNode(hashProviderCollectionNode);
			rootNode.AddNode(symmetricCryptoProviderCollectionNode);

			rootNode.DefaultHashProvider = defaultHashProviderNode;
			rootNode.DefaultSymmetricCryptoProvider = defaultSymmetricProviderNode;

			rootNode.RequirePermission = cryptographySettings.SectionInformation.RequirePermission;

			return rootNode;
		}

		private void CreateSymmetricCryptoProviderNode(SymmetricCryptoProviderCollectionNode symmetricCryptoProviderCollectionNode, object symmetricCryptoProviderData)
		{
			SymmetricCryptoProviderNode symmetricAlgorithmProviderNode = NodeCreationService.CreateNodeByDataType(symmetricCryptoProviderData.GetType(), new object[] { symmetricCryptoProviderData }) as SymmetricCryptoProviderNode;
			if (null == symmetricAlgorithmProviderNode)
			{
				LogNodeMapError(symmetricCryptoProviderCollectionNode, symmetricCryptoProviderData.GetType());
				return;
			}

			if (string.Compare(symmetricAlgorithmProviderNode.Name, cryptographySettings.DefaultSymmetricCryptoProviderName) == 0)
			{
				defaultSymmetricProviderNode = symmetricAlgorithmProviderNode;
			}

			symmetricCryptoProviderCollectionNode.AddNode(symmetricAlgorithmProviderNode);
		}

		private void CreateHashProviderNode(HashProviderCollectionNode hashProviderCollectionNode, HashProviderData hashProviderData)
		{
			HashProviderNode hashProviderNode = NodeCreationService.CreateNodeByDataType(hashProviderData.GetType(), new object[] { hashProviderData }) as HashProviderNode;
			if (null == hashProviderNode)
			{
				LogNodeMapError(hashProviderCollectionNode, hashProviderData.GetType());
				return;
			}

			if (string.Compare(hashProviderNode.Name, cryptographySettings.DefaultHashProviderName) == 0)
			{
				defaultHashProviderNode = hashProviderNode;
			}

			hashProviderCollectionNode.AddNode(hashProviderNode);
		}

		private static bool AlgorithmHasEncryptionKey(ConfigurationNode algorithmNode)
		{
			return algorithmNode is ICryptographicKeyConfigurationNode;
		}
	}
}
