//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;

[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]

[assembly: AssemblyTitle("Enterprise Library Policy Injection Application Block")]
[assembly: AssemblyDescription("Enterprise Library Policy Injection Application Block")]
[assembly: AssemblyVersion("6.0.0.0")]
[assembly: AssemblyFileVersion("6.0.1304.0")]

[assembly: AllowPartiallyTrustedCallers]

[assembly: ComVisible(false)]

[assembly: HandlesSection(PolicyInjectionSettings.SectionName)]

[assembly: AddApplicationBlockCommand(
                PolicyInjectionSettings.SectionName,
                typeof(PolicyInjectionSettings),
                TitleResourceName = "AddPolicyInjectionSettings",
                TitleResourceType = typeof(DesignResources))]
