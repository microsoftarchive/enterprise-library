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
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_log_entry_serializer
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_serializing_log_entry_with_extended_properties : Context
    {
        private LogEntry logEntry;
        private byte[] serializedEntry;

        protected override void Act()
        {
            base.Act();

            this.logEntry =
                new LogEntry
                {
                    Message = "message",
                    EventId = 100,
                    Priority = 200,
                    Severity = Diagnostics.TraceEventType.Error,
                    Title = "title",
                    ExtendedProperties =
                        new Dictionary<string, object>
                        {
                            { "key1", "value1" }
                        }
                };
            this.serializedEntry = this.serializer.Serialize(this.logEntry);
        }

        [TestMethod]
        public void then_can_deserialize_the_entry()
        {
            var actualEntry = this.serializer.Deserialize(this.serializedEntry);

            LogEntryAssert.AreEqual(this.logEntry, actualEntry);
        }
    }
}
