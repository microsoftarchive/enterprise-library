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

using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Installers;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests
{
    [RunInstaller(true)]
    public class PerfCounterForCallHandlerTestsInstaller : System.Configuration.Install.Installer
    {
        private readonly PerformanceCountersInstaller countersInstaller;

        public PerfCounterForCallHandlerTestsInstaller()
        {
            countersInstaller = new PerformanceCountersInstaller(PerformanceCounterCallHandlerFixture.TestCategoryName);
            Installers.Add(countersInstaller);
        }
    }
}
