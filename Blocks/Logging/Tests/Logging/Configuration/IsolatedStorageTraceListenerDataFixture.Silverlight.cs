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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenIsolatedStorageTraceListenerData
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData =
                new IsolatedStorageTraceListenerData
                {
                    Name = "listener",
                    MaxSizeInKilobytes = 1,
                    RepositoryName = "test repository"
                };
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCreatesTwoTypeRegistrations()
        {
            Assert.AreEqual(2, listenerData.GetRegistrations().Count());
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCreatesATypeRegistrationForTheListenerWithTheConfiguredName()
        {
            listenerData.GetRegistrations().First(tr => tr.Name == "listener")
                .AssertForServiceType(typeof(TraceListener))
                .ForName("listener")
                .ForImplementationType(typeof(IsolatedStorageTraceListener));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenTraceListenerRegistrationIsSingleton()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Singleton,
                listenerData.GetRegistrations().First(tr => tr.Name == "listener").Lifetime);
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenTraceListenerRegistrationIsInjectedWithTheNameProperty()
        {
            listenerData.GetRegistrations().First(tr => tr.Name == "listener")
                .AssertProperties()
                .WithValueProperty("Name", "listener")
                .VerifyProperties();
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenTraceListenerRegistrationIsInjectedWithALogEntryRepository()
        {
            listenerData.GetRegistrations().First(tr => tr.Name == "listener")
                .AssertConstructor()
                .WithContainerResolvedParameter<ILogEntryRepository>("test repository")
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCreatesATypeRegistrationForTheRepository()
        {
            listenerData.GetRegistrations().First(tr => tr.Name == "test repository")
                .AssertForServiceType(typeof(ILogEntryRepository))
                .ForName("test repository")
                .ForImplementationType(typeof(IsolatedStorageLogEntryRepository));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenRepositoryRegistrationIsSingleton()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Singleton,
                listenerData.GetRegistrations().First(tr => tr.Name == "test repository").Lifetime);
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenRepositoryRegistrationInjectsRepositoryNameAndMaxSize()
        {
            listenerData.GetRegistrations().First(tr => tr.Name == "test repository")
                .AssertConstructor()
                .WithValueConstructorParameter("test repository")
                .WithValueConstructorParameter(1)
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenIsolatedStorageTraceListenerDataWithNoMaxSize
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData =
                new IsolatedStorageTraceListenerData
                {
                    Name = "listener",
                    RepositoryName = "test repository"
                };
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenRepositoryRegistrationInjectsRepositoryNameAndDefaultMaxSize()
        {
            listenerData.GetRegistrations().First(tr => tr.Name == "test repository")
                .AssertConstructor()
                .WithValueConstructorParameter("test repository")
                .WithValueConstructorParameter(256)
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenIsolatedStorageTraceListenerDataWithNoRepositoryName
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData =
                new IsolatedStorageTraceListenerData
                {
                    Name = "listener",
                };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrows()
        {
            listenerData.GetRegistrations();
        }
    }
}
