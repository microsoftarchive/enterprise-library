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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability
{
    /// <summary>
    /// Provides an implementation for <see cref="CustomSymmetricCryptoProviderData"/> that
    /// splits policy overrides processing and WMI objects generation, performing appropriate logging of 
    /// policy processing errors.
    /// </summary>
    public class CustomSymmetricCryptoProviderDataManageabilityProvider
        : CustomProviderDataManageabilityProvider<CustomSymmetricCryptoProviderData>
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="CustomSymmetricCryptoProviderDataManageabilityProvider"/> class.
        /// </summary>
        public CustomSymmetricCryptoProviderDataManageabilityProvider()
            : base(Resources.SymmetricCryptoProviderPolicyNameTemplate)
        {
            CustomSymmetricCryptoProviderDataWmiMapper.RegisterWmiTypes();
        }

        /// <summary>
        /// Creates the <see cref="ConfigurationSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        protected override void GenerateWmiObjects(CustomSymmetricCryptoProviderData configurationObject,
                                                   ICollection<ConfigurationSetting> wmiSettings)
        {
            CustomSymmetricCryptoProviderDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
        }
    }
}
