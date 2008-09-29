//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright ? Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security.Permissions;
using System.Management.Instrumentation;

[assembly: AssemblyTitle("Enterprise Library Shared Library Tests")]
[assembly: AssemblyDescription("Enterprise Library Shared Library Tests")]
[assembly: AssemblyVersion("4.0.0.926")]


[assembly: Instrumented(@"root\EnterpriseLibrary")]
[assembly: WmiConfiguration(@"root\EnterpriseLibrary", HostingModel = ManagementHostingModel.Decoupled, IdentifyLevel = false)]
