//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Management.Instrumentation;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;


[assembly: SecurityPermission(SecurityAction.RequestMinimum)]
[assembly: AssemblyTitle("Enterprise Library Logging Application Block")]
[assembly: AssemblyDescription("Enterprise Library Logging Application Block")]
[assembly: AssemblyVersion("5.0.414.0")]
[assembly: Instrumented(@"root\EnterpriseLibrary")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]

[assembly: HandlesSection(LoggingSettings.SectionName)]
[assembly: AddApplicationBlockCommand(
                LoggingSettings.SectionName,
                typeof(LoggingSettings),
                TitleResourceName = "AddLoggingSettings",
                TitleResourceType = typeof(DesignResources),
                CommandModelTypeName = LoggingDesignTime.CommandTypeNames.AddLoggingBlockCommand)]
