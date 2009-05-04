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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class TraceSourceNodeFixture : ConfigurationDesignHost
    {
        protected override void InitializeCore()
        {
            base.InitializeCore();
        }

        [TestMethod]
        public void TraceSourceNodeDefaults()
        {
            TraceSourceNode node = new TraceSourceNodeImpl();
            Assert.AreEqual(string.Empty, node.Name);
            Assert.AreEqual(SourceLevels.All, node.SourceLevels);
            Assert.AreEqual(LogSource.DefaultAutoFlushProperty, node.AutoFlush);
        }

        [TestMethod]
        public void TraceSourceNodeFromTraceSourceData()
        {
            string name = "name";
            bool defaultAutoFlush = true;
            SourceLevels defaultSourceLevel = SourceLevels.All;

            TraceSourceData data = new TraceSourceData(name, defaultSourceLevel, defaultAutoFlush);

            TraceSourceNode node = new TraceSourceNodeImpl(data);

            Assert.AreEqual(data.AutoFlush, node.AutoFlush);
            Assert.AreEqual(data.DefaultLevel, node.SourceLevels);
            Assert.AreEqual(data.Name, node.Name);
        }

        [TestMethod]
        public void TraceSourceDataFromTraceSourceNode()
        {
            string name = "name";
            bool defaultAutoFlush = false;
            SourceLevels defaultSourceLevel = SourceLevels.All;

            TraceSourceNode node = new TraceSourceNodeImpl(name, defaultSourceLevel, defaultAutoFlush);

            TraceSourceData data = new TraceSourceData(name, defaultSourceLevel, defaultAutoFlush);

            Assert.AreEqual(node.Name, data.Name);
            Assert.AreEqual(node.SourceLevels, data.DefaultLevel);
            Assert.AreEqual(node.AutoFlush, data.AutoFlush);
        }

        class TraceSourceNodeImpl : TraceSourceNode
        {
            public TraceSourceNodeImpl()
                : base() {}

            public TraceSourceNodeImpl(TraceSourceData data)
                : this(data.Name, data.DefaultLevel, data.AutoFlush) {}

            public TraceSourceNodeImpl(string name,
                                       SourceLevels sourceLevels,
                                       bool autoFlush)
                : base(name, sourceLevels, autoFlush) {}
        }
    }
}
