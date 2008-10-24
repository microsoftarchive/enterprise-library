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
using System.Configuration.Install;
using System.Management.Instrumentation;

[assembly: Instrumented(@"root\EnterpriseLibrary")]

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : DefaultManagementProjectInstaller 
    {
        /// <summary>
        /// Represents the installer for the instrumentation events. Not intended for direct use.
        /// </summary>
        public ProjectInstaller()
        {
            ManagementInstaller managementInstaller = new ManagementInstaller();
            Installers.Add(managementInstaller);
        }
    }
}
