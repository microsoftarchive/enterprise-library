//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigurationSourceBuilder"/> extensions to support creation of cryptography configuration settings.
    /// </summary>
    public static class CryptographyConfigurationSourceBuilderExtension
    {
        /// <summary>
        /// Main entry point to configure a <see cref="CryptographySettings"/> section.
        /// </summary>
        /// <param name="configurationSourceBuilder">The builder interface to extend.</param>
        /// <returns>A fluent interface to further configure the cryptography configuration section.</returns>
        public static IConfigureCryptography ConfigureCryptography(this IConfigurationSourceBuilder configurationSourceBuilder)
        {
            return new ConfigureCryptographyBuilder(configurationSourceBuilder);
        }

        private class ConfigureCryptographyBuilder : IConfigureCryptography, IConfigureCryptographyExtension
        {
            CryptographySettings cryptoSettings;
            public ConfigureCryptographyBuilder(IConfigurationSourceBuilder configurationSourceBuilder)
            {
                cryptoSettings = new CryptographySettings();
                configurationSourceBuilder.AddSection(CryptographySettings.SectionName, cryptoSettings);
            }

            CryptographySettings IConfigureCryptographyExtension.CryptographySettings
            {
                get { return cryptoSettings; }
            }
        }
    }



}
