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
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestClass]
    public class AuthorizationInstrumentationFixture
    {
        AuthorizationProviderInstrumentationProvider enabledInstrumentationProvider;
        AuthorizationProviderInstrumentationProvider disabledInstrumentationProvider;
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
            enabledInstrumentationProvider = new AuthorizationProviderInstrumentationProvider(instanceName, true, true, formatter);
            disabledInstrumentationProvider = new AuthorizationProviderInstrumentationProvider(instanceName, false, false, formatter);
            totalAuthorizationCheckFailedCounter = new EnterpriseLibraryPerformanceCounter(AuthorizationProviderInstrumentationProvider.PerformanceCountersCategoryName, AuthorizationProviderInstrumentationProvider.TotalAuthorizationCheckFailedCounterName, formattedInstanceName);
            totalAuthorizationCheckPerformedCounter = new EnterpriseLibraryPerformanceCounter(AuthorizationProviderInstrumentationProvider.PerformanceCountersCategoryName, AuthorizationProviderInstrumentationProvider.TotalAuthorizationCheckPerformedCounterName, formattedInstanceName);

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
            enabledInstrumentationProvider.FireAuthorizationCheckPerformed("foo", "bar");

            Assert.AreEqual(1, totalAuthorizationCheckPerformedCounter.Value);
        }

        [TestMethod]
        public void TotalAuthorizationCheckFailedCounterIncremented()
        {
            enabledInstrumentationProvider.FireAuthorizationCheckFailed("foo", "bar");

            Assert.AreEqual(1, totalAuthorizationCheckFailedCounter.Value);
        }

        [TestMethod]
        public void AuthorizationCheckDoesUpdatePerformanceCountersIfEnabled()
        {
            EnterpriseLibraryPerformanceCounter performanceCounter
                = CreatePerformanceCounter(AuthorizationProviderInstrumentationProvider.AuthorizationCheckPerformedCounterName);
            performanceCounter.Clear();
            Assert.AreEqual(0L, performanceCounter.GetValueFor(formattedInstanceName));

            FireAuthorizationCheckPerformed(enabledInstrumentationProvider);

            // Timing dependant
            Assert.AreEqual(50L, performanceCounter.GetValueFor(formattedInstanceName));
        }

        [TestMethod]
        public void AuthorizationCheckDoesNotUpdatePerformanceCountersIfDisabled()
        {
            EnterpriseLibraryPerformanceCounter performanceCounter
                = CreatePerformanceCounter(AuthorizationProviderInstrumentationProvider.AuthorizationCheckPerformedCounterName);
            performanceCounter.Clear();
            Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);

            FireAuthorizationCheckPerformed(disabledInstrumentationProvider);

            // Timing dependant
            Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);
        }

        [TestMethod]
        public void AuthorizationFailureDoesUpdatePerformanceCountersIfEnabled()
        {
            EnterpriseLibraryPerformanceCounter performanceCounter
                = CreatePerformanceCounter(AuthorizationProviderInstrumentationProvider.AuthorizationCheckFailedCounterName);
            performanceCounter.Clear();
            Assert.AreEqual(0L, performanceCounter.GetValueFor(formattedInstanceName));

            FireAuthorizationCheckFailed(enabledInstrumentationProvider);

            // Timing dependant
            Assert.AreEqual(50L, performanceCounter.GetValueFor(formattedInstanceName));
        }

        [TestMethod]
        public void AuthorizationFailureDoesNotUpdatePerformanceCountersIfDisabled()
        {
            EnterpriseLibraryPerformanceCounter performanceCounter
                = CreatePerformanceCounter(AuthorizationProviderInstrumentationProvider.AuthorizationCheckFailedCounterName);
            performanceCounter.Clear();
            Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);

            FireAuthorizationCheckFailed(disabledInstrumentationProvider);

            // Timing dependant
            Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);
        }

        EnterpriseLibraryPerformanceCounter CreatePerformanceCounter(string counterName)
        {
            return new EnterpriseLibraryPerformanceCounter(
                AuthorizationProviderInstrumentationProvider.PerformanceCountersCategoryName,
                counterName,
                formattedInstanceName);
        }

        void FireAuthorizationCheckPerformed(IAuthorizationProviderInstrumentationProvider instrumentationProvider)
        {
            for (int i = 0; i < numberOfEvents; i++)
            {
                try
                {
                    instrumentationProvider.FireAuthorizationCheckPerformed(identity, taskName);
                }
                catch (Exception) { }
            }
        }

        void FireAuthorizationCheckFailed(IAuthorizationProviderInstrumentationProvider instrumentationProvider)
        {
            for (int i = 0; i < numberOfEvents; i++)
            {
                try
                {
                    instrumentationProvider.FireAuthorizationCheckFailed(identity, taskName);
                }
                catch (Exception) { }
            }
        }
    }
}
