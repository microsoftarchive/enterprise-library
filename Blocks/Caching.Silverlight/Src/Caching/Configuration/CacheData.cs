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
