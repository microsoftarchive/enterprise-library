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
using System.Runtime.InteropServices;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;
using System.Security;

[assembly: ConfigurationDesignManager(typeof(CachingCryptographyConfigurationDesignManager), typeof(SecurityCryptographyConfigurationDesignManager))]
[assembly: ConfigurationDesignManager(typeof(CachingCryptographyConfigurationDesignManager), typeof(CachingConfigurationDesignManager))]

[assembly: ComVisible(false)]

[assembly: ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

[assembly: AssemblyTitle("Enterprise Library Caching Cryptography Provider Design")]
[assembly: AssemblyDescription("Enterprise Library Caching Cryptography Provider Design")]
[assembly: AssemblyVersion("4.0.0.829")]

[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]
