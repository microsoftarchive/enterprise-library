//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;

[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

[assembly: AssemblyTitle("Enterprise Library Validation Application Block")]
[assembly: AssemblyDescription("Enterprise Library Validation Application Block")]
[assembly: AssemblyVersion("5.0.414.0")]

[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]

[assembly: HandlesSection(ValidationSettings.SectionName)]
[assembly: AddApplicationBlockCommand(
                ValidationSettings.SectionName,
                typeof(ValidationSettings),
                TitleResourceType = typeof(DesignResources),
                TitleResourceName = "AddValidationSettings")]
