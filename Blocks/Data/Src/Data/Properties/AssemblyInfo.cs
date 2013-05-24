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
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]
[assembly: AssemblyTitle("Enterprise Library Data Access Application Block")]
[assembly: AssemblyDescription("Enterprise Library Data Access Application Block")]
[assembly: AssemblyVersion("6.0.0.0")]
[assembly: AssemblyFileVersion("6.0.1304.0")]
[assembly: SecurityTransparent]

[assembly: ComVisible(false)]

[assembly: HandlesSection(DataAccessDesignTime.ConnectionStringSettingsSectionName)]
[assembly: HandlesSection(DatabaseSettings.SectionName, ClearOnly = true)]

[assembly: AddApplicationBlockCommand(
            DataAccessDesignTime.ConnectionStringSettingsSectionName,
            typeof(ConnectionStringsSection),
            TitleResourceName = "AddDataSettings",
            TitleResourceType = typeof(DesignResources),
            CommandModelTypeName = DataAccessDesignTime.CommandTypeNames.AddDataAccessBlockCommand)]
