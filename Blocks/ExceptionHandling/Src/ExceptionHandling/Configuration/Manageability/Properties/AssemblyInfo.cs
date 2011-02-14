//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability;

[assembly : ConfigurationSectionManageabilityProvider(ExceptionHandlingSettings.SectionName, typeof(ExceptionHandlingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(CustomHandlerDataManageabilityProvider), typeof(CustomHandlerData), typeof(ExceptionHandlingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(ReplaceHandlerDataManageabilityProvider), typeof(ReplaceHandlerData), typeof(ExceptionHandlingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(WrapHandlerDataManageabilityProvider), typeof(WrapHandlerData), typeof(ExceptionHandlingSettingsManageabilityProvider))]
