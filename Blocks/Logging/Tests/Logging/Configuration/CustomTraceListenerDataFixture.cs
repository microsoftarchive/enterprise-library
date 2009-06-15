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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenCustomTraceListenerDataWithInitializationDataForNonCustomTraceListenerType
    {
        private CustomTraceListenerData listenerData;

        [TestInitialize]
        public void Given()
        {
            listenerData =
                new CustomTraceListenerData(
                     "custom trace listener",
                     typeof(System.Diagnostics.TextWriterTraceListener),
                     "someInitData")
                {
                    Formatter = "formatter"
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
            listenerData.GetRegistrations().Where(tr => tr.Name == "custom trace listener").First()
                .AssertForServiceType(typeof(TraceListener))
                .ForName("custom trace listener")
                .ForImplementationType(typeof(ReconfigurableTraceListenerWrapper));
        }

        [TestMethod]
        public void ThenWrapperRegistrationIsSingleton()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Singleton,
                listenerData.GetRegistrations().Where(tr => tr.Name == "custom trace listener").First().Lifetime);
        }

        [TestMethod]
        public void ThenWrapperRegistrationIsInjectedWithTheWrappedTraceListenerAndTheLoggingUpdateCoordinator()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "custom trace listener").First()
                .AssertConstructor()
                .WithContainerResolvedParameter<TraceListener>("custom trace listener\u200Cimplementation")
                .WithContainerResolvedParameter<ILoggingUpdateCoordinator>(null)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenRegistrationIsForCorrectServiceAndType()
        {
            TypeRegistration registry =
                listenerData.GetRegistrations().Where(tr => tr.Name == "custom trace listener\u200Cimplementation").First();

            registry.AssertForServiceType(typeof(TraceListener))
                .ForName("custom trace listener\u200Cimplementation")
                .ForImplementationType(typeof(System.Diagnostics.TextWriterTraceListener));
        }

        [TestMethod]
        public void ThenRegistrationTargetsConstructorWithInitialData()
        {
            TypeRegistration registry =
                listenerData.GetRegistrations().Where(tr => tr.Name == "custom trace listener\u200Cimplementation").First();

            registry.AssertConstructor()
                .WithValueConstructorParameter<string>("someInitData")
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenRegistryEntryIncludesPropertyForTraceOptions()
        {
            TypeRegistration registry =
                listenerData.GetRegistrations().Where(tr => tr.Name == "custom trace listener\u200Cimplementation").First();

            registry.AssertProperties()
                .WithValueProperty("TraceOutputOptions", TraceOptions.None)
                .WithValueProperty("Name", "custom trace listener\u200Cimplementation")
                .VerifyProperties();
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCreatedRegistrationsAreSingleton()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Transient,
                listenerData.GetRegistrations().Where(tr => tr.Name == "custom trace listener\u200Cimplementation").First().Lifetime);
        }
    }

    [TestClass]
    public class GivenCustomTraceListenerDataWithInitializationDataForCustomTraceListenerType
    {
        private CustomTraceListenerData listenerData;

        [TestInitialize]
        public void Given()
        {
            listenerData =
                new CustomTraceListenerData(
                     "custom trace listener",
                     typeof(MockCustomTraceListener),
                     "someInitData")
                {
                    Formatter = "formatter"
                };
        }

        [TestMethod]
        public void ThenRegistrationIsForCorrectServiceAndType()
        {
            TypeRegistration registry =
                listenerData.GetRegistrations().First(tr => tr.Name == "custom trace listener\u200Cimplementation");

            registry.AssertForServiceType(typeof(TraceListener))
                .ForName("custom trace listener\u200Cimplementation")
                .ForImplementationType(typeof(MockCustomTraceListener));
        }

        [TestMethod]
        public void ThenRegistrationTargetsConstructorWithInitialData()
        {
            TypeRegistration registry =
                listenerData.GetRegistrations().First(tr => tr.Name == "custom trace listener\u200Cimplementation");

            registry.AssertConstructor()
                .WithValueConstructorParameter<string>("someInitData")
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenRegistryEntryIncludesPropertyForTraceOptions()
        {
            TypeRegistration registry =
                listenerData.GetRegistrations().First(tr => tr.Name == "custom trace listener\u200Cimplementation");

            registry.AssertProperties()
                .WithValueProperty("TraceOutputOptions", TraceOptions.None)
                .WithValueProperty("Name", "custom trace listener\u200Cimplementation")
                .WithContainerResolvedProperty<ILogFormatter>("Formatter", "formatter")
                .VerifyProperties();
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenWrappedRegistrationIsTransient()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Transient,
                listenerData.GetRegistrations().First(tr => tr.Name == "custom trace listener\u200Cimplementation").Lifetime);
        }
    }

    [TestClass]
    public class GivenCustomTraceListenerDataWithInitializationDataWithEmptyFormatterNameForCustomTraceListenerType
    {
        private CustomTraceListenerData listenerData;

        [TestInitialize]
        public void Given()
        {
            listenerData =
                new CustomTraceListenerData(
                     "custom trace listener",
                     typeof(MockCustomTraceListener),
                     "someInitData")
                {
                    Formatter = string.Empty
                };
        }

        [TestMethod]
        public void ThenRegistrationIsForCorrectServiceAndType()
        {
            TypeRegistration registry =
                listenerData.GetRegistrations().First(tr => tr.Name == "custom trace listener\u200Cimplementation");

            registry.AssertForServiceType(typeof(TraceListener))
                .ForName("custom trace listener\u200Cimplementation")
                .ForImplementationType(typeof(MockCustomTraceListener));
        }

        [TestMethod]
        public void ThenRegistrationTargetsConstructorWithInitialData()
        {
            TypeRegistration registry =
                listenerData.GetRegistrations().First(tr => tr.Name == "custom trace listener\u200Cimplementation");

            registry.AssertConstructor()
                .WithValueConstructorParameter<string>("someInitData")
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenRegistryEntryIncludesPropertyForTraceOptions()
        {
            TypeRegistration registry =
                listenerData.GetRegistrations().First(tr => tr.Name == "custom trace listener\u200Cimplementation");

            registry.AssertProperties()
                .WithValueProperty("TraceOutputOptions", TraceOptions.None)
                .WithValueProperty("Name", "custom trace listener\u200Cimplementation")
                .VerifyProperties();
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenWrappedRegistrationIsTransient()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Transient,
                listenerData.GetRegistrations().First(tr => tr.Name == "custom trace listener\u200Cimplementation").Lifetime);
        }
    }

    [TestClass]
    public class GivenCustomTraceListenerWithoutExpectedConstructor
    {
        private CustomTraceListenerData listenerData;

        [TestInitialize]
        public void Given()
        {
            listenerData = new CustomTraceListenerData("someName",
                                                       typeof(MockCustomTraceListenerWithoutExpectedConstructor),
                                                       "initData");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenRegistrationsRetrieved_ThenThrowsException()
        {
            var registrations = listenerData.GetRegistrations().ToArray();
        }
    }

    internal class MockCustomTraceListenerWithoutExpectedConstructor : CustomTraceListener
    {
        public MockCustomTraceListenerWithoutExpectedConstructor()
        {
        }

        public override void Write(string message)
        {
            throw new System.NotImplementedException();
        }

        public override void WriteLine(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}

