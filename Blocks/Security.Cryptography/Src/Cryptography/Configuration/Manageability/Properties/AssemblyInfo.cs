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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability;

[assembly : ConfigurationSectionManageabilityProvider("securityCryptographyConfiguration", typeof(CryptographySettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(CustomHashProviderDataManageabilityProvider), typeof(CustomHashProviderData), typeof(CryptographySettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(KeyedHashAlgorithmProviderDataManageabilityProvider), typeof(KeyedHashAlgorithmProviderData), typeof(CryptographySettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(HashAlgorithmProviderDataManageabilityProvider), typeof(HashAlgorithmProviderData), typeof(CryptographySettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(CustomSymmetricCryptoProviderDataManageabilityProvider), typeof(CustomSymmetricCryptoProviderData), typeof(CryptographySettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(DpapiSymmetricCryptoProviderDataManageabilityProvider), typeof(DpapiSymmetricCryptoProviderData), typeof(CryptographySettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(SymmetricAlgorithmProviderDataManageabilityProvider), typeof(SymmetricAlgorithmProviderData), typeof(CryptographySettingsManageabilityProvider))]
