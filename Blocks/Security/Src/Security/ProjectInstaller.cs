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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Let the system know that the InstallUtil.exe tool will be run against this assembly
    /// </summary>
	[RunInstaller(true)]
	public partial class ProjectInstaller : DefaultManagementProjectInstaller
	{
        /// <summary>
        /// Represents the installer for the instrumentation events. Not intended for direct use.
        /// </summary>
		public ProjectInstaller()
		{
			InitializeComponent();

			Installers.Add(new ReflectionInstaller<EventLogInstallerBuilder>());
			Installers.Add(new ReflectionInstaller<PerformanceCounterInstallerBuilder>());
		}
	}
}
