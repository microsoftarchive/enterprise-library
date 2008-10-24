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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Tests
{
	static class TestConfigurationSource
	{
		public static DictionaryConfigurationSource GenerateConfiguration()
		{
			DictionaryConfigurationSource sections = new DictionaryConfigurationSource();
			sections.Add(DatabaseSettings.SectionName, GenerateDatabaseSettings());
			sections.Add(CacheManagerSettings.SectionName, GenerateCacheManagerSettings());
			return sections;
		}

		private static DatabaseSettings GenerateDatabaseSettings()
		{
			DatabaseSettings settings = new DatabaseSettings();
			return settings;
		}

		private static CacheManagerSettings GenerateCacheManagerSettings()
		{
			CacheManagerSettings settings = new CacheManagerSettings();
			settings.BackingStores.Add(new DataCacheStorageData("Data Cache Storage", "CachingDatabase", "Partition1"));
			settings.CacheManagers.Add(new CacheManagerData("InDatabasePersistence",
				1, 100, 100, "Data Cache Storage"));
			settings.CacheManagers.Add(new CacheManagerData("SecondInDatabasePersistence",
				1, 100, 100, "Data Cache Storage"));
			return settings;
		}
	}
}
