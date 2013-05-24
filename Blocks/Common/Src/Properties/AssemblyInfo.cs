//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]
[assembly: AssemblyTitle("Enterprise Library Shared Library")]
[assembly: AssemblyDescription("Enterprise Library Shared Library")]
[assembly: AssemblyVersion("6.0.0.0")]
[assembly: AssemblyFileVersion("6.0.1304.0")]

[assembly: AllowPartiallyTrustedCallers]

[assembly: ComVisible(false)]

[assembly: HandlesSection(ConfigurationSourceSection.SectionName)]
[assembly: HandlesSection(AppSettingsDesignTime.AppSettingsSectionName)]

[assembly: AddApplicationBlockCommand(
                AppSettingsDesignTime.AppSettingsSectionName,
                typeof(AppSettingsSection),
                TitleResourceType = typeof(DesignResources),
                TitleResourceName = "AddApplicationSettingsTitle")]

[assembly: AddApplicationBlockCommand(ConfigurationSourceSection.SectionName,
            typeof(ConfigurationSourceSection),
            TitleResourceType = typeof(DesignResources),
            TitleResourceName = "AddConfigurationSourcesTitle",
            CommandModelTypeName = ConfigurationSourcesDesignTime.CommandTypeNames.AddConfigurationSourcesBlockCommand)]
