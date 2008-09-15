//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Runtime.InteropServices;
using System.Security;

[assembly: ConfigurationDesignManager(typeof(CachingConfigurationDesignManager))]

[assembly: ComVisible(false)]

[assembly: ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]

[assembly: AssemblyTitle("Enterprise Library Caching Application Block Design")]
[assembly: AssemblyDescription("Enterprise Library Caching Application Block Design")]
[assembly: AssemblyVersion("4.0.0.912")]

