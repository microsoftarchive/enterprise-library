//===============================================================================
// Microsoft patterns & practices Enterprise Library
// SQL Configuration Source QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Design;

[assembly: ConfigurationDesignManager(typeof(SqlConfigurationSourceDesignManager))]

[assembly: FileIOPermission(SecurityAction.RequestMinimum)]

[assembly: AssemblyTitle("Enterprise Library Sql Configuration Source Quickstart")]
[assembly: AssemblyDescription("Enterprise Library Sql Configuration Source Quickstart")]
[assembly: AssemblyVersion("2.0.0.0")]

[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]
