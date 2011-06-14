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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class given_a_log_writer_with_null_values : ArrangeActAssert
    {
        protected LogWriterImpl LogWriter;

        protected override void Arrange()
        {
            base.Arrange();

            LogWriter =
                new LogWriterImpl(
                    new ILogFilter[0],
                    new LogSource[0],
                    null,
                    null,
                    new LogSource("errors", new TraceListener[] { new MockTraceListener("mockTraceListener") }, SourceLevels.Error, false),
                    "",
                    true,
                    false);
        }

        protected override void Teardown()
        {
            base.Teardown();
            LogWriter.Dispose();
            LogWriter = null;
        }

        [TestMethod]
        public void then_can_apply_changes()
        {
            var context = LogWriter.GetUpdateContext();
            context.ApplyChanges();
        }
    }
}
