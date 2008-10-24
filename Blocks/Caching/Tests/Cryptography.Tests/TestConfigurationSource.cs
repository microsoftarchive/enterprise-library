//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Tests
{
	static class TestConfigurationSource
	{
		public static DictionaryConfigurationSource GenerateConfiguration()
		{
			DictionaryConfigurationSource sections = new DictionaryConfigurationSource();
			sections.Add(CacheManagerSettings.SectionName, GenerateCacheManagerSettings());
			return sections;
		}

		private static CryptographySettings GenerateCryptographySettings()
		{
			CryptographySettings settings = new CryptographySettings();
			settings.SymmetricCryptoProviders.Add(new DpapiSymmetricCryptoProviderData("dpapi1", DataProtectionScope.CurrentUser));
			return settings;
		}

		private static CacheManagerSettings GenerateCacheManagerSettings()
		{
			CacheManagerSettings settings = new CacheManagerSettings();
			settings.BackingStores.Add(new CacheStorageData("inMemoryWithEncryptor", typeof(NullBackingStore), "dpapiEncryptor"));
			settings.BackingStores.Add(new CacheStorageData("inMemoryWithNullEncryptor", typeof(NullBackingStore), "nullEncryptor"));
			
			settings.EncryptionProviders.Add(new SymmetricStorageEncryptionProviderData("dpapiEncryptor", "dpapi1"));
			settings.EncryptionProviders.Add(new MockStorageEncryptionProviderData("nullEncryptor"));

			settings.CacheManagers.Add(new CacheManagerData("InMemoryPersistenceWithNullEncryption",
				60, 100, 10, "inMemoryWithNullEncryptor"));
			settings.CacheManagers.Add(new CacheManagerData("InMemoryPersistenceWithSymmetricEncryption",
				60, 100, 10, "inMemoryWithEncryptor"));
			return settings;
		}
	}
}
