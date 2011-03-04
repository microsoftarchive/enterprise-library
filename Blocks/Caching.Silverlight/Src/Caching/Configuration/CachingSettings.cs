using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    [ContentProperty("Caches")]
    public class CachingSettings : ConfigurationSection, ITypeRegistrationsProvider
    {
        /// <summary>
        /// The name used to serialize the configuration section.
        /// </summary>
        public const string SectionName = BlockSectionNames.Caching;

        private NamedElementCollection<CacheData> caches = new NamedElementCollection<CacheData>();

        /// <summary>
        /// Gets the collection of caches.
        /// </summary>
        public NamedElementCollection<CacheData> Caches
        {
            get { return this.caches; }
        }

        /// <summary>
        /// Gets or sets the default cache name.
        /// </summary>
        public string DefaultCache { get; set; }

        public IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            return SetDefaultCacheManagerRegistration(this.Caches.SelectMany(cd => cd.GetRegistrations(configurationSource)));
        }

        private IEnumerable<TypeRegistration> SetDefaultCacheManagerRegistration(IEnumerable<TypeRegistration> cacheManagerRegistrations)
        {
            foreach (TypeRegistration registration in cacheManagerRegistrations)
            {
                if (registration.ServiceType == typeof(ObjectCache) && string.Equals(registration.Name, DefaultCache, StringComparison.CurrentCulture))
                {
                    registration.IsDefault = true;
                    yield return registration;
                }
                else
                {
                    yield return registration;
                }
            }
        }

        public IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
        {
            throw new System.NotImplementedException();
        }
    }
}
