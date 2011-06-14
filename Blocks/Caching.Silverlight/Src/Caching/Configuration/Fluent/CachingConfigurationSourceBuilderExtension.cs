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
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigurationSourceBuilder"/> extensions to support creation of caching configuration settings.
    /// </summary>
    public static class CachingConfigurationSourceBuilderExtension
    {
        /// <summary>
        /// Main entry point to configure a <see cref="CachingSettings"/> section.
        /// </summary>
        /// <param name="configurationSourceBuilder">The builder interface to extend.</param>
        /// <returns>A fluent interface to further configure the caching configuration section.</returns>
        public static IConfigureCaching ConfigureCaching(this IConfigurationSourceBuilder configurationSourceBuilder)
        {
            return new ConfigureCachingBuilder(configurationSourceBuilder);
        }

        private class ConfigureCachingBuilder : IConfigureCaching, IConfigureCachingExtension
        {
            CachingSettings cryptoSettings;
            public ConfigureCachingBuilder(IConfigurationSourceBuilder configurationSourceBuilder)
            {
                cryptoSettings = new CachingSettings();
                configurationSourceBuilder.AddSection(CachingSettings.SectionName, cryptoSettings);
            }

            CachingSettings IConfigureCachingExtension.CachingSettings
            {
                get { return cryptoSettings; }
            }
        }
    }



}
