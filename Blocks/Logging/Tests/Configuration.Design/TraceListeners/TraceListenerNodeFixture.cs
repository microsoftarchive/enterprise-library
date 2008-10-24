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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests.TraceListeners
{
    [TestClass]
    public class TraceListenerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void TraceListenerNodeDefaults()
        {
            TraceListenerNode listener = new TraceListenerNodeImpl();

            Assert.AreEqual(TraceOptions.None, listener.TraceOutputOptions);
            Assert.AreEqual(SourceLevels.All, listener.Filter);
        }

        [TestMethod]
        public void TraceListenerNodeEqualsTraceListenerData()
        {
            TraceOptions traceOptions = TraceOptions.Callstack;
            SourceLevels filter = SourceLevels.Critical;

            TraceListenerNode listener = new TraceListenerNodeImpl(traceOptions, filter);

            TraceListenerData data = new TraceListenerData();
            data.Filter = filter;
            data.TraceOutputOptions = traceOptions;

            Assert.AreEqual(listener.TraceOutputOptions, data.TraceOutputOptions);
            Assert.AreEqual(listener.Filter, data.Filter);
        }

        [TestMethod]
        public void TraceListenerNodeTest()
        {
            TraceOptions options = TraceOptions.DateTime;

            TraceListenerNode listenerNode = new TraceListenerNodeImpl(new TraceListenerData());
            listenerNode.TraceOutputOptions = options;

            TraceListenerData nodeData = listenerNode.TraceListenerData;

            Assert.AreEqual(options, nodeData.TraceOutputOptions);
        }

        [TestMethod]
        public void TraceListenerNodeDataTest()
        {
            TraceOptions options = TraceOptions.DateTime;

            TraceListenerData traceListenerData = new TraceListenerData();
            traceListenerData.TraceOutputOptions = options;

            TraceListenerNode traceListenerNode = new TraceListenerNodeImpl(traceListenerData);

            Assert.AreEqual(options, traceListenerNode.TraceOutputOptions);
        }

        class TraceListenerNodeImpl : TraceListenerNode
        {
            public TraceListenerNodeImpl()
                : base() {}

            public TraceListenerNodeImpl(TraceOptions traceOptions,
                                         SourceLevels filter)
                : base(traceOptions, filter) {}

            public TraceListenerNodeImpl(TraceListenerData data)
            {
                Rename(data.Name);
                TraceOutputOptions = data.TraceOutputOptions;
            }

            public override TraceListenerData TraceListenerData
            {
                get
                {
                    TraceListenerData data = new TraceListenerData();
                    data.Name = Name;
                    data.TraceOutputOptions = TraceOutputOptions;
                    data.Filter = Filter;
                    return data;
                }
            }
        }
    }
}
