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
    public class AuthorizationInstrumentationFixture
    {
        AuthorizationProviderInstrumentationProvider instrumentationProvider;
        AuthorizationProviderInstrumentationListener disabledInstrumentationListener;
        AuthorizationProviderInstrumentationListener enabledInstrumentationListener;
        EnterpriseLibraryPerformanceCounter totalAuthorizationCheckFailedCounter;
        EnterpriseLibraryPerformanceCounter totalAuthorizationCheckPerformedCounter;
        AppDomainNameFormatter formatter;
        const string instanceName = "testInstance";
        string formattedInstanceName;
        const string identity = "identity";
        const string taskName = "testTask";
        const int numberOfEvents = 50;

        [TestInitialize]
        public void SetUp()
        {
            formatter = new AppDomainNameFormatter();
            formattedInstanceName = formatter.CreateName(instanceName);
            instrumentationProvider = new AuthorizationProviderInstrumentationProvider();
            enabledInstrumentationListener = new AuthorizationProviderInstrumentationListener(instanceName, true, true, true, formatter);
            disabledInstrumentationListener = new AuthorizationProviderInstrumentationListener(instanceName, false, false, false, formatter);
            totalAuthorizationCheckFailedCounter = new EnterpriseLibraryPerformanceCounter(AuthorizationProviderInstrumentationListener.PerformanceCountersCategoryName, AuthorizationProviderInstrumentationListener.TotalAuthorizationCheckFailedCounterName, formattedInstanceName);
            totalAuthorizationCheckPerformedCounter = new EnterpriseLibraryPerformanceCounter(AuthorizationProviderInstrumentationListener.PerformanceCountersCategoryName, AuthorizationProviderInstrumentationListener.TotalAuthorizationCheckPerformedCounterName, formattedInstanceName);

            ClearCounters();
        }

        void ClearCounters()
        {
            totalAuthorizationCheckFailedCounter.Clear();
            totalAuthorizationCheckPerformedCounter.Clear();
        }

        [TestMethod]
        public void TotalAuthorizationCheckPerformedCounterIncremented()
        {
            enabledInstrumentationListener.AuthorizationCheckPerformed(this, new AuthorizationOperationEventArgs("foo", "bar"));

            Assert.AreEqual(1, totalAuthorizationCheckPerformedCounter.Value);
        }

        [TestMethod]
        public void TotalAuthorizationCheckFailedCounterIncremented()
        {
            enabledInstrumentationListener.AuthorizationCheckFailed(this, new AuthorizationOperationEventArgs("foo", "bar"));

            Assert.AreEqual(1, totalAuthorizationCheckFailedCounter.Value);
        }

        [TestMethod]
        public void AuthorizationCheckDoesNotifyWmiIfEnabled()
        {
            new ReflectionInstrumentationBinder().Bind(instrumentationProvider, enabledInstrumentationListener);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
            {
                FireAuthorizationCheckPerformed();
                eventListener.WaitForEvents();

                Assert.AreEqual(numberOfEvents, eventListener.EventsReceived.Count);
            }
        }

        [TestMethod]
        public void AuthorizationCheckDoesNotNotifyWmiIfDisabled()
        {
            new ReflectionInstrumentationBinder().Bind(instrumentationProvider, disabledInstrumentationListener);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
            {
                FireAuthorizationCheckPerformed();
                eventListener.WaitForEvents();

                Assert.AreEqual(0, eventListener.EventsReceived.Count);
            }
        }

        [TestMethod]
        public void AuthorizationCheckDoesUpdatePerformanceCountersIfEnabled()
        {
            new ReflectionInstrumentationBinder().Bind(instrumentationProvider, enabledInstrumentationListener);

            EnterpriseLibraryPerformanceCounter performanceCounter
                = CreatePerformanceCounter(AuthorizationProviderInstrumentationListener.AuthorizationCheckPerformedCounterName);
            performanceCounter.Clear();
            Assert.AreEqual(0L, performanceCounter.GetValueFor(formattedInstanceName));

            FireAuthorizationCheckPerformed();

            // Timing dependant
            Assert.AreEqual(50L, performanceCounter.GetValueFor(formattedInstanceName));
        }

        [TestMethod]
        public void AuthorizationCheckDoesNotUpdatePerformanceCountersIfDisabled()
        {
            new ReflectionInstrumentationBinder().Bind(instrumentationProvider, disabledInstrumentationListener);

            EnterpriseLibraryPerformanceCounter performanceCounter
                = CreatePerformanceCounter(AuthorizationProviderInstrumentationListener.AuthorizationCheckPerformedCounterName);
            performanceCounter.Clear();
            Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);

            FireAuthorizationCheckPerformed();

            // Timing dependant
            Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);
        }

        [TestMethod]
        public void AuthorizationFailureDoesNotifyWmiIfEnabled()
        {
            new ReflectionInstrumentationBinder().Bind(instrumentationProvider, enabledInstrumentationListener);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
            {
                FireAuthorizationCheckFailed();
                eventListener.WaitForEvents();

                Assert.AreEqual(numberOfEvents, eventListener.EventsReceived.Count);
            }
        }

        [TestMethod]
        public void AuthorizationFailureDoesNotNotifyWmiIfDisabled()
        {
            new ReflectionInstrumentationBinder().Bind(instrumentationProvider, disabledInstrumentationListener);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
            {
                FireAuthorizationCheckFailed();
                eventListener.WaitForEvents();

                Assert.AreEqual(0, eventListener.EventsReceived.Count);
            }
        }

        [TestMethod]
        public void AuthorizationFailureDoesUpdatePerformanceCountersIfEnabled()
        {
            new ReflectionInstrumentationBinder().Bind(instrumentationProvider, enabledInstrumentationListener);

            EnterpriseLibraryPerformanceCounter performanceCounter
                = CreatePerformanceCounter(AuthorizationProviderInstrumentationListener.AuthorizationCheckFailedCounterName);
            performanceCounter.Clear();
            Assert.AreEqual(0L, performanceCounter.GetValueFor(formattedInstanceName));

            FireAuthorizationCheckFailed();

            // Timing dependant
            Assert.AreEqual(50L, performanceCounter.GetValueFor(formattedInstanceName));
        }

        [TestMethod]
        public void AuthorizationFailureDoesNotUpdatePerformanceCountersIfDisabled()
        {
            new ReflectionInstrumentationBinder().Bind(instrumentationProvider, disabledInstrumentationListener);

            EnterpriseLibraryPerformanceCounter performanceCounter
                = CreatePerformanceCounter(AuthorizationProviderInstrumentationListener.AuthorizationCheckFailedCounterName);
            performanceCounter.Clear();
            Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);

            FireAuthorizationCheckFailed();

            // Timing dependant
            Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);
        }

        EnterpriseLibraryPerformanceCounter CreatePerformanceCounter(string counterName)
        {
            return new EnterpriseLibraryPerformanceCounter(
                AuthorizationProviderInstrumentationListener.PerformanceCountersCategoryName,
                counterName,
                formattedInstanceName);
        }

        void FireAuthorizationCheckPerformed()
        {
            for (int i = 0; i < numberOfEvents; i++)
            {
                try
                {
                    instrumentationProvider.FireAuthorizationCheckPerformed(identity, taskName);
                }
                catch (Exception) {}
            }
        }

        void FireAuthorizationCheckFailed()
        {
            for (int i = 0; i < numberOfEvents; i++)
            {
                try
                {
                    instrumentationProvider.FireAuthorizationCheckFailed(identity, taskName);
                }
                catch (Exception) {}
            }
        }
    }
}
