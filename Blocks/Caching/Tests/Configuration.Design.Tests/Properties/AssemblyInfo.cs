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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;

[assembly: AssemblyTitle("Enterprise Library Caching Data Provider Design Tests")]
[assembly: AssemblyDescription("Enterprise Library Caching Data Provider Design Tests")]
[assembly: AssemblyVersion("4.0.0.815")]

[assembly: ConfigurationDesignManager(typeof(MockCacheManagerDataConfigurationDesignManager), typeof(CachingConfigurationDesignManager))]

