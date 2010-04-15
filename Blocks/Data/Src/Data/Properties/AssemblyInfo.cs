//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

[assembly: SecurityPermission(SecurityAction.RequestMinimum)]
[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]
[assembly: AssemblyTitle("Enterprise Library Data Access Application Block")]
[assembly: AssemblyDescription("Enterprise Library Data Access Application Block")]
[assembly: AssemblyVersion("5.0.414.0")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]

[assembly: HandlesSection(DataAccessDesignTime.ConnectionStringSettingsSectionName)]
[assembly: HandlesSection(DatabaseSettings.SectionName, ClearOnly = true)]

[assembly: AddApplicationBlockCommand(
            DataAccessDesignTime.ConnectionStringSettingsSectionName,
            typeof(ConnectionStringsSection),
            TitleResourceName = "AddDataSettings",
            TitleResourceType = typeof(DesignResources),
            CommandModelTypeName = DataAccessDesignTime.CommandTypeNames.AddDataAccessBlockCommand)]
