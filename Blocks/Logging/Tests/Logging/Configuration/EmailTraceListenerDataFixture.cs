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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    [TestClass]
    public class GivenEmailTraceListenerDataWithFilterData
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData =
                new EmailTraceListenerData("listener", "to", "from", "subject starter", "subject ender", "smtp", 25, "formatter")
                {
                    TraceOutputOptions = TraceOptions.DateTime | TraceOptions.Callstack,
                    Filter = SourceLevels.Warning
                };
        }
        

        [TestMethod]
        public void WhenCreatingInstanceUsingDefaultContructor_ThenListenerDataTypeIsSet()
        {
            var listener = new EmailTraceListenerData();
            Assert.AreEqual(typeof(EmailTraceListenerData), listener.ListenerDataType);
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatesTwoTypeRegistrations()
        {
            Assert.AreEqual(2, listenerData.GetRegistrations().Count());
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatesATypeRegistrationForTheWrapperWithTheOriginalName()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener").First()
                .AssertForServiceType(typeof(TraceListener))
                .ForName("listener")
                .ForImplementationType(typeof(ReconfigurableTraceListenerWrapper));
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenWrapperRegistrationIsSingleton()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Singleton,
                listenerData.GetRegistrations().Where(tr => tr.Name == "listener").First().Lifetime);
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenWrapperRegistrationIsInjectedWithTheWrappedTraceListenerAndTheLoggingUpdateCoordinator()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener").First()
                .AssertConstructor()
                .WithContainerResolvedParameter<TraceListener>("listener\u200Cimplementation")
                .WithContainerResolvedParameter<ILoggingUpdateCoordinator>(null)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenWrapperRegistrationIsInjectedWithTheNameProperty()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener").First()
                .AssertProperties()
                .WithValueProperty("Name", "listener")
                .VerifyProperties();
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationMapsTraceListenerToEmailTraceListenerForTheSuppliedName()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First()
                .AssertForServiceType(typeof(TraceListener))
                .ForName("listener\u200Cimplementation")
                .ForImplementationType(typeof(EmailTraceListener));
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationHasTheExpectedConstructorParameters()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First()
                .AssertConstructor()
                .WithValueConstructorParameter("to")
                .WithValueConstructorParameter("from")
                .WithValueConstructorParameter("subject starter")
                .WithValueConstructorParameter("subject ender")
                .WithValueConstructorParameter("smtp")
                .WithValueConstructorParameter(25)
                .WithContainerResolvedParameter<ILogFormatter>("formatter")
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationInjectsFilterAndNameAndTraceOutputOptionsProperties()
        {
            TraceFilter filter;

            listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First()
                .AssertProperties()
                .WithValueProperty("Name", "listener\u200Cimplementation")
                .WithValueProperty("TraceOutputOptions", TraceOptions.DateTime | TraceOptions.Callstack)
                .WithValueProperty("Filter", out filter)
                .VerifyProperties();

            Assert.AreEqual(SourceLevels.Warning, ((EventTypeFilter)filter).EventType);
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenWrappedRegistrationIsTransient()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Transient,
                listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First().Lifetime);
        }
    }
}
