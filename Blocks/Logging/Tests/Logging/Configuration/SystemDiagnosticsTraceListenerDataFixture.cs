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

using System.Collections.Specialized;
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
    public class GivenSystemTraceListenerWithNoInitializationData
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Given()
        {
            listenerData = new SystemDiagnosticsTraceListenerData(
                "systemDiagnosticsTraceListener",
                typeof(System.Diagnostics.TextWriterTraceListener),
                string.Empty
                );
        }

        [TestMethod]
        public void WhenCreatingInstanceUsingDefaultContructor_ThenListenerDataTypeIsSet()
        {
            var listener = new SystemDiagnosticsTraceListenerData();
            Assert.AreEqual(typeof(SystemDiagnosticsTraceListenerData), listener.ListenerDataType);
        }

        [TestMethod]
        public void ThenCreatesTwoTypeRegistrations()
        {
            Assert.AreEqual(2, listenerData.GetRegistrations().Count());
        }

        [TestMethod]
        public void ThenCreatesATypeRegistrationForTheWrapperWithTheOriginalName()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "systemDiagnosticsTraceListener").First()
                .AssertForServiceType(typeof(TraceListener))
                .ForName("systemDiagnosticsTraceListener")
                .ForImplementationType(typeof(ReconfigurableTraceListenerWrapper));
        }

        [TestMethod]
        public void ThenWrapperRegistrationIsSingleton()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Singleton,
                listenerData.GetRegistrations().Where(tr => tr.Name == "systemDiagnosticsTraceListener").First().Lifetime);
        }

        [TestMethod]
        public void ThenWrapperRegistrationIsInjectedWithTheWrappedTraceListenerAndTheLoggingUpdateCoordinator()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "systemDiagnosticsTraceListener").First()
                .AssertConstructor()
                .WithContainerResolvedParameter<TraceListener>("systemDiagnosticsTraceListener\u200Cimplementation")
                .WithContainerResolvedParameter<ILoggingUpdateCoordinator>(null)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenRegistryEntryReturnsNamedServiceEntry()
        {
            TypeRegistration registry =
                listenerData.GetRegistrations().Where(tr => tr.Name == "systemDiagnosticsTraceListener\u200Cimplementation").First();

            registry.AssertForServiceType(typeof(TraceListener))
                .ForName("systemDiagnosticsTraceListener\u200Cimplementation")
                .ForImplementationType(typeof(System.Diagnostics.TextWriterTraceListener));
        }

        [TestMethod]
        public void ThenRegistryEntryReturnsEmptyConstructor()
        {
            TypeRegistration registry =
                listenerData.GetRegistrations().Where(tr => tr.Name == "systemDiagnosticsTraceListener\u200Cimplementation").First();

            registry.AssertConstructor()
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenRegistryIsTransient()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Transient,
                listenerData.GetRegistrations().Where(tr => tr.Name == "systemDiagnosticsTraceListener\u200Cimplementation").First().Lifetime);
        }
    }

    [TestClass]
    public class GivenSystemTraceListenerWithInitializationData
    {
        private SystemDiagnosticsTraceListenerData listenerData;

        [TestInitialize]
        public void Given()
        {
            listenerData = new SystemDiagnosticsTraceListenerData(
                 "systemDiagnosticsTraceListener",
                 typeof(System.Diagnostics.TextWriterTraceListener),
                 "someInitData"
             );
        }

        [TestMethod]
        public void ThenCreatesTwoTypeRegistrations()
        {
            Assert.AreEqual(2, listenerData.GetRegistrations().Count());
        }

        [TestMethod]
        public void ThenRegistrationIsForCorrectServiceAndType()
        {
            TypeRegistration registry
                = listenerData.GetRegistrations().Where(tr => tr.Name == "systemDiagnosticsTraceListener\u200Cimplementation").First();

            registry.AssertForServiceType(typeof(TraceListener))
                .ForName("systemDiagnosticsTraceListener\u200Cimplementation")
                .ForImplementationType(typeof(System.Diagnostics.TextWriterTraceListener));
        }

        [TestMethod]
        public void ThenRegistrationTargetsConstructorWithInitialData()
        {
            TypeRegistration registry
                = listenerData.GetRegistrations().Where(tr => tr.Name == "systemDiagnosticsTraceListener\u200Cimplementation").First();

            registry.AssertConstructor()
                .WithValueConstructorParameter<string>("someInitData")
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenRegistryEntryIncludesPropertyForTraceOptions()
        {
            TypeRegistration registry
                = listenerData.GetRegistrations().Where(tr => tr.Name == "systemDiagnosticsTraceListener\u200Cimplementation").First();

            registry.AssertProperties()
                .WithValueProperty("TraceOutputOptions", TraceOptions.None)
                .WithValueProperty("Name", "systemDiagnosticsTraceListener\u200Cimplementation")
                .VerifyProperties();
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenRegistryIsTransient()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Transient,
                listenerData.GetRegistrations().Where(tr => tr.Name == "systemDiagnosticsTraceListener\u200Cimplementation").First().Lifetime);
        }
    }

    [TestClass]
    public class GivenSystemTraceListenerDataTraceOptionsAndFilterSpecified
    {
        private TypeRegistration registryEntry;
        private TypeRegistration wrapperRegistryEntry;

        [TestInitialize]
        public void Given()
        {
            var listenerData = new SystemDiagnosticsTraceListenerData(
                "systemDiagnosticsTraceListener",
                typeof(System.Diagnostics.TextWriterTraceListener),
                "initData",
                TraceOptions.ProcessId | TraceOptions.Callstack
                );

            listenerData.Filter = SourceLevels.Critical;
            registryEntry = 
                listenerData.GetRegistrations().Where(tr => tr.Name == "systemDiagnosticsTraceListener\u200Cimplementation").First();
            wrapperRegistryEntry =
                listenerData.GetRegistrations().Where(tr => tr.Name == "systemDiagnosticsTraceListener").First();
        }

        [TestMethod]
        public void ThenRegistryEntryIncludesPropertyForTraceOptionsAndFilter()
        {
            TraceFilter filter;

            registryEntry.AssertProperties()
                .WithValueProperty("TraceOutputOptions", TraceOptions.ProcessId | TraceOptions.Callstack)
                .WithValueProperty("Filter", out filter)
                .WithValueProperty("Name", "systemDiagnosticsTraceListener\u200Cimplementation")
                .VerifyProperties();

            Assert.AreEqual(SourceLevels.Critical, ((EventTypeFilter)filter).EventType);
        }
    }

    [TestClass]
    public class GivenSystemTraceListenerDataWithAttributes
    {
        private SystemDiagnosticsTraceListenerData listenerData;

        [TestInitialize]
        public void Given()
        {
            listenerData = new SystemDiagnosticsTraceListenerData(
                "systemDiagnosticsTraceListener",
                typeof(System.Diagnostics.TextWriterTraceListener),
                "initData");
            listenerData.Attributes.Add("checkone", "one");
            listenerData.Attributes.Add("checktwo", "two");
        }

        [TestMethod]
        public void ThenThreeRegistryEntriesAreProvided()
        {
            Assert.AreEqual(3, listenerData.GetRegistrations().Count());
        }

        [TestMethod]
        public void ThenWrappedRegistrationIsImplementationName()
        {
            var registration = listenerData.GetRegistrations().First(r => r.Name == "systemDiagnosticsTraceListener\u200Cimplementation");
            registration.AssertForServiceType(typeof(TraceListener))
                .ForName("systemDiagnosticsTraceListener\u200Cimplementation")
                .ForImplementationType(typeof(AttributeSettingTraceListenerWrapper));
        }

        [TestMethod]
        public void ThenWrappedRegistrationResolvesInnerRegistrationBySynthesizedName()
        {
            var registrations = listenerData.GetRegistrations();
            var wrappingRegistration = registrations.First(r => r.Name == "systemDiagnosticsTraceListener\u200Cimplementation");
            var resolvedParameter = (ContainerResolvedParameter)wrappingRegistration.ConstructorParameters.ElementAt(0);

            Assert.AreSame(typeof(TraceListener), resolvedParameter.Type);

            var resolveTargetRegistration = registrations.First(r => r.Name == resolvedParameter.Name);
            resolveTargetRegistration.AssertForServiceType(typeof(TraceListener))
                .ForImplementationType(typeof(TextWriterTraceListener));
        }

        [TestMethod]
        public void ThenWrappedRegistrationProvidesAttributesToConstructors()
        {
            var registration = listenerData.GetRegistrations().First(r => r.Name == "systemDiagnosticsTraceListener\u200Cimplementation");

            var parameterValue = (ConstantParameterValue)registration.ConstructorParameters.ElementAt(1);
            CollectionAssert.AreEquivalent(listenerData.Attributes, ((NameValueCollection)parameterValue.Value));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCreatedRegistrationsExceptTheOneForTheOriginalNameAreTransient()
        {
            Assert.AreEqual(
                0,
                listenerData.GetRegistrations()
                    .Where(tr => tr.Lifetime != TypeRegistrationLifetime.Transient && tr.Name != "systemDiagnosticsTraceListener").Count());
        }
    }
}
