//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

[assembly: AssemblyTitle("Enterprise Library Security Application Block")]
[assembly: AssemblyDescription("Enterprise Library Security Application Block")]
[assembly: AssemblyVersion("5.0.414.0")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]


[assembly: HandlesSection(SecuritySettings.SectionName)]
[assembly: AddApplicationBlockCommand(
                SecuritySettings.SectionName,
                typeof(SecuritySettings),
                TitleResourceName = "AddSecuritySettings",
                TitleResourceType = typeof(DesignResources))]
