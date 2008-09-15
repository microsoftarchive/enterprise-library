//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;

[assembly : ConfigurationDesignManager(typeof(ExceptionHandlingLoggingConfigurationDesignManager), typeof(LoggingConfigurationDesignManager))]
[assembly : ConfigurationDesignManager(typeof(ExceptionHandlingLoggingConfigurationDesignManager), typeof(ExceptionHandlingConfigurationDesignManager))]
[assembly : ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true)]
[assembly : SecurityPermission(SecurityAction.RequestMinimum)]
[assembly : ComVisible(false)]
[assembly : AssemblyTitle("Enterprise Library Exception Handling And Logging Application Block Design")]
[assembly : AssemblyDescription("Enterprise Library Exception Handling And Logging Application Block Design")]
[assembly : AssemblyVersion("4.0.0.912")]
[assembly : AllowPartiallyTrustedCallers]
[assembly : SecurityTransparent]
