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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    public abstract class Given_RemoteListenerInConfigurationSourceBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationSendToRemoteServiceTraceListener RemoteListenerBuilder;
        private string traceListenerName = "remote listener";

        protected override void Arrange()
        {
            base.Arrange();

            RemoteListenerBuilder = base.CategorySourceBuilder.SendTo.RemoteService(traceListenerName);
        }

        protected RemoteServiceTraceListenerData GetRemoteServiceTraceListenerData()
        {
            return base.GetLoggingConfiguration().TraceListeners.OfType<RemoteServiceTraceListenerData>()
                .Where(x => x.Name == traceListenerName).First();
        }
    }

    [TestClass]
    public class When_CallingSendToRemoteListenerOnLogToCategoryConfigurationBuilder : Given_RemoteListenerInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();
        }

        [TestMethod]
        public void ThenSubmitIntervalIsDefault()
        {
            Assert.AreEqual(TimeSpan.FromMinutes(1), GetRemoteServiceTraceListenerData().SubmitInterval);
        }

        [TestMethod]
        public void ThenSendImmediatelyIsFalse()
        {
            Assert.AreEqual(false, GetRemoteServiceTraceListenerData().SendImmediately);
        }

        [TestMethod]
        public void ThenIsolatedStorageBufferMaxSizeInKilobytesIsZero()
        {
            Assert.AreEqual(0, GetRemoteServiceTraceListenerData().IsolatedStorageBufferMaxSizeInKilobytes);
        }

        [TestMethod]
        public void ThenMaxBufferedEntriesIs100()
        {
            Assert.AreEqual(100, GetRemoteServiceTraceListenerData().MaxElementsInBuffer);
        }
    }

    [TestClass]
    public class When_CallingSendToRemoteListenerPassingNullForName : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_SendToRemoteService_TrowsArgumentException()
        {
            base.CategorySourceBuilder.SendTo.RemoteService(null);
        }
    }


    [TestClass]
    public class When_CallingWithLoggingServiceFactoryOnRemoteServiceTraceListener : Given_RemoteListenerInConfigurationSourceBuilder
    {
        ILoggingServiceFactory factory = Mock.Of<ILoggingServiceFactory>();

        protected override void Act()
        {
            base.RemoteListenerBuilder.WithLoggingServiceFactory(factory);
        }

        [TestMethod]
        public void ThenLoggingServiceFactoryIsSame()
        {
            Assert.AreSame(factory, base.GetRemoteServiceTraceListenerData().LoggingServiceFactory);
        }
    }

    [TestClass]
    public class When_CallingWithLoggingServiceEndpointConfigurationNameOnRemoteServiceTraceListener : Given_RemoteListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RemoteListenerBuilder.WithLoggingServiceEndpointConfigurationName("test");
        }

        [TestMethod]
        [Ignore]
        public void ThenLoggingServiceFactoryIsCreatedForEndpoint()
        {
            Assert.IsNull(base.initializeException);
            Assert.IsNotNull(base.GetRemoteServiceTraceListenerData().LoggingServiceFactory);
            Assert.IsInstanceOfType(base.GetRemoteServiceTraceListenerData().LoggingServiceFactory, typeof(LoggingServiceFactory));
            Assert.AreSame("test", ((LoggingServiceFactory)base.GetRemoteServiceTraceListenerData().LoggingServiceFactory).EndpointConfigurationName);
        }
    }

    [TestClass]
    public class When_CallingSetSubmitIntervalOnRemoteServiceTraceListener : Given_RemoteListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RemoteListenerBuilder.SetSubmitInterval(TimeSpan.FromSeconds(33));
        }

        [TestMethod]
        public void ThenUseAuthenticationIsTrue()
        {
            Assert.AreEqual(TimeSpan.FromSeconds(33), base.GetRemoteServiceTraceListenerData().SubmitInterval);
        }
    }

    [TestClass]
    public class When_CallingSendsImmediatelyOnRemoteServiceTraceListener : Given_RemoteListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RemoteListenerBuilder.SendsImmediately();
        }

        [TestMethod]
        public void ThenUseAuthenticationIsTrue()
        {
            Assert.AreEqual(true, base.GetRemoteServiceTraceListenerData().SendImmediately);
        }
    }

    [TestClass]
    public class When_CallingSetIsolatedStorageBufferMaxSizeInKilobytesOnRemoteServiceTraceListener : Given_RemoteListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RemoteListenerBuilder.SetIsolatedStorageBufferMaxSizeInKilobytes(123);
        }

        [TestMethod]
        public void ThenUseAuthenticationIsTrue()
        {
            Assert.AreEqual(123, base.GetRemoteServiceTraceListenerData().IsolatedStorageBufferMaxSizeInKilobytes);
        }
    }

    [TestClass]
    public class When_CallingSetMaxElementsInBufferOnRemoteServiceTraceListener : Given_RemoteListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RemoteListenerBuilder.SetMaxElementsInBuffer(321);
        }

        [TestMethod]
        public void ThenUseAuthenticationIsTrue()
        {
            Assert.AreEqual(321, base.GetRemoteServiceTraceListenerData().MaxElementsInBuffer);
        }
    }
}
