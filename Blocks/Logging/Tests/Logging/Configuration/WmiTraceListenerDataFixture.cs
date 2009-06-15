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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenWmiTraceListenerDataWithNoFilterData
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData = new WmiTraceListenerData("listener");
        }

        [TestMethod]
        public void ThenCreatesTwoTypeRegistrations()
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
        public void WhenCreatesRegistration_ThenCreatedRegistrationMapsTraceListenerToWmiTraceListenerForTheSuppliedName()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First()
                .AssertForServiceType(typeof(TraceListener))
                .ForName("listener\u200Cimplementation")
                .ForImplementationType(typeof(WmiTraceListener));
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationHasNoConstructorParameters()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First()
                .AssertConstructor()
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationInjectsNameAndTraceOutputOptionsProperties()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First()
                .AssertProperties()
                .WithValueProperty("Name", "listener\u200Cimplementation")
                .WithValueProperty("TraceOutputOptions", TraceOptions.None)
                .VerifyProperties();
        }

        [TestMethod]
        public void ThenWrappedRegistrationIsTransient()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Transient,
                listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First().Lifetime);
        }
    }

    [TestClass]
    public class GivenWmiTraceListenerWithTraceOptions
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Given()
        {
            listenerData = new WmiTraceListenerData("listener") { TraceOutputOptions = TraceOptions.Callstack | TraceOptions.ProcessId };
        }

        [TestMethod]
        public void ThenRegistrationIncludesInitializationsForTraceOutputOptions()
        {
            TypeRegistration registration = 
                listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First();

            registration
                .AssertProperties()
                .WithValueProperty("Name", "listener\u200Cimplementation")
                .WithValueProperty("TraceOutputOptions", listenerData.TraceOutputOptions)
                .VerifyProperties();
        }
    }

    [TestClass]
    public class GivenWmiTraceListenerDataWithFilterData
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData = new WmiTraceListenerData("listener") { Filter = SourceLevels.Warning };
        }

        [TestMethod]
        public void ThenCreatesTwoTypeRegistrations()
        {
            Assert.AreEqual(2, listenerData.GetRegistrations().Count());
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationMapsTraceListenerToWmiTraceListenerForTheSuppliedName()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First()
                .AssertForServiceType(typeof(TraceListener))
                .ForName("listener\u200Cimplementation")
                .ForImplementationType(typeof(WmiTraceListener));
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationHasNoConstructorParameters()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First()
                .AssertConstructor()
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationInjectsFilterAndNameAndTraceOutputOptionsProperties()
        {
            TraceFilter filter;

            listenerData.GetRegistrations().Where(tr => tr.Name == "listener\u200Cimplementation").First()
                .AssertProperties()
                .WithValueProperty("Name", "listener\u200Cimplementation")
                .WithValueProperty("TraceOutputOptions", TraceOptions.None)
                .WithValueProperty("Filter", out filter)
                .VerifyProperties();

            Assert.AreEqual(SourceLevels.Warning, ((EventTypeFilter)filter).EventType);
        }
    }
}
