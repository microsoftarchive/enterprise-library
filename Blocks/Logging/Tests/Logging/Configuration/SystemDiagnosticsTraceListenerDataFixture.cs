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
            listenerData =
                new SystemDiagnosticsTraceListenerData(
                    "systemDiagnosticsTraceListener",
                    typeof(MockSystemDiagsTraceListener),
                    "");
        }

        [TestMethod]
        public void WhenCreatingInstanceUsingDefaultContructor_ThenListenerDataTypeIsSet()
        {
            var listener = new SystemDiagnosticsTraceListenerData();
            Assert.AreEqual(typeof(SystemDiagnosticsTraceListenerData), listener.ListenerDataType);
        }

        [TestMethod]
        public void WhenCreatingListener_ThenCreatesListenerWithNoArgsConstructor()
        {
            var settings = new LoggingSettings { };

            var listener = (MockSystemDiagsTraceListener)listenerData.BuildTraceListener(settings);

            Assert.IsNotNull(listener);
            Assert.AreEqual("systemDiagnosticsTraceListener", listener.Name);
            Assert.AreSame(MockSystemDiagsTraceListener.NoValue, listener.Value);
            Assert.AreEqual(TraceOptions.None, listener.TraceOutputOptions);
            Assert.IsNull(listener.Filter);
            Assert.AreEqual(0, listener.Attributes.Count);
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
                 typeof(MockSystemDiagsTraceListener),
                 "someInitData"
             );
        }

        [TestMethod]
        public void WhenCreatingListener_ThenCreatesListenerWithSingleArgConstructor()
        {
            var settings = new LoggingSettings { };

            var listener = (MockSystemDiagsTraceListener)listenerData.BuildTraceListener(settings);

            Assert.IsNotNull(listener);
            Assert.AreEqual("systemDiagnosticsTraceListener", listener.Name);
            Assert.AreSame("someInitData", listener.Value);
            Assert.AreEqual(TraceOptions.None, listener.TraceOutputOptions);
            Assert.IsNull(listener.Filter);
            Assert.AreEqual(0, listener.Attributes.Count);
        }
    }

    [TestClass]
    public class GivenSystemTraceListenerDataTraceOptionsAndFilterSpecified
    {
        private SystemDiagnosticsTraceListenerData listenerData;

        [TestInitialize]
        public void Given()
        {
            this.listenerData = new SystemDiagnosticsTraceListenerData(
                "systemDiagnosticsTraceListener",
                typeof(MockSystemDiagsTraceListener),
                "initData",
                TraceOptions.ProcessId | TraceOptions.Callstack
                );
            listenerData.Filter = SourceLevels.Critical;
        }

        [TestMethod]
        public void WhenCreatingListener_ThenCreatesListenerWithSingleArgConstructor()
        {
            var settings = new LoggingSettings { };

            var listener = (MockSystemDiagsTraceListener)listenerData.BuildTraceListener(settings);

            Assert.IsNotNull(listener);
            Assert.AreEqual("systemDiagnosticsTraceListener", listener.Name);
            Assert.AreEqual("initData", listener.Value);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.Callstack, listener.TraceOutputOptions);
            Assert.IsNotNull(listener.Filter);
            Assert.AreEqual(SourceLevels.Critical, ((EventTypeFilter)listener.Filter).EventType);
            Assert.AreEqual(0, listener.Attributes.Count);
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
                typeof(MockSystemDiagsTraceListener),
                "initData");
            listenerData.Attributes.Add("checkone", "one");
            listenerData.Attributes.Add("checktwo", "two");
        }

        [TestMethod]
        public void WhenCreatingListener_ThenCreatesListenerWithSingleArgConstructorAndSetsAttributes()
        {
            var settings = new LoggingSettings { };

            var listener = (MockSystemDiagsTraceListener)listenerData.BuildTraceListener(settings);

            Assert.IsNotNull(listener);
            Assert.AreEqual("systemDiagnosticsTraceListener", listener.Name);
            Assert.AreEqual("initData", listener.Value);
            Assert.AreEqual(TraceOptions.None, listener.TraceOutputOptions);
            Assert.IsNull(listener.Filter);
            Assert.AreEqual(2, listener.Attributes.Count);
            Assert.AreEqual("one", listener.Attributes["checkone"]);
            Assert.AreEqual("two", listener.Attributes["checktwo"]);
        }
    }

    [TestClass]
    public class MiscSystemTraceListenerDataScenarios
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatingListerWithNullType_ThenThrows()
        {
            var settings = new LoggingSettings { };
            new SystemDiagnosticsTraceListenerData { TypeName = null }.BuildTraceListener(settings);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatingListerWithEmptyType_ThenThrows()
        {
            var settings = new LoggingSettings { };
            new SystemDiagnosticsTraceListenerData { TypeName = "" }.BuildTraceListener(settings);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatingListerWithInvalidType_ThenThrows()
        {
            var settings = new LoggingSettings { };
            new SystemDiagnosticsTraceListenerData { TypeName = "clearly not a \t type name" }.BuildTraceListener(settings);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatingListerWithNonListenerdType_ThenThrows()
        {
            var settings = new LoggingSettings { };
            new SystemDiagnosticsTraceListenerData { TypeName = typeof(string).AssemblyQualifiedName }.BuildTraceListener(settings);
        }
    }

    public class MockSystemDiagsTraceListener : TraceListener
    {
        public static readonly object NoValue = new object();

        private object value;

        public MockSystemDiagsTraceListener()
        {
            this.value = NoValue;
        }

        public MockSystemDiagsTraceListener(string initData)
        {
            this.value = initData;
        }

        public object Value
        {
            get { return this.value; }
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
