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
    /// Base class for fluent interface builders that extend the caching configuration fluent interface.
    /// </summary>
    public abstract class CacheManagerSettingsExtension : ICachingConfiguration, ICachingConfigurationExtension
    {
        readonly ICachingConfigurationExtension extensionContext;

        /// <summary>
        /// Creates an instance of <see cref="CacheManagerSettingsExtension"/> passing the caching configuration's fluent interface builder.
        /// </summary>
        /// <param name="context">The current caching configuration's fluent interface builder.<br/>
        /// This interface must implement <see cref="ICachingConfigurationExtension"/>.</param>
        public CacheManagerSettingsExtension(ICachingConfiguration context)
        {
            extensionContext = context as ICachingConfigurationExtension;

            if (extensionContext == null) throw new ArgumentException(
                String.Format(Resources.Culture, Resources.ExceptionParameterMustImplement, typeof(ICachingConfigurationExtension)),
                "context");
        }

        /// <summary>
        /// Returns the <see cref="CacheManagerSettings"/> instance that is currently being build up.
        /// </summary>
        protected CacheManagerSettings CachingSettings
        {
            get { return extensionContext.CachingSettings; }
        }

        CacheManagerSettings ICachingConfigurationExtension.CachingSettings
        {
            get { return extensionContext.CachingSettings; }
        }
    }

    /// <summary>
    /// Allows access to the underlying <see cref="CacheManagerSettings"/> being configured.
    /// </summary>
    public interface ICachingConfigurationExtension
    {
        /// <summary>
        /// Returns the <see cref="CacheManagerSettings"/> instance that is currently being build up.
        /// </summary>
        CacheManagerSettings CachingSettings { get; }
    }
}
