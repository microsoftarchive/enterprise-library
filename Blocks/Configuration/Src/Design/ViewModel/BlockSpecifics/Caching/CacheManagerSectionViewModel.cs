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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using System.Windows;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{

#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class CacheManagerSectionViewModel : SectionViewModel
    {
        public const string DefaultNullBackingStore = "NullBackingStore";

        CacheManagerSettings cacheManagerSettings;

        public CacheManagerSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
            NullBackingStoreName = DefaultNullBackingStore;
            cacheManagerSettings = (CacheManagerSettings)section;
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


        protected override object CreateBindable()
        {
            var cacheManagers = DescendentElements().Where(x => x.ConfigurationType == typeof(NameTypeConfigurationElementCollection<CacheManagerDataBase, CustomCacheManagerData>)).First();
            var backingStores = DescendentElements().Where(x => x.ConfigurationType == typeof(NameTypeConfigurationElementCollection<CacheStorageData, CustomCacheStorageData>)).First();
            var storeEncryptionProviders = DescendentElements().Where(x => x.ConfigurationType == typeof(NameTypeConfigurationElementCollection<StorageEncryptionProviderData, StorageEncryptionProviderData>)).First();

            return new HorizontalListLayout(
                new HeaderedListLayout(cacheManagers), 
                new HeaderedListLayout(backingStores), 
                new HeaderedListLayout(storeEncryptionProviders));
        }


        protected override void BeforeSave(ConfigurationSection sectionToSave)
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

#pragma warning restore 1591
}
