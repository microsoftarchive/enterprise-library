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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestClass]
    public class SecurityCacheInstrumentationFixture
    {
        SecurityCacheProviderInstrumentationProvider instrumentationProvider;
        SecurityCacheProviderInstrumentationListener disabledInstrumentationListener;
        SecurityCacheProviderInstrumentationListener enabledInstrumentationListener;
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
            instrumentationProvider = new SecurityCacheProviderInstrumentationProvider();
            enabledInstrumentationListener = new SecurityCacheProviderInstrumentationListener(instanceName, true, true, true, formatter);
            disabledInstrumentationListener = new SecurityCacheProviderInstrumentationListener(instanceName, false, false, false, formatter);
            totalSecurityCacheReadPerformedCounter = new EnterpriseLibraryPerformanceCounter(SecurityCacheProviderInstrumentationListener.PerfomanceCountersCategoryName, SecurityCacheProviderInstrumentationListener.TotalSecurityCacheReadPerformedCounterName, formattedInstanceName);

            ClearCounters();
        }

        void ClearCounters()
        {
            totalSecurityCacheReadPerformedCounter.Clear();
        }

        [TestMethod]
        public void TotalSecurityCacheReadPerformedCounterIncremented()
        {
            enabledInstrumentationListener.SecurityCacheReadPerformed(this, new SecurityCacheOperationEventArgs(SecurityEntityType.Identity, null));

            Assert.AreEqual(1, totalSecurityCacheReadPerformedCounter.Value);
        }

        [TestMethod]
        public void SecurityCacheCheckDoesNotifyWmiIfEnabled()
        {
            new ReflectionInstrumentationBinder().Bind(instrumentationProvider, enabledInstrumentationListener);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
            {
                FireSecurityCacheReadPerformed();
                eventListener.WaitForEvents();

                Assert.AreEqual(numberOfEvents, eventListener.EventsReceived.Count);
            }
        }

        [TestMethod]
        public void SecurityCacheCheckDoesNotNotifyWmiIfDisabled()
        {
            new ReflectionInstrumentationBinder().Bind(instrumentationProvider, disabledInstrumentationListener);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
            {
                FireSecurityCacheReadPerformed();
                eventListener.WaitForEvents();

                Assert.AreEqual(0, eventListener.EventsReceived.Count);
            }
        }

        [TestMethod]
        public void SecurityCacheCheckDoesUpdatePerformanceCountersIfEnabled()
        {
            new ReflectionInstrumentationBinder().Bind(instrumentationProvider, enabledInstrumentationListener);

            EnterpriseLibraryPerformanceCounter performanceCounter
                = CreatePerformanceCounter(SecurityCacheProviderInstrumentationListener.SecurityCacheReadPerformedCounterName);
            performanceCounter.Clear();
            Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);

            FireSecurityCacheReadPerformed();

            // Timing dependant
            Assert.IsFalse(performanceCounter.GetValueFor(formattedInstanceName) == 0);
        }

        [TestMethod]
        public void SecurityCacheCheckDoesNotUpdatePerformanceCountersIfDisabled()
        {
            new ReflectionInstrumentationBinder().Bind(instrumentationProvider, disabledInstrumentationListener);

            EnterpriseLibraryPerformanceCounter performanceCounter
                = CreatePerformanceCounter(SecurityCacheProviderInstrumentationListener.SecurityCacheReadPerformedCounterName);
            performanceCounter.Clear();
            Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);

            FireSecurityCacheReadPerformed();

            // Timing dependant
            Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);
        }

        EnterpriseLibraryPerformanceCounter CreatePerformanceCounter(string counterName)
        {
            return new EnterpriseLibraryPerformanceCounter(
                SecurityCacheProviderInstrumentationListener.PerfomanceCountersCategoryName,
                counterName,
                formattedInstanceName);
        }

        void FireSecurityCacheReadPerformed()
        {
            for (int i = 0; i < numberOfEvents; i++)
            {
                try
                {
                    instrumentationProvider.FireSecurityCacheReadPerformed(SecurityEntityType.Identity, null);
                }
                catch (Exception) {}
            }
        }
    }
}