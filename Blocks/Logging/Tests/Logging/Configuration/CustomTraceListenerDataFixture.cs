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
        public void ThenRegistrationIsForCorrectServiceAndType()
        {
            TypeRegistration registry = listenerData.GetRegistrations().ElementAt(0);

            registry.AssertForServiceType(typeof(TraceListener))
                .ForName("custom trace listener")
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
                .WithValueProperty("Name", "custom trace listener")
                .VerifyProperties();
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
            TypeRegistration registry = listenerData.GetRegistrations().ElementAt(0);

            registry.AssertForServiceType(typeof(TraceListener))
                .ForName("custom trace listener")
                .ForImplementationType(typeof(MockCustomTraceListener));
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
                .WithValueProperty("Name", "custom trace listener")
                .WithContainerResolvedProperty<ILogFormatter>("Formatter", "formatter")
                .VerifyProperties();
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
            TypeRegistration registry = listenerData.GetRegistrations().ElementAt(0);

            registry.AssertForServiceType(typeof(TraceListener))
                .ForName("custom trace listener")
                .ForImplementationType(typeof(MockCustomTraceListener));
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
                .WithValueProperty("Name", "custom trace listener")
                .VerifyProperties();
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
                                                       typeof (MockCustomTraceListenerWithoutExpectedConstructor),
                                                       "initData");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenRegistrationsRetrieved_ThenThrowsException()
        {
            var registrations = listenerData.GetRegistrations();
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

