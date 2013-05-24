#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration;

[assembly: AssemblyTitle("Enterprise Library Transient Fault Handling Application Block")]
[assembly: AssemblyDescription("Enterprise Library Transient Fault Handling Application Block")]

[assembly: AssemblyVersion("6.0.0.0")]
[assembly: AssemblyFileVersion("6.0.1304.0")]

[assembly: ComVisible(false)]

[assembly: SecurityTransparent]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: HandlesSection(RetryPolicyConfigurationSettings.SectionName)]
[assembly: AddApplicationBlockCommand(
            RetryPolicyConfigurationSettings.SectionName,
            typeof(RetryPolicyConfigurationSettings),
            TitleResourceName = "AddRetryPolicyConfigurationSettings",
            TitleResourceType = typeof(DesignResources),
            CommandModelTypeName = TransientFaultHandlingDesignTime.CommandTypeNames.AddTransientFaultHandlingBlockCommand)]