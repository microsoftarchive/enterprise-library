//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design;
using System.Security;

[assembly : ConfigurationDesignManager(typeof(SecurityAzManConfigurationDesignManager), typeof(SecurityConfigurationDesignManager))]

[assembly : ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess=true)]

[assembly : AssemblyTitle("Enterprise Library Security AzMan Provider Design")]
[assembly : AssemblyDescription("Enterprise Library Security AzMan Provider Design")]
[assembly : AssemblyVersion("4.0.0.912")]

[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]
