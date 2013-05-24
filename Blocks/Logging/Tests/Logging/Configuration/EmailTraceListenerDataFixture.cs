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
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
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
                    Filter = SourceLevels.Warning,
                    AuthenticationMode = EmailAuthenticationMode.UserNameAndPassword,
                    UserName = "user",
                    Password = "secret",
                    UseSSL = true
                };
        }

        [TestMethod]
        public void WhenCreatingInstanceUsingDefaultContructor_ThenListenerDataTypeIsSet()
        {
            var listener = new EmailTraceListenerData();
            Assert.AreEqual(typeof(EmailTraceListenerData), listener.ListenerDataType);
        }

        [TestMethod]
        public void WhenCreatingListener_ThenCreatesEmailListener()
        {
            var settings = new LoggingSettings { Formatters = { new TextFormatterData { Name = "formatter", Template = "template" } } };

            var listener = (EmailTraceListener)listenerData.BuildTraceListener(settings);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(TraceOptions.DateTime | TraceOptions.Callstack, listener.TraceOutputOptions);
            Assert.IsNotNull(listener.Filter);
            Assert.AreEqual(SourceLevels.Warning, ((EventTypeFilter)listener.Filter).EventType);
            Assert.IsInstanceOfType(listener.Formatter, typeof(TextFormatter));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatingListenerWithNoFormatterAvailable_ThenThrows()
        {
            var settings = new LoggingSettings { Formatters = { } };

            listenerData.BuildTraceListener(settings);
        }
    }

    [TestClass]
    public class GivenEmailTraceListenerDataWithNoFormatter
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData =
                new EmailTraceListenerData("listener", "to", "from", "subject starter", "subject ender", "smtp", 25, "")
                {
                    TraceOutputOptions = TraceOptions.DateTime | TraceOptions.Callstack,
                    Filter = SourceLevels.Warning,
                    AuthenticationMode = EmailAuthenticationMode.UserNameAndPassword,
                    UserName = "user",
                    Password = "secret",
                    UseSSL = true
                };
        }

        [TestMethod]
        public void WhenCreatingListener_ThenCreatesEmailListener()
        {
            var settings = new LoggingSettings { Formatters = { new TextFormatterData { Name = "formatter", Template = "template" } } };

            var listener = (EmailTraceListener)listenerData.BuildTraceListener(settings);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(TraceOptions.DateTime | TraceOptions.Callstack, listener.TraceOutputOptions);
            Assert.IsNotNull(listener.Filter);
            Assert.AreEqual(SourceLevels.Warning, ((EventTypeFilter)listener.Filter).EventType);
            Assert.IsNull(listener.Formatter);
        }
    }
}
