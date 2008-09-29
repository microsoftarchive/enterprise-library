//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;

[assembly : ConfigurationDesignManager(typeof(DataConfigurationDesignManager))]
[assembly : ConfigurationDesignManager(typeof(ConnectionStringsConfigurationDesignManager), typeof(DataConfigurationDesignManager))]
[assembly : ConfigurationDesignManager(typeof(OracleConnectionConfigurationDesignManager), typeof(ConnectionStringsConfigurationDesignManager))]
[assembly : ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true)]
[assembly : SecurityPermission(SecurityAction.RequestMinimum)]
[assembly : AssemblyTitle("Enterprise Library Data Access Application Block Design")]
[assembly : AssemblyDescription("Enterprise Library Data Access Application Block Design")]
[assembly : AssemblyVersion("4.0.0.926")]
[assembly : ComVisible(false)]
[assembly : AllowPartiallyTrustedCallers]
[assembly : SecurityTransparent]
