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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenXmlTraceListenerDataWithFilterData
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData =
                new XmlTraceListenerData("listener", "file name")
                {
                    TraceOutputOptions = TraceOptions.DateTime | TraceOptions.Callstack,
                    Filter = SourceLevels.Warning
                };
        }

        [TestMethod]
        public void WhenCreatingInstanceUsingDefaultContructor_ThenListenerDataTypeIsSet()
        {
            var listener = new XmlTraceListenerData();
            Assert.AreEqual(typeof(XmlTraceListenerData), listener.ListenerDataType);
        }

        [TestMethod]
        public void WhenCreatingListener_ThenCreatesXmlListener()
        {
            var settings = new LoggingSettings();

            var listener = (XmlTraceListener)listenerData.BuildTraceListener(settings);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(TraceOptions.DateTime | TraceOptions.Callstack, listener.TraceOutputOptions);
            Assert.IsNotNull(listener.Filter);
            Assert.AreEqual(SourceLevels.Warning, ((EventTypeFilter)listener.Filter).EventType);
        }
    }
}
