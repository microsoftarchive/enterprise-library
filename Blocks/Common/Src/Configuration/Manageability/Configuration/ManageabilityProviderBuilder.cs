//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration
{
    /// <summary>
    /// Builder for manageability configuration providers.
    /// </summary>
    public class ManageabilityProviderBuilder
    {
        /// <summary>
        /// Create a manageability configuration provider.
        /// </summary>
        /// <param name="manageabilityProviderData">The provdier data.</param>
        /// <returns>A <see cref="ConfigurationElementManageabilityProvider"/> object.</returns>
        public ConfigurationElementManageabilityProvider CreateConfigurationElementManageabilityProvider(
            ConfigurationElementManageabilityProviderData manageabilityProviderData)
        {
            if (manageabilityProviderData == null) throw new ArgumentNullException("manageabilityProviderData");

            return (ConfigurationElementManageabilityProvider)Activator.CreateInstance(manageabilityProviderData.Type);
        }

        /// <summary>
        /// Create a manageability configuration provider.
        /// </summary>
        /// <param name="manageabilityProviderData">The provdier data.</param>
        /// <returns>A <see cref="ConfigurationSectionManageabilityProvider"/> object.</returns>
        public ConfigurationSectionManageabilityProvider CreateConfigurationSectionManageabilityProvider(
            ConfigurationSectionManageabilityProviderData manageabilityProviderData)
        {
            if (manageabilityProviderData == null) throw new ArgumentNullException("manageabilityProviderData");

            IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();

            foreach (ConfigurationElementManageabilityProviderData subProviderData in manageabilityProviderData.ManageabilityProviders)
            {
                ConfigurationElementManageabilityProvider subManageabilityProvider
                    = CreateConfigurationElementManageabilityProvider(subProviderData);
                subProviders.Add(subProviderData.TargetType, subManageabilityProvider);
            }

            return (ConfigurationSectionManageabilityProvider)Activator.CreateInstance(manageabilityProviderData.Type,
                                                                                       subProviders);
        }
    }
}
