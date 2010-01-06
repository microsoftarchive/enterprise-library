//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Management.Instrumentation;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

[assembly : SecurityPermission(SecurityAction.RequestMinimum)]
[assembly : ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]
[assembly : AssemblyTitle("Enterprise Library Caching Application Block")]
[assembly : AssemblyDescription("Enterprise Library Caching Application Block")]
[assembly : AssemblyVersion("4.1.0.0")]
[assembly : Instrumented(@"root\EnterpriseLibrary")]
[assembly : AllowPartiallyTrustedCallers]
[assembly : SecurityTransparent]

[assembly: HandlesSection(CacheManagerSettings.SectionName)]
[assembly: AddApplicationBlockCommand(
            CacheManagerSettings.SectionName,
            typeof(CacheManagerSettings),
            TitleResourceName = "AddCachingSettings",
            TitleResourceType = typeof(DesignResources),
            CommandModelTypeName = CachingDesignTime.CommandTypeNames.AddCachingBlockCommand)]
