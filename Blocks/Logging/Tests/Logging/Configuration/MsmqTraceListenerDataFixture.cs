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
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    [TestClass]
    public class GivenMsmqTraceListenerDataWithFilterData
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData =
                new MsmqTraceListenerData(
                    "listener",
                    "queue path",
                    "formatter",
                    MessagePriority.High,
                    true,
                    TimeSpan.MinValue,
                    TimeSpan.MaxValue,
                    false,
                    true,
                    false,
                    MessageQueueTransactionType.Automatic)
                    {
                        TraceOutputOptions = TraceOptions.DateTime | TraceOptions.Callstack,
                        Filter = SourceLevels.Warning
                    };
        }

        [TestMethod]
        public void ThenCreatesTwoTypeRegistrations()
        {
            Assert.AreEqual(2, listenerData.GetRegistrations().Count());
        }

        [TestMethod]
        public void ThenCreatesATypeRegistrationForTheWrapperWithTheOriginalName()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener").First()
                .AssertForServiceType(typeof(TraceListener))
                .ForName("listener")
                .ForImplementationType(typeof(ReconfigurableTraceListenerWrapper));
        }

        [TestMethod]
        public void ThenWrapperRegistrationIsSingleton()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Singleton,
                listenerData.GetRegistrations().Where(tr => tr.Name == "listener").First().Lifetime);
        }

        [TestMethod]
        public void ThenWrapperRegistrationIsInjectedWithTheWrappedTraceListenerAndTheLoggingUpdateCoordinator()
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
        public void WhenCreatesRegistration_ThenCreatedRegistrationMapsTraceListenerToMsmqTraceListenerForTheSuppliedName()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First()
                .AssertForServiceType(typeof(TraceListener))
                .ForName("listener\u200Cimplementation")
                .ForImplementationType(typeof(MsmqTraceListener));
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationHasTheExpectedConstructorParameters()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First()
                .AssertConstructor()
                .WithValueConstructorParameter("listener")
                .WithValueConstructorParameter("queue path")
                .WithContainerResolvedParameter<ILogFormatter>("formatter")
                .WithValueConstructorParameter(MessagePriority.High)
                .WithValueConstructorParameter(true)
                .WithValueConstructorParameter(TimeSpan.MinValue)
                .WithValueConstructorParameter(TimeSpan.MaxValue)
                .WithValueConstructorParameter(false)
                .WithValueConstructorParameter(true)
                .WithValueConstructorParameter(false)
                .WithValueConstructorParameter(MessageQueueTransactionType.Automatic)
                .WithContainerResolvedParameter<ILoggingInstrumentationProvider>(null)
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
        public void WhenCreatesRegistrations_ThenWrappedListenerRegistrationIsTransient()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Transient,
                listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First().Lifetime);
        }
    }
}
