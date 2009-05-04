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

using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
	static class TestConfigurationSource
	{
		public static DictionaryConfigurationSource GenerateConfiguration()
		{
			DictionaryConfigurationSource sections = new DictionaryConfigurationSource();
			sections.Add(CacheManagerSettings.SectionName, GenerateCacheManagerSettings());
			return sections;
		}

		public static DictionaryConfigurationSource GenerateConfigurationWithInstrumentation()
		{
			DictionaryConfigurationSource sections = GenerateConfiguration();
			sections.Add(InstrumentationConfigurationSection.SectionName, GenerateInstrumentationSettings());
			return sections;
		}

		private static CacheManagerSettings GenerateCacheManagerSettings()
		{
			CacheManagerSettings settings = new CacheManagerSettings();
			settings.DefaultCacheManager = "ShortInMemoryPersistence";

			settings.BackingStores.Add(new CacheStorageData("inMemory", typeof(NullBackingStore)));
			settings.BackingStores.Add(new IsolatedStorageCacheStorageData("inIsolatedStorage", "", "EntLib"));
			settings.BackingStores.Add(new CacheStorageData("inMemoryWithNullEncryptor", typeof(NullBackingStore), "nullEncryptor"));
			settings.BackingStores.Add(new IsolatedStorageCacheStorageData("inIsolatedStorageWithNullEncryptor", "nullEncryptor", "EntLib"));
			settings.BackingStores.Add(new IsolatedStorageCacheStorageData("inMemoryWithBadStorage", "EntLib", "badname"));

			settings.EncryptionProviders.Add(new MockStorageEncryptionProviderData("nullEncryptor"));

			settings.CacheManagers.Add(new CacheManagerData("InMemoryPersistence",
				60, 100, 10, "inMemory"));
			settings.CacheManagers.Add(new CacheManagerData("InIsoStorePersistence",
				1, 100, 2, "inIsolatedStorage"));
			settings.CacheManagers.Add(new CacheManagerData("SmallInMemoryPersistence",
				1, 3, 2, "inMemory"));
			settings.CacheManagers.Add(new CacheManagerData("ShortInMemoryPersistence",
				1, 10, 2, "inMemory"));
			settings.CacheManagers.Add(new CacheManagerData("CacheManagerWithBadCacheStorageInstance",
				60, 100, 10, ""));
			//settings.CacheManagers.Add(new CacheManagerData("CacheManagerWithBadStoreType",
			//    60, 100, 10, "storageWithBadType"));
			settings.CacheManagers.Add(new CacheManagerData("InMemoryPersistenceWithNullEncryption",
				60, 100, 10, "inMemoryWithNullEncryptor"));
			settings.CacheManagers.Add(new CacheManagerData("InIsoStorePersistenceWithNullEncryption",
				1, 100, 2, "inIsolatedStorageWithNullEncryptor"));
			settings.CacheManagers.Add(new CacheManagerData("InIsoStorePersistenceWithNullEncryption2",
				1, 100, 2, "inIsolatedStorageWithNullEncryptor"));
			settings.CacheManagers.Add(new CacheManagerData("test", 10, 10, 1, "foo"));
			settings.CacheManagers.Add(new CacheManagerData("BadStorageProvider", 10, 10, 1, "inMemoryWithBadStorage"));
			settings.CacheManagers.Add(new CacheManagerData("BadBackingStore", 10, 10, 1, "foo"));
			settings.CacheManagers.Add(new MockCacheManagerData("MockManager", "FooValue"));
			return settings;
		}

		private static InstrumentationConfigurationSection GenerateInstrumentationSettings()
		{
            InstrumentationConfigurationSection settings = new InstrumentationConfigurationSection(true, true, true, "fooApplicationName");

			return settings;
		}
	}
}
