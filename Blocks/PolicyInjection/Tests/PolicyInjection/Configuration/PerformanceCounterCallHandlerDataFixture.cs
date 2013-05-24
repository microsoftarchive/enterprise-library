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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    public class PerformanceCounterCallHandlerDataFixture : CallHandlerDataFixtureBase
    {
        [TestMethod]
        public void DataClassHasCorrectDefaults()
        {
            PerformanceCounterCallHandlerData data =
                new PerformanceCounterCallHandlerData("Counter data");

            Assert.AreEqual(
                PerformanceCounterCallHandlerDefaults.UseTotalCounter,
                data.UseTotalCounter);
            Assert.AreEqual(
                PerformanceCounterCallHandlerDefaults.IncrementAverageCallDuration,
                data.IncrementAverageCallDuration);
            Assert.AreEqual(
                PerformanceCounterCallHandlerDefaults.IncrementCallsPerSecond,
                data.IncrementCallsPerSecond);
            Assert.AreEqual(
                PerformanceCounterCallHandlerDefaults.IncrementExceptionsPerSecond,
                data.IncrementExceptionsPerSecond);
            Assert.AreEqual(
                PerformanceCounterCallHandlerDefaults.IncrementNumberOfCalls,
                data.IncrementNumberOfCalls);
            Assert.AreEqual(
                PerformanceCounterCallHandlerDefaults.IncrementTotalExceptions,
                data.IncrementTotalExceptions);

            Assert.AreEqual(0, data.Order);
        }

        [TestMethod]
        public void CanDeserializeCallHandlerData()
        {
            PerformanceCounterCallHandlerData data = new PerformanceCounterCallHandlerData("counter data");
            data.CategoryName = "My Category";
            data.InstanceName = "Method - {namespace}.{type}.{method}";
            data.UseTotalCounter = false;
            data.Order = 10;

            PerformanceCounterCallHandlerData deserialized =
                (PerformanceCounterCallHandlerData)SerializeAndDeserializeHandler(data);

            Assert.AreEqual(data.Name, deserialized.Name);
            Assert.AreEqual(data.CategoryName, deserialized.CategoryName);
            Assert.AreEqual(data.InstanceName, deserialized.InstanceName);
            Assert.AreEqual(data.Order, deserialized.Order);
        }
    }

    [TestClass]
    public class GivenAPerformanceCounterCallHandlerData
    {
        private CallHandlerData callHandlerData;

        [TestInitialize]
        public void Setup()
        {
            callHandlerData =
                new PerformanceCounterCallHandlerData("perf counter")
                    {
                        Order = 300,
                        CategoryName = "category",
                        InstanceName = "instance",
                        UseTotalCounter = true,
                        IncrementNumberOfCalls = false,
                        IncrementCallsPerSecond = true,
                        IncrementAverageCallDuration = false,
                        IncrementTotalExceptions = true,
                        IncrementExceptionsPerSecond = false
                    };
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenCanResolveCallHandler()
        {
            using (var container = new UnityContainer())
            {
                this.callHandlerData.ConfigureContainer(container, "-suffix");

                var handler = (PerformanceCounterCallHandler)container.Resolve<ICallHandler>("perf counter-suffix");

                Assert.AreEqual(300, handler.Order);
                Assert.AreEqual("category", handler.Category);
                Assert.AreEqual("instance", handler.InstanceName);
                Assert.AreEqual(true, handler.UseTotalCounter);
                Assert.AreEqual(false, handler.IncrementNumberOfCalls);
                Assert.AreEqual(true, handler.IncrementCallsPerSecond);
                Assert.AreEqual(false, handler.IncrementAverageCallDuration);
                Assert.AreEqual(true, handler.IncrementTotalExceptions);
                Assert.AreEqual(false, handler.IncrementExceptionsPerSecond);

                Assert.AreNotSame(handler, container.Resolve<ICallHandler>("perf counter-suffix"));
            }
        }
    }
}
