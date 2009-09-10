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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a custom cache manager.
    /// </summary>
    /// <seealso cref="CustomCacheManagerData"/>
    public interface ICachingConfigurationCustomCacheManager : ICachingConfiguration, IFluentInterface
    {
        /// <summary>
        /// Specifies the current custom cache manager as the default cache manager instance.
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure this custom cache manager.</returns>
        /// <seealso cref="CustomCacheManagerData"/>
        ICachingConfigurationCustomCacheManager UseAsDefaultCache();

    }
}
