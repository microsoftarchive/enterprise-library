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

using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Base class for fluent interface builders that extend the caching configuration fluent interface.
    /// </summary>
    public abstract class ConfigureCachingExtension : IConfigureCaching, IConfigureCachingExtension
    {
        private readonly IConfigureCachingExtension contextExtension;

        /// <summary>
        /// Creates an instance of <see cref="ConfigureCachingExtension"/> passing the caching configuration's fluent interface builder.
        /// </summary>
        /// <param name="context">The current caching configuration's fluent interface builder.<br/>
        /// This interface must implement <see cref="IConfigureCachingExtension"/>.</param>
        protected ConfigureCachingExtension(IConfigureCaching context)
        {
            contextExtension = context as IConfigureCachingExtension;
            if (contextExtension == null) throw new ArgumentException(
                string.Format(CultureInfo.CurrentCulture, Resources.ExceptionTypeMustImplementInterface, typeof(IConfigureCachingExtension).FullName),
                "context");
        }


        /// <summary>
        /// Returns the <see cref="CachingSettings"/> instance that is currently being build up.
        /// </summary>
        protected CachingSettings CachingSettings
        {
            get { return contextExtension.CachingSettings; }
        }

        CachingSettings IConfigureCachingExtension.CachingSettings
        {
            get { return contextExtension.CachingSettings; }
        }
    }


    /// <summary>
    /// Allows access to the underlying <see cref="CachingSettings"/> being configured.
    /// </summary>
    public interface IConfigureCachingExtension
    {
        /// <summary>
        /// Returns the <see cref="CachingSettings"/> instance that is currently being build up.
        /// </summary>
        CachingSettings CachingSettings { get; }
    }
}
