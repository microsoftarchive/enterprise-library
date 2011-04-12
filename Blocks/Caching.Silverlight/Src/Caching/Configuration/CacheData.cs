using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// Base class for configuration objects describing instances of caches.
    /// </summary>
    public abstract class CacheData : NamedConfigurationElement
    {
        /// <summary>
        /// Returns the <see cref="TypeRegistration"/> entries for this configuration object.
        /// </summary>
        /// <returns>A set of registry entries.</returns>        
        public abstract IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource);
    }
}
