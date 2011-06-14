//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Logging;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Logging.Properties;

[assembly: AssemblyTitle("Enterprise Library Configuration for Silverlight Logging")]
[assembly: AssemblyDescription("Enterprise Library Configuration for Silverlight Logging")]
[assembly: AssemblyVersion("5.0.505.0")]

[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]

[assembly: AddApplicationBlockCommand(
            LoggingSettings.SectionName,
            typeof(LoggingSettings),
            TitleResourceName = "AddLoggingSettings",
            TitleResourceType = typeof(LoggingResources),
            CommandModelTypeName = LoggingDesignTime.CommandTypeNames.AddLoggingBlockCommand)]
