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

using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    [RunInstaller(true)]
    public partial class PerfCounterInstaller : Installer
    {
        public PerfCounterInstaller()
        {
            InitializeComponent();
            InstallPerformanceCounters();
        }

        private void InstallPerformanceCounters()
        {
            PerformanceCounterInstaller installer = new PerformanceCounterInstaller();
            installer.CategoryName = EnterpriseLibraryPerformanceCounterFixture.counterCategoryName;
            installer.CategoryHelp = "J Random Text";
            installer.CategoryType = PerformanceCounterCategoryType.MultiInstance;

            CounterCreationData firstCounterData = new CounterCreationData(EnterpriseLibraryPerformanceCounterFixture.counterName, "Test Counter", PerformanceCounterType.NumberOfItems32);
            CounterCreationData secondCounterData = new CounterCreationData("SecondTestCounter", "Second Test Counter", PerformanceCounterType.NumberOfItems32);

            installer.Counters.Add(firstCounterData);
            installer.Counters.Add(secondCounterData);

            Installers.Add(installer);
        }
    }
}
