//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.ComponentModel;
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Manageability
{
	/// <summary>
	/// Installer for the WMI objects defined in the assembly.
	/// </summary>
	[RunInstaller(true)]
	public class Installer : DefaultManagementInstaller
	{
	}
}
