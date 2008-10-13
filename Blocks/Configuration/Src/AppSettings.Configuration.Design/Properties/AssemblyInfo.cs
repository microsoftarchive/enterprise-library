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
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

[assembly : ComVisible(false)]
[assembly : ConfigurationDesignManager(typeof(AppSettingsConfigurationDesignManager))]
[assembly : ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true)]
[assembly : SecurityPermission(SecurityAction.RequestMinimum)]
[assembly : AssemblyTitle("Enterprise Library AppSettings Design")]
[assembly : AssemblyDescription("Enterprise Library AppSettings Design")]
[assembly : AssemblyVersion("4.0.0.1010")]
[assembly : AllowPartiallyTrustedCallers]
[assembly : SecurityTransparent]
