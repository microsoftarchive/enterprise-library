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

using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;


[assembly: AssemblyTitle("Enterprise Library Logging Application Block")]
[assembly: AssemblyDescription("Enterprise Library Logging Application Block")]
[assembly: AssemblyVersion("6.0.0.0")]
[assembly: AssemblyFileVersion("6.0.1304.0")]

[assembly: AllowPartiallyTrustedCallers]

[assembly: ComVisible(false)]

[assembly: HandlesSection(LoggingSettings.SectionName)]
[assembly: AddApplicationBlockCommand(
                LoggingSettings.SectionName,
                typeof(LoggingSettings),
                TitleResourceName = "AddLoggingSettings",
                TitleResourceType = typeof(DesignResources),
                CommandModelTypeName = LoggingDesignTime.CommandTypeNames.AddLoggingBlockCommand)]
