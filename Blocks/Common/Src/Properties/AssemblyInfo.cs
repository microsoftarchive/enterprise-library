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
using System.Security;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;

[assembly: SecurityPermission(SecurityAction.RequestMinimum)]
[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]
[assembly: AssemblyTitle("Enterprise Library Shared Library")]
[assembly: AssemblyDescription("Enterprise Library Shared Library")]
[assembly: AssemblyVersion("5.0.414.0")]
[assembly: ReflectionPermission(SecurityAction.RequestMinimum)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]



[assembly: HandlesSection(InstrumentationConfigurationSection.SectionName)]
[assembly: HandlesSection(ConfigurationSourceSection.SectionName)]
[assembly: HandlesSection(AppSettingsDesignTime.AppSettingsSectionName)]

[assembly: AddApplicationBlockCommand(
                InstrumentationConfigurationSection.SectionName,
                typeof(InstrumentationConfigurationSection),
                TitleResourceType = typeof(DesignResources),
                TitleResourceName = "AddInstrumentationSettingsTitle",
                CommandModelTypeName = CommonDesignTime.CommandTypeNames.AddInstrumentationApplicationBlockCommand)]

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
