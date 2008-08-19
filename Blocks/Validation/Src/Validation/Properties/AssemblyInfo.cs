//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Security.Permissions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Management.Instrumentation;
using System.Security;

[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

[assembly: AssemblyTitle("Enterprise Library Validation Application Block")]
[assembly: AssemblyDescription("Enterprise Library Validation Application Block")]
[assembly: AssemblyVersion("4.0.0.815")]

[assembly: Instrumented(@"root\EnterpriseLibrary")]
[assembly: WmiConfiguration(@"root\EnterpriseLibrary", HostingModel = ManagementHostingModel.Decoupled, IdentifyLevel = false)]

[assembly: InternalsVisibleTo("Microsoft.Practices.EnterpriseLibrary.Validation.Tests")]

[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]
