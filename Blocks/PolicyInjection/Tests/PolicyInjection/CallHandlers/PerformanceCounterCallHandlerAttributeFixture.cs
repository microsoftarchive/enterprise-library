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

using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.AttributeDrivenPolicy
{
    [TestClass]
    public class PerformanceCounterCallHandlerAttributeFixture
    {
        [TestMethod]
        public void ShouldCreateDefaultHandlerFromAttribute()
        {
            PerformanceCounterCallHandlerAttribute attribute =
                new PerformanceCounterCallHandlerAttribute("My Category", "My instance");

            PerformanceCounterCallHandler handler = GetHandlerFromAttribute(attribute);

            Assert.AreEqual("My Category", handler.Category);
            Assert.AreEqual("My instance", handler.InstanceName);
            Assert.AreEqual(PerformanceCounterCallHandlerDefaults.IncrementAverageCallDuration,
                            handler.IncrementAverageCallDuration);
            Assert.AreEqual(PerformanceCounterCallHandlerDefaults.IncrementCallsPerSecond,
                            handler.IncrementCallsPerSecond);
            Assert.AreEqual(PerformanceCounterCallHandlerDefaults.IncrementExceptionsPerSecond,
                            handler.IncrementExceptionsPerSecond);
            Assert.AreEqual(PerformanceCounterCallHandlerDefaults.IncrementNumberOfCalls,
                            handler.IncrementNumberOfCalls);
            Assert.AreEqual(PerformanceCounterCallHandlerDefaults.IncrementTotalExceptions,
                            handler.IncrementTotalExceptions);
            Assert.AreEqual(PerformanceCounterCallHandlerDefaults.UseTotalCounter,
                            handler.UseTotalCounter);
        }

        [TestMethod]
        public void ShouldBeAbleToSetAllAttributes()
        {
            PerformanceCounterCallHandlerAttribute attribute =
                new PerformanceCounterCallHandlerAttribute("My Category", "My instance");
            attribute.IncrementAverageCallDuration =
                !PerformanceCounterCallHandlerDefaults.IncrementAverageCallDuration;
            attribute.IncrementCallsPerSecond =
                !PerformanceCounterCallHandlerDefaults.IncrementCallsPerSecond;
            attribute.IncrementExceptionsPerSecond =
                !PerformanceCounterCallHandlerDefaults.IncrementExceptionsPerSecond;
            attribute.IncrementNumberOfCalls =
                !PerformanceCounterCallHandlerDefaults.IncrementNumberOfCalls;
            attribute.IncrementTotalExceptions =
                !PerformanceCounterCallHandlerDefaults.IncrementTotalExceptions;
            attribute.UseTotalCounter =
                !PerformanceCounterCallHandlerDefaults.UseTotalCounter;

            PerformanceCounterCallHandler handler = GetHandlerFromAttribute(attribute);

            Assert.AreEqual(attribute.IncrementAverageCallDuration,
                            handler.IncrementAverageCallDuration);

            Assert.AreEqual(attribute.IncrementCallsPerSecond,
                            handler.IncrementCallsPerSecond);
            Assert.AreEqual(attribute.IncrementExceptionsPerSecond,
                            handler.IncrementExceptionsPerSecond);
            Assert.AreEqual(attribute.IncrementNumberOfCalls,
                            handler.IncrementNumberOfCalls);
            Assert.AreEqual(attribute.IncrementTotalExceptions,
                            handler.IncrementTotalExceptions);
            Assert.AreEqual(attribute.UseTotalCounter,
                            handler.UseTotalCounter);
        }

        PerformanceCounterCallHandler GetHandlerFromAttribute(HandlerAttribute attribute)
        {
            return (PerformanceCounterCallHandler)attribute.CreateHandler(null);
        }
    }
}
