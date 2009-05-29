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

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation.Tests
{
    [TestClass]
    public class DataInstrumentationProviderPerfCounterFixture
    {
        private NewDataInstrumentationProvider provider;

        const string instanceName = "Foo";
        string formattedInstanceName;

        EnterpriseLibraryPerformanceCounter totalConnectionOpenedCounter;
        EnterpriseLibraryPerformanceCounter totalConnectionFailedCounter;
        EnterpriseLibraryPerformanceCounter totalCommandsExecutedCounter;
        EnterpriseLibraryPerformanceCounter totalCommandsFailedCounter;

        IPerformanceCounterNameFormatter nameFormatter;

        [TestInitialize]
        public void SetUp()
        {
            nameFormatter = new FixedPrefixNameFormatter("Prefix - ");
            formattedInstanceName = nameFormatter.CreateName(instanceName);
            totalConnectionOpenedCounter = new EnterpriseLibraryPerformanceCounter(
                NewDataInstrumentationProvider.CounterCategoryName,
                NewDataInstrumentationProvider.TotalConnectionOpenedCounter, 
                formattedInstanceName);
            totalConnectionFailedCounter = new EnterpriseLibraryPerformanceCounter(
                NewDataInstrumentationProvider.CounterCategoryName,
                NewDataInstrumentationProvider.TotalConnectionFailedCounter,
                formattedInstanceName);
            totalCommandsExecutedCounter = new EnterpriseLibraryPerformanceCounter(
                NewDataInstrumentationProvider.CounterCategoryName, 
                NewDataInstrumentationProvider.TotalCommandsExecutedCounter,
                formattedInstanceName);
            totalCommandsFailedCounter = new EnterpriseLibraryPerformanceCounter(
                NewDataInstrumentationProvider.CounterCategoryName,
                NewDataInstrumentationProvider.TotalCommandsFailedCounter,
                formattedInstanceName);

            provider = new NewDataInstrumentationProvider(instanceName, true, true, true, nameFormatter);

            ClearExistingCounts();
        }

        [TestMethod]
        public void TotalConnectionOpenedCounterIncremented()
        {
            provider.FireConnectionOpenedEvent();

            Assert.AreEqual(1, totalConnectionOpenedCounter.Value);
        }
        
        [TestMethod]
        public void InstanceConnectionOpenedCounterIncremented()
        {
            provider.FireConnectionOpenedEvent();
            Assert.AreEqual(1, totalConnectionOpenedCounter.GetValueFor(formattedInstanceName));
        }

        [TestMethod]
        public void TotalConnectionFailedCounterIncremented()
        {
            provider.FireConnectionFailedEvent("BadConnectionString", new Exception());
            Assert.AreEqual(1, totalConnectionFailedCounter.Value);
        }

        [TestMethod]
        public void InstanceConnectionFailedCounterIncremented()
        {
            provider.FireConnectionFailedEvent("BadConnectionString", new Exception());
            Assert.AreEqual(1, totalConnectionFailedCounter.GetValueFor(formattedInstanceName));
        }

        [TestMethod]
        public void TotalCommandExecutedCounterIncremented()
        {
            provider.FireCommandExecutedEvent(DateTime.Now);
            provider.FireCommandExecutedEvent(DateTime.Now);

            Assert.AreEqual(2, totalCommandsExecutedCounter.Value);
        }

        [TestMethod]
        public void InstanceCommandExecutedCounterIncremented()
        {
            provider.FireCommandExecutedEvent(DateTime.Now);
            provider.FireCommandExecutedEvent(DateTime.Now);

            Assert.AreEqual(2, totalCommandsExecutedCounter.GetValueFor(formattedInstanceName));
        }

        [TestMethod]
        public void TotalCommandFailedCounterIncremented()
        {
            const long maxCount = 5;
            for (long i = 0; i < maxCount; i++)
            {
                provider.FireCommandFailedEvent("fooCommand", "badConnection", new Exception());
            }

            Assert.AreEqual(maxCount, totalCommandsFailedCounter.Value);
        }

        [TestMethod]
        public void InstanceTotalCommandFailedCounterIncremented()
        {
            const long maxCount = 5;
            for (long i = 0; i < maxCount; i++)
            {
                provider.FireCommandFailedEvent("fooCommand", "badConnection", new Exception());
            }

            Assert.AreEqual(maxCount, totalCommandsFailedCounter.GetValueFor(formattedInstanceName));
        }

        [TestMethod]
        public void IncrementsMultipleInstancesIndependently()
        {
            string firstInstanceName = "first";
            string secondInstanceName = "second";

            FixedPrefixNameFormatter formatter = new FixedPrefixNameFormatter("Baz - ");

            var connectionOpenedCounter = new EnterpriseLibraryPerformanceCounter(
                NewDataInstrumentationProvider.CounterCategoryName,
                NewDataInstrumentationProvider.TotalConnectionOpenedCounter,
                formatter.CreateName(firstInstanceName), 
                formatter.CreateName(secondInstanceName));

            var firstProvider = new NewDataInstrumentationProvider(firstInstanceName, true, true, true, formatter);
            var secondProvider = new NewDataInstrumentationProvider(secondInstanceName, true, true, true, formatter);

            firstProvider.FireConnectionOpenedEvent();

            Assert.AreEqual(1, connectionOpenedCounter.GetValueFor(formatter.CreateName(firstInstanceName)));
            Assert.AreEqual(0, connectionOpenedCounter.GetValueFor(formatter.CreateName(secondInstanceName)));

            secondProvider.FireConnectionOpenedEvent();

            Assert.AreEqual(1, connectionOpenedCounter.GetValueFor(formatter.CreateName(firstInstanceName)));
            Assert.AreEqual(1, connectionOpenedCounter.GetValueFor(formatter.CreateName(secondInstanceName)));
        }

        void ClearExistingCounts()
        {
            totalConnectionOpenedCounter.Clear();
            totalConnectionFailedCounter.Clear();
            totalCommandsExecutedCounter.Clear();
            totalCommandsFailedCounter.Clear();
        }
    }
}
