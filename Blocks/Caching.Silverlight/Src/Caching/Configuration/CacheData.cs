using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CacheData : NamedConfigurationElement
    {
        public abstract IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource);
    }
}
