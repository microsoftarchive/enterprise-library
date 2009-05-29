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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestClass]
    public class SecurityCacheInstrumentationFixture
    {
        SecurityCacheProviderInstrumentationProvider enabledInstrumentationProvider;
        SecurityCacheProviderInstrumentationProvider disabledInstrumentationProvider;
        EnterpriseLibraryPerformanceCounter totalSecurityCacheReadPerformedCounter;
        AppDomainNameFormatter formatter;
        const string instanceName = "testInstance";
        string formattedInstanceName;
        const int numberOfEvents = 50;

        [TestInitialize]
        public void SetUp()
        {
            formatter = new AppDomainNameFormatter();
            formattedInstanceName = formatter.CreateName(instanceName);
            enabledInstrumentationProvider = new SecurityCacheProviderInstrumentationProvider(instanceName, true, true, true, formatter);
            disabledInstrumentationProvider = new SecurityCacheProviderInstrumentationProvider(instanceName, false, false, false, formatter);

            totalSecurityCacheReadPerformedCounter = new EnterpriseLibraryPerformanceCounter(SecurityCacheProviderInstrumentationProvider.PerfomanceCountersCategoryName, SecurityCacheProviderInstrumentationProvider.TotalSecurityCacheReadPerformedCounterName, formattedInstanceName);

            ClearCounters();
        }

        void ClearCounters()
        {
            totalSecurityCacheReadPerformedCounter.Clear();
        }

        [TestMethod]
        public void TotalSecurityCacheReadPerformedCounterIncremented()
        {
            enabledInstrumentationProvider.FireSecurityCacheReadPerformed(SecurityEntityType.Identity, null);

            Assert.AreEqual(1, totalSecurityCacheReadPerformedCounter.Value);
        }

        [TestMethod]
        public void SecurityCacheCheckDoesNotifyWmiIfEnabled()
        {
            using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
            {
                FireSecurityCacheReadPerformed(enabledInstrumentationProvider);
                eventListener.WaitForEvents();

                Assert.AreEqual(numberOfEvents, eventListener.EventsReceived.Count);
            }
        }

        [TestMethod]
        public void SecurityCacheCheckDoesNotNotifyWmiIfDisabled()
        {
            using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
            {
                FireSecurityCacheReadPerformed(disabledInstrumentationProvider);
                eventListener.WaitForEvents();

                Assert.AreEqual(0, eventListener.EventsReceived.Count);
            }
        }

        [TestMethod]
        public void SecurityCacheCheckDoesUpdatePerformanceCountersIfEnabled()
        {
            EnterpriseLibraryPerformanceCounter performanceCounter
                = CreatePerformanceCounter(SecurityCacheProviderInstrumentationProvider.SecurityCacheReadPerformedCounterName);
            performanceCounter.Clear();
            Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);

            FireSecurityCacheReadPerformed(enabledInstrumentationProvider);

            // Timing dependant
            Assert.IsFalse(performanceCounter.GetValueFor(formattedInstanceName) == 0);
        }

        [TestMethod]
        public void SecurityCacheCheckDoesNotUpdatePerformanceCountersIfDisabled()
        {
            EnterpriseLibraryPerformanceCounter performanceCounter
                = CreatePerformanceCounter(SecurityCacheProviderInstrumentationProvider.SecurityCacheReadPerformedCounterName);
            performanceCounter.Clear();
            Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);

            FireSecurityCacheReadPerformed(disabledInstrumentationProvider);

            // Timing dependant
            Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);
        }

        EnterpriseLibraryPerformanceCounter CreatePerformanceCounter(string counterName)
        {
            return new EnterpriseLibraryPerformanceCounter(
                SecurityCacheProviderInstrumentationProvider.PerfomanceCountersCategoryName,
                counterName,
                formattedInstanceName);
        }

        void FireSecurityCacheReadPerformed(ISecurityCacheProviderInstrumentationProvider instrumentationProvider)
        {
            for (int i = 0; i < numberOfEvents; i++)
            {
                try
                {
                    instrumentationProvider.FireSecurityCacheReadPerformed(SecurityEntityType.Identity, null);
                }
                catch (Exception) { }
            }
        }
    }
}
