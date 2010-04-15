//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionHandlerInstrumentationProviderFixture
    {
        const string policyName = "policy";
        const string exceptionMessage = "exception message";
        const string counterCategoryName = "Enterprise Library Exception Handling Counters";
        const string TotalExceptionHandlersExecuted = "Total Exception Handlers Executed";
        const string TotalExceptionsHandled = "Total Exceptions Handled";
        const string instanceName = "Foo";
        string formattedInstanceName;

        IExceptionHandlingInstrumentationProvider provider;
        IPerformanceCounterNameFormatter nameFormatter;
        EnterpriseLibraryPerformanceCounter totalExceptionHandlersExecuted;
        EnterpriseLibraryPerformanceCounter totalExceptionsHandled;

        ExceptionPolicyImpl exceptionPolicy;

        [TestInitialize]
        public void SetUp()
        {
            var handlers = new IExceptionHandler[] { new MockThrowingExceptionHandler() };

            var instrumentationProvider = new ExceptionHandlingInstrumentationProvider(policyName, true, true,
                                                                                          "ApplicationInstanceName");
            var policyEntry = new ExceptionPolicyEntry(typeof(ArgumentException), PostHandlingAction.None, handlers, instrumentationProvider);
            var policyEntries = new Dictionary<Type, ExceptionPolicyEntry>
            {
                {typeof (ArgumentException), policyEntry}
            };

            exceptionPolicy = new ExceptionPolicyImpl(policyName, policyEntries);

            nameFormatter = new FixedPrefixNameFormatter("Prefix - ");
            provider = new ExceptionHandlingInstrumentationProvider(instanceName, true, true, nameFormatter);
            formattedInstanceName = nameFormatter.CreateName(instanceName);
            totalExceptionHandlersExecuted = new EnterpriseLibraryPerformanceCounter(counterCategoryName, TotalExceptionHandlersExecuted, formattedInstanceName);
            totalExceptionsHandled = new EnterpriseLibraryPerformanceCounter(counterCategoryName, TotalExceptionsHandled, formattedInstanceName);
        }

        [TestMethod]
        public void TotalExceptionHandlersExecutedCounterIncremented()
        {
            provider.FireExceptionHandlerExecutedEvent();

            Assert.AreEqual(1, totalExceptionHandlersExecuted.Value);
        }

        [TestMethod]
        public void TotalExceptionsHandledCounterIncremented()
        {
            provider.FireExceptionHandledEvent();
            Assert.AreEqual(1, totalExceptionsHandled.Value);
        }

        [TestMethod]
        public void FailureHandlingExceptionWritesToEventLog()
        {
            var exception = new ArgumentException(exceptionMessage);
            DateTime testStartTime = DateTime.Now;
            Thread.Sleep(1500); // Log granularity is to the second, force us to the next second

            using (var eventLog = GetEventLog())
            {
                try
                {
                    exceptionPolicy.HandleException(exception);
                }
                catch (ExceptionHandlingException) { }

                var entries = eventLog.GetEntriesSince(testStartTime)
                    .Where(entry => entry.Message.IndexOf(exceptionMessage) > -1);

                Assert.AreEqual(1, entries.Count());
            }
        }

        static EventLog GetEventLog()
        {
            return new EventLog("Application", ".", "Enterprise Library ExceptionHandling");
        }

        public class FixedPrefixNameFormatter : IPerformanceCounterNameFormatter
        {
            readonly string prefix;

            public FixedPrefixNameFormatter(string prefix)
            {
                this.prefix = prefix;
            }

            public string CreateName(string nameSuffix)
            {
                return prefix + nameSuffix;
            }
        }
    }
}
