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
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;

[assembly : ConfigurationDesignManager(typeof(CachingDatabaseConfigurationDesignManager), typeof(ConnectionStringsConfigurationDesignManager))]
[assembly : ConfigurationDesignManager(typeof(CachingDatabaseConfigurationDesignManager), typeof(CachingConfigurationDesignManager))]
[assembly : ComVisible(false)]
[assembly : ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true)]
[assembly : SecurityPermission(SecurityAction.RequestMinimum)]
[assembly : AssemblyTitle("Enterprise Library Caching Data Provider Design")]
[assembly : AssemblyDescription("Enterprise Library Caching Data Provider Design")]
[assembly : AssemblyVersion("4.0.0.815")]
[assembly : AllowPartiallyTrustedCallers]
[assembly : SecurityTransparent]
