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
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service.Tests.LoggingService.given_an_incoming_log_entry
{
    public abstract class Context
    {
        protected LogEntryMessage TestLogEntry;
        protected Service.LoggingService LoggingService;
        protected IList<LogEntry> LogWriterEntries;

        protected const string TestListenerName = "listenerName";
        protected const string TestSource = "source";
        protected const string TestMessage = "message";
        protected const int TestId = 100;
        protected readonly string[] TestCategories = new[] { "cat1", "cat2" };
        protected const TraceEventType TestTraceEventType = TraceEventType.Error;
        protected readonly string[] TestExtendedPropertiesKeys = new[] { "key1", "key2" };
        protected readonly string[] TestExtendedPropertiesValues = new[] { "value1", "value2" };
        protected readonly DateTime TestTimeStamp = new DateTime(2000, 2, 3, 5, 6, 7, 8, DateTimeKind.Utc);

        /// <summary>
        /// When overridden in a derived class, this method is used to
        /// set up the current state of the specs context.
        /// </summary>
        /// <remarks>This method is called automatically before every test,
        /// before the <see cref="Act"/> method.</remarks>
        protected virtual void Arrange()
        {
            LogWriterEntries = new List<LogEntry>();
            TestLogEntry = new LogEntryMessage
            {
                Message = TestMessage,
                Categories = TestCategories,
                Severity = TestTraceEventType,
                ExtendedPropertiesKeys = TestExtendedPropertiesKeys,
                ExtendedPropertiesValues = TestExtendedPropertiesValues,
                TimeStamp = TestTimeStamp.ToString("o", CultureInfo.InvariantCulture)
            };
            var loggerMock = new Mock<LogWriter>();
            loggerMock.Setup(x => x.Write(It.IsAny<LogEntry>())).Callback<LogEntry>(e => LogWriterEntries.Add(e));

            LoggingService = new Service.LoggingService(loggerMock.Object);
        }

        [TestInitialize]
        public void MainSetup()
        {
            Arrange();
            Act();
        }

        [TestCleanup]
        public void MainTeardown()
        {
            Teardown();
        }

        /// <summary>
        /// When overridden in a derived class, this method is used to
        /// perform interactions against the system under test.
        /// </summary>
        /// <remarks>This method is called automatically after <see cref="Arrange"/>
        /// and before each test method runs.</remarks>
        protected virtual void Act()
        {
        }

        /// <summary>
        /// When overridden in a derived class, this method is used to
        /// reset the state of the system after a test method has completed.
        /// </summary>
        /// <remarks>This method is called automatically after each TestMethod has run.</remarks>
        protected virtual void Teardown()
        {
        }
    }
}
