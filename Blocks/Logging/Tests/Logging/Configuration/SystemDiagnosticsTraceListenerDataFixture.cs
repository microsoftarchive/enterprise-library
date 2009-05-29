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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
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
        public void ThenRegistryEntryReturnsNamedServiceEntry()
        {
            TypeRegistration registry = listenerData.GetRegistrations().ElementAt(0);

            registry.AssertForServiceType(typeof(TraceListener))
                .ForName("systemDiagnosticsTraceListener")
                .ForImplementationType(typeof(System.Diagnostics.TextWriterTraceListener));
        }

        [TestMethod]
        public void ThenRegistryEntryReturnsEmptyConstructor()
        {
            TypeRegistration registry = listenerData.GetRegistrations().ElementAt(0);

            registry.AssertConstructor()
                .VerifyConstructorParameters();
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
        public void ThenRegistrationIsForCorrectServiceAndType()
        {
            TypeRegistration registry = listenerData.GetRegistrations().ElementAt(0);

            registry.AssertForServiceType(typeof(TraceListener))
                .ForName("systemDiagnosticsTraceListener")
                .ForImplementationType(typeof(System.Diagnostics.TextWriterTraceListener));
        }

        [TestMethod]
        public void ThenRegistrationTargetsConstructorWithInitialData()
        {
            TypeRegistration registry = listenerData.GetRegistrations().ElementAt(0);

            registry.AssertConstructor()
                .WithValueConstructorParameter<string>("someInitData")
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenRegistryEntryIncludesPropertyForTraceOptions()
        {
            TypeRegistration registry = listenerData.GetRegistrations().ElementAt(0);

            registry.AssertProperties()
                .WithValueProperty("TraceOutputOptions", TraceOptions.None)
                .WithValueProperty("Name", "systemDiagnosticsTraceListener")
                .VerifyProperties();
        }
    }

    [TestClass]
    public class GivenSystemTraceListenerDataTraceOptionsAndFilterSpecified
    {
        private TypeRegistration registryEntry;

        [TestInitialize]
        public void Given()
        {
            var listenerData = new SystemDiagnosticsTraceListenerData(
                "systemDiagnostricsTraceListener",
                typeof(System.Diagnostics.TextWriterTraceListener),
                "initData",
                TraceOptions.ProcessId | TraceOptions.Callstack
                );

            listenerData.Filter = SourceLevels.Critical;
            registryEntry = listenerData.GetRegistrations().ElementAt(0);
        }

        [TestMethod]
        public void ThenRegistryEntryIncludesPropertyForTraceOptionsAndFilter()
        {
            TraceFilter filter;

            registryEntry.AssertProperties()
                .WithValueProperty("TraceOutputOptions", TraceOptions.ProcessId | TraceOptions.Callstack)
                .WithValueProperty("Filter", out filter)
                .WithValueProperty("Name", "systemDiagnostricsTraceListener")
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
                "systemDiagnostricsTraceListener",
                typeof(System.Diagnostics.TextWriterTraceListener),
                "initData");
            listenerData.Attributes.Add("checkone", "one");
            listenerData.Attributes.Add("checktwo", "two");
        }

        [TestMethod]
        public void ThenTwoRegistryEntriesAreProvided()
        {
            Assert.AreEqual(2, listenerData.GetRegistrations().Count());
        }

        [TestMethod]
        public void ThenWrappedRegistrationIsRootName()
        {
            var registration = listenerData.GetRegistrations().First(r => r.Name == listenerData.Name);
            registration.AssertForServiceType(typeof(TraceListener))
                .ForName(listenerData.Name)
                .ForImplementationType(typeof(AttributeSettingTraceListenerWrapper));
        }

        [TestMethod]
        public void ThenWrappedRegistrationResolvesInnerRegistrationBySynthesizedName()
        {
            var registrations = listenerData.GetRegistrations();
            var wrappingRegistration = registrations.First(r => r.Name == listenerData.Name);
            var resolvedParameter = (ContainerResolvedParameter)wrappingRegistration.ConstructorParameters.ElementAt(0);

            Assert.AreSame(typeof(TraceListener), resolvedParameter.Type);

            var resolveTargetRegistration = registrations.First(r => r.Name == resolvedParameter.Name);
            resolveTargetRegistration.AssertForServiceType(typeof(TraceListener))
                .ForImplementationType(typeof(TextWriterTraceListener));
        }

        [TestMethod]
        public void ThenWrappedRegistrationProvidesAttributesToConstructors()
        {
            var registration = listenerData.GetRegistrations().First(r => r.Name == listenerData.Name);

            var parameterValue = (ConstantParameterValue)registration.ConstructorParameters.ElementAt(1);
            CollectionAssert.AreEquivalent(listenerData.Attributes, ((NameValueCollection)parameterValue.Value));
        }
    }
}
