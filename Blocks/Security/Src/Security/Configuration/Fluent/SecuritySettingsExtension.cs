//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigurationSourceBuilder"/> extensions to support creation of security configuration settings.
    /// </summary>
    public static class SecuritySettingsExtension
    {
        /// <summary>
        /// Main entry point to create a <see cref="SecuritySettings"/> section.
        /// </summary>
        /// <param name="configurationSourceBuilder">The builder interface to extend.</param>
        /// <returns>A fluent interface to further configure the security configuration section.</returns>
        public static IConfigureSecuritySettings ConfigureSecurity(this IConfigurationSourceBuilder configurationSourceBuilder)
        {
            return new ConfigureSecuritySettingsBuilder(configurationSourceBuilder);
        }

        private class ConfigureSecuritySettingsBuilder : IConfigureSecuritySettings, IConfigureSecuritySettingsExtension
        {
            SecuritySettings securitySettings;

            public ConfigureSecuritySettingsBuilder(IConfigurationSourceBuilder configurationSuorceBuilder)
            {
                securitySettings = new SecuritySettings();
                configurationSuorceBuilder.AddSection(SecuritySettings.SectionName, securitySettings);
            }

            SecuritySettings IConfigureSecuritySettingsExtension.SecuritySettings
            {
                get { return securitySettings; }
            }
        }
    }
}
