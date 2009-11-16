//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class CacheManagerSectionViewModel : PositionedSectionViewModel
    {
        public const string DefaultNullBackingStore = "NullBackingStore";

        CacheManagerSettings cacheManagerSettings;

        public CacheManagerSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
            NullBackingStoreName = DefaultNullBackingStore;
            cacheManagerSettings = (CacheManagerSettings)section;

            Positioning.PositionCollection("Cache Managers",
                typeof(NameTypeConfigurationElementCollection<CacheManagerDataBase, CustomCacheManagerData>),
                typeof(CacheManagerDataBase),
                new PositioningInstructions { FixedColumn = 0, FixedRow = 0 });

            Positioning.PositionCollection("Backing Stores",
                typeof(NameTypeConfigurationElementCollection<CacheStorageData, CustomCacheStorageData>),
                typeof(CacheStorageData),
                new PositioningInstructions { FixedColumn = 1, FixedRow = 0 });

            Positioning.PositionCollection("Encryption Providers",
                typeof(NameTypeConfigurationElementCollection<StorageEncryptionProviderData, StorageEncryptionProviderData>),
                typeof(StorageEncryptionProviderData),
                new PositioningInstructions { FixedColumn = 2, FixedRow = 0 });
        }

        public override void Initialize(InitializeContext context)
        {
            base.Initialize(context);

            var memoryBackingStores = cacheManagerSettings.BackingStores.Where(x => x.GetType() == typeof(CacheStorageData)).Select(x => x.Name);
            
            if (memoryBackingStores.Any())
            {
                NullBackingStoreName = memoryBackingStores.First();
            }
        }

        public override void BeforeSave(ConfigurationSection sectionToSave)
        {
            base.BeforeSave(sectionToSave);
            CacheManagerSettings cacheManagerToSave = (CacheManagerSettings)sectionToSave;
            if (!cacheManagerToSave.BackingStores.Any(x => x.Name == NullBackingStoreName))
            {
                cacheManagerToSave.BackingStores.Add(new CacheStorageData { Type = typeof(NullBackingStore), Name = NullBackingStoreName });
            }
        }

        public string NullBackingStoreName
        {
            get;
            private set;
        }
    }
}
