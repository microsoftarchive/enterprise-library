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

using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    [TestClass]
    public class GivenFormattedEventLogTraceListenerDataWithFilterData
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData =
                new FormattedEventLogTraceListenerData("listener", "source", "log", "machine", "formatter")
                    {
                        TraceOutputOptions = TraceOptions.DateTime | TraceOptions.Callstack,
                        Filter = SourceLevels.Warning
                    };
        }

        [TestMethod]
        public void ThenCreatesSingleTypeRegistration()
        {
            Assert.AreEqual(1, listenerData.GetContainerConfigurationModel().Count());
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationMapsTraceListenerToFormattedEventLogTraceListenerForTheSuppliedName()
        {
            listenerData.GetContainerConfigurationModel().ElementAt(0)
                .AssertForServiceType(typeof(TraceListener))
                .ForName("listener")
                .ForImplementationType(typeof(FormattedEventLogTraceListener));
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationHasTheExpectedConstructorParameters()
        {
            listenerData.GetContainerConfigurationModel().ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter("source")
                .WithValueConstructorParameter("log")
                .WithValueConstructorParameter("machine")
                .WithContainerResolvedParameter<ILogFormatter>("formatter")
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationInjectsFilterAndNameAndTraceOutputOptionsProperties()
        {
            TraceFilter filter;

            listenerData.GetContainerConfigurationModel().ElementAt(0)
                .AssertProperties()
                .WithValueProperty("Name", "listener")
                .WithValueProperty("TraceOutputOptions", TraceOptions.DateTime | TraceOptions.Callstack)
                .WithValueProperty("Filter", out filter)
                .VerifyProperties();

            Assert.AreEqual(SourceLevels.Warning, ((EventTypeFilter)filter).EventType);
        }
    }
}
