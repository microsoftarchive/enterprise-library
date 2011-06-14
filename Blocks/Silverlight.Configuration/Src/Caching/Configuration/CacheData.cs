//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// <para>Represents the common configuration data for all silverlight caches.</para>
    /// </summary>
    public class CacheData : NameTypeConfigurationElement
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CacheData"/> class.</para>
        /// </summary>
        public CacheData()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CacheData"/> class with a cache type.
        /// </summary>
        /// <param name="type">The type of the cache.</param>
        protected CacheData(Type type)
            : this(null, type)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CacheData"/> class with a name and a cache type.
        /// </summary>
        /// <param name="name">The name of the configured cache.</param>
        /// <param name="type">The type of the cache.</param>
        public CacheData(string name, Type type)
            : base(name, type)
        {
        }
    }
}
