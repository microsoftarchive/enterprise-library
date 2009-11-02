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

using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides;

[assembly : ReflectionPermission(SecurityAction.RequestMinimum)]
[assembly : SecurityPermission(SecurityAction.RequestMinimum)]
[assembly : ComVisible(false)]
[assembly : AssemblyTitle("Enterprise Library Configuration Application Block Environmental Overrides Extension")]
[assembly : AssemblyDescription("Enterprise Library Configuration Application Block Environmental Overrides Extension")]
[assembly : AssemblyVersion("4.1.0.0")]
[assembly : AllowPartiallyTrustedCallers]
[assembly : SecurityTransparent]
