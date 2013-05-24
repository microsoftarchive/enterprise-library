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

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class TracerManagerFixture
    {
        [TestMethod]
        public void GetTracerFromTraceManagerWithNoInstrumentation()
        {
            MockTraceListener.Reset();

            LogSource source = new LogSource("tracesource", new[] { new MockTraceListener() }, SourceLevels.All);

            List<LogSource> traceSources = new List<LogSource>(new LogSource[] { source });
            LogWriter lg = new LogWriter(new List<ILogFilter>(), new List<LogSource>(), source, null, new LogSource("errors"), "default", true, false);

            TraceManager tm = new TraceManager(lg);

            Assert.IsNotNull(tm);

            using (tm.StartTrace("testoperation"))
            {
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
            }

            Assert.AreEqual(2, MockTraceListener.Entries.Count);
        }
    }
}
