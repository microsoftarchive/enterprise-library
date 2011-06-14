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

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_trace_listener
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_logging_an_entry : Context
    {
        private LogEntry logEntry;

        protected override void Act()
        {
            this.logEntry = new LogEntry { };

            this.traceListener.TraceData(new Diagnostics.TraceEventCache(), "test", Diagnostics.TraceEventType.Error, 0, this.logEntry);
        }

        [TestMethod]
        public void then_entry_is_logged_to_the_repository()
        {
            this.repositoryMock.Verify(r => r.Add(this.logEntry));
        }
    }
}
