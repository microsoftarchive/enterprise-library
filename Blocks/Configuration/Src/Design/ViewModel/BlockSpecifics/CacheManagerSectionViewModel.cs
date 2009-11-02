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

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class CacheManagerSectionViewModel : PositionedSectionViewModel
    {
        private const string NullBackingStore = "NullBackingStore";

        CacheManagerSettings cacheManagerSettings;

        public CacheManagerSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {

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

        public override void AfterOpen(IDesignConfigurationSource configurationSource)
        {
            var memoryBackingStores = cacheManagerSettings.BackingStores.Where(x => x.GetType() == typeof(CacheStorageData)).Select(x => x.Name);
            foreach (ElementViewModel cacheManager in DescendentElements(x => typeof(CacheManagerData).IsAssignableFrom(x.ConfigurationType)))
            {
                string storageProvider = (string)cacheManager.Property("CacheStorage").Value;
                if (memoryBackingStores.Contains(storageProvider))
                {
                    cacheManager.Property("CacheStorage").Value = string.Empty;
                }
            }
        }

        public override void BeforeSave(ConfigurationSection sectionToSave)
        {
            base.BeforeSave(sectionToSave);

            CacheManagerSettings cacheManagerToSave = (CacheManagerSettings)sectionToSave;
            if (!cacheManagerToSave.BackingStores.Any(x => x.Name == NullBackingStore))
            {
                cacheManagerToSave.BackingStores.Add(new CacheStorageData { Type = typeof(NullBackingStore), Name = NullBackingStore });
            }

            foreach(CacheManagerData cacheManager in cacheManagerToSave.CacheManagers.Where(x=>string.IsNullOrEmpty(x.Name)))
            {
                cacheManager.CacheStorage = NullBackingStore;
            }
        }
    }
}
