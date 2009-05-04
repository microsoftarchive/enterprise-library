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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability;

[assembly : ConfigurationSectionManageabilityProvider(SecuritySettings.SectionName, typeof(SecuritySettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(AuthorizationRuleProviderDataManageabilityProvider), typeof(AuthorizationRuleProviderData), typeof(SecuritySettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(CustomAuthorizationProviderDataManageabilityProvider), typeof(CustomAuthorizationProviderData), typeof(SecuritySettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(CustomSecurityCacheProviderDataManageabilityProvider), typeof(CustomSecurityCacheProviderData), typeof(SecuritySettingsManageabilityProvider))]
