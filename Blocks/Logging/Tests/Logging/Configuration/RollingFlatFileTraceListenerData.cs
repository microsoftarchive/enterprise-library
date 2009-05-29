//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenRollingFlatFileTraceListenerDataWithFilterData
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData =
                new RollingFlatFileTraceListenerData(
                    "listener",
                    "file name",
                    "header",
                    "footer",
                    100,
                    "timestamp pattern",
                    RollFileExistsBehavior.Increment,
                    RollInterval.Day,
                    TraceOptions.DateTime | TraceOptions.Callstack,
                    "formatter")
                {
                    Filter = SourceLevels.Warning
                };
        }

        [TestMethod]
        public void ThenCreatesSingleTypeRegistration()
        {
            Assert.AreEqual(1, listenerData.GetRegistrations().Count());
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationMapsTraceListenerToRollingFlatFileTraceListenerForTheSuppliedName()
        {
            listenerData.GetRegistrations().ElementAt(0)
                .AssertForServiceType(typeof(TraceListener))
                .ForName("listener")
                .ForImplementationType(typeof(RollingFlatFileTraceListener));
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationHasTheExpectedConstructorParameters()
        {
            listenerData.GetRegistrations().ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter("file name")
                .WithValueConstructorParameter("header")
                .WithValueConstructorParameter("footer")
                .WithContainerResolvedParameter<ILogFormatter>("formatter")
                .WithValueConstructorParameter(100)
                .WithValueConstructorParameter("timestamp pattern")
                .WithValueConstructorParameter(RollFileExistsBehavior.Increment)
                .WithValueConstructorParameter(RollInterval.Day)
                .WithContainerResolvedParameter<ILoggingInstrumentationProvider>(null)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationInjectsFilterAndNameAndTraceOutputOptionsProperties()
        {
            TraceFilter filter;

            listenerData.GetRegistrations().ElementAt(0)
                .AssertProperties()
                .WithValueProperty("Name", "listener")
                .WithValueProperty("TraceOutputOptions", TraceOptions.DateTime | TraceOptions.Callstack)
                .WithValueProperty("Filter", out filter)
                .VerifyProperties();

            Assert.AreEqual(SourceLevels.Warning, ((EventTypeFilter)filter).EventType);
        }
    }
}
