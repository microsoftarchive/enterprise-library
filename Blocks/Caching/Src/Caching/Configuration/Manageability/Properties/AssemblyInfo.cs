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

using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

[assembly : ConfigurationSectionManageabilityProvider(CacheManagerSettings.SectionName, typeof(CacheManagerSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(CacheStorageDataManageabilityProvider), typeof(CacheStorageData), typeof(CacheManagerSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(CustomCacheStorageDataManageabilityProvider), typeof(CustomCacheStorageData), typeof(CacheManagerSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(IsolatedStorageCacheStorageDataManageabilityProvider), typeof(IsolatedStorageCacheStorageData), typeof(CacheManagerSettingsManageabilityProvider))]
