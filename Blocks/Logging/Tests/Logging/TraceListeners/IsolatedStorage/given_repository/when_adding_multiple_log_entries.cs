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

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_repository
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_adding_multiple_log_entries : Context
    {
        private LogEntry logEntry0, logEntry1, logEntry2;

        protected override void Act()
        {
            base.Act();

            this.logEntry0 =
                new LogEntry
                {
                    Message = "some message",
                    Categories = new[] { "category1", "category2" }
                };

            this.logEntry1 =
                new LogEntry
                {
                    Message = "some message 2",
                    Categories = new[] { "category3", "category5" }
                };

            this.logEntry2 =
                new LogEntry
                {
                    Message = "some message 3",
                    Categories = new[] { "category1", "category2" },
                    Severity = Diagnostics.TraceEventType.Verbose
                };

            this.repository.Add(this.logEntry0);
            this.repository.Add(this.logEntry1);
            this.repository.Add(this.logEntry2);
        }

        [TestMethod]
        public void then_can_retrieve_the_entries()
        {
            var actualEntries = this.repository.RetrieveEntries();

            Assert.AreEqual(3, actualEntries.Count());
            LogEntryAssert.AreEqual(this.logEntry0, actualEntries.ElementAt(0));
            LogEntryAssert.AreEqual(this.logEntry1, actualEntries.ElementAt(1));
            LogEntryAssert.AreEqual(this.logEntry2, actualEntries.ElementAt(2));
        }

    }
}
