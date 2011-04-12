using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Design.ViewModel.BlockSpecifics.Caching;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// <para>Configuration settings for the Caching Application Block for Silverlight Integration Pack.</para>
    /// </summary>
    [ViewModel(typeof(CachingSettingsViewModel))]
    [ResourceDescription(typeof(CachingResources), "CachingSettingsDescription")]
    [ResourceDisplayName(typeof(CachingResources), "CachingSettingsDisplayName")]
    public class CachingSettings : SerializableConfigurationSection
    {
        private const string defaultCacheProperty = "defaultCache";
        private const string cachesProperty = "caches";

        /// <summary>
        /// Gets the configuration section name for the Caching Application Block for Silverlight Integration Pack.
        /// </summary>
        public const string SectionName = "cachingSilverlightConfiguration";

        /// <summary>
        /// The instance name of the default <see cref="CacheData"/> instance.
        /// </summary>
        [ConfigurationProperty(defaultCacheProperty, IsRequired = false)]
        [Reference(typeof(NameTypeConfigurationElementCollection<CacheData, CustomCacheData>), typeof(CacheData))]
        [ResourceDescription(typeof(CachingResources), "CachingSettingsDefaultCacheDescription")]
        [ResourceDisplayName(typeof(CachingResources), "CachingSettingsDefaultCacheDisplayName")]
        public string DefaultCache
        {
            get { return (string)this[defaultCacheProperty]; }
            set { this[defaultCacheProperty] = value; }
        }

        /// /// <summary>
        /// <para>Gets the <see cref="Caches"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The caches available in configuration. The default is an empty collection.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>caches</c> element in configuration.</para>
        /// </remarks>
        [ConfigurationProperty(cachesProperty, IsRequired = false)]
        [ConfigurationCollection(typeof(CacheData))]
        [ResourceDescription(typeof(CachingResources), "CachingSettingsCachesDescription")]
        [ResourceDisplayName(typeof(CachingResources), "CachingSettingsCachesDisplayName")]
        public NameTypeConfigurationElementCollection<CacheData, CustomCacheData> Caches
        {
            get { return (NameTypeConfigurationElementCollection<CacheData, CustomCacheData>)base[cachesProperty]; }
        }
    }
}
