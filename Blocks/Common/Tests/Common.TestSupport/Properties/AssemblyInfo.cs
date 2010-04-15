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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks;

[assembly: AssemblyTitle("Enterprise Library Shared Tests Support")]
[assembly: AssemblyDescription("Enterprise Library Shared Tests Support")]
[assembly: AssemblyVersion("5.0.414.0")]

[assembly: ConfigurationSectionManageabilityProvider("section1", typeof(MockConfigurationSectionManageabilityProvider))]
[assembly: ConfigurationElementManageabilityProvider(typeof(MockConfigurationSectionManageabilityProviderAlt), typeof(string), typeof(MockConfigurationSectionManageabilityProvider))]
[assembly: CLSCompliant(false)]
