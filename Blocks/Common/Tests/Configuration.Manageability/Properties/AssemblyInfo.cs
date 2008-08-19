//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security.Permissions;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;

[assembly: AssemblyTitle("Enterprise Library Shared Library Manageability Tests")]
[assembly: AssemblyDescription("Enterprise Library Shared Library Manageability Tests")]
[assembly: AssemblyVersion("4.0.0.815")]

[assembly: ConfigurationSectionManageabilityProvider("section1", typeof(MockConfigurationSectionManageabilityProvider))]
[assembly: ConfigurationElementManageabilityProvider(typeof(MockConfigurationSectionManageabilityProviderAlt), typeof(string), typeof(MockConfigurationSectionManageabilityProvider))]