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
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class RemoteServiceTraceListenerDataFixture
    {
        private RemoteServiceTraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData = new RemoteServiceTraceListenerData {Name = "listener", LoggingServiceFactory = new Mock<ILoggingServiceFactory>().Object};
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCreatesOneTypeRegistration()
        {
            Assert.AreEqual(1, listenerData.GetRegistrations().Count());
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCreatesATypeRegistrationForTheListenerWithTheConfiguredName()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener").First()
                .AssertForServiceType(typeof(TraceListener))
                .ForName("listener")
                .ForImplementationType(typeof(RemoteServiceTraceListener));
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
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrowsIfSubmitIntervalIsBelowOrEqualZero()
        {
            var data = new RemoteServiceTraceListenerData
                           {
                               Name = "listener",
                               LoggingServiceFactory = new Mock<ILoggingServiceFactory>().Object,
                               SubmitInterval = TimeSpan.FromSeconds(0)
                           };
            data.GetRegistrations().ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrowsIfLoggingServiceFactoryIsNull()
        {
            var data = new RemoteServiceTraceListenerData
            {
                Name = "listener"
            };
            data.GetRegistrations().ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrowsIfIsolatedStorageBufferMaxSizeInKilobytesIsBelowMinimum()
        {
            var data = new RemoteServiceTraceListenerData
            {
                Name = "listener",
                IsolatedStorageBufferMaxSizeInKilobytes = 4
            };
            data.GetRegistrations().ToArray();
        }
    }
}
