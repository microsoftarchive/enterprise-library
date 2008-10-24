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
using System.Security.Permissions;
using System.Management.Instrumentation;

[assembly : FileIOPermission(SecurityAction.RequestMinimum)]
[assembly : SecurityPermission(SecurityAction.RequestMinimum)]
[assembly : RegistryPermission(SecurityAction.RequestMinimum)]
[assembly : ReflectionPermission(SecurityAction.RequestMinimum, Flags=ReflectionPermissionFlag.MemberAccess)]

[assembly: AssemblyTitle("Enterprise Library Logging Application Block MSMQ Distributor")]
[assembly: AssemblyDescription("Enterprise Library Logging Application Block MSMQ Distributor")]
[assembly: AssemblyVersion("4.0.0.1024")]


[assembly: Instrumented(@"root\EnterpriseLibrary")]
[assembly: WmiConfiguration(@"root\EnterpriseLibrary", HostingModel = ManagementHostingModel.Decoupled, IdentifyLevel = false)]
