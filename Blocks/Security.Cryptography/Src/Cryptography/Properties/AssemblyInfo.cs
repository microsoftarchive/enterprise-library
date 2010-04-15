//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]
[assembly: AssemblyTitle("Enterprise Library Cryptography Application Block")]
[assembly: AssemblyDescription("Enterprise Library Cryptography Application Block")]
[assembly: AssemblyVersion("5.0.414.0")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]

[assembly: HandlesSection(CryptographySettings.SectionName)]

[assembly: AddApplicationBlockCommand(
                CryptographySettings.SectionName,
                typeof(CryptographySettings),
                TitleResourceName = "AddCryptographySettings",
                TitleResourceType = typeof(DesignResources))]
