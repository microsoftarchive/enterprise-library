using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service.Tests.LoggingService.given_an_incoming_log_entry
{
    public abstract class Context : ArrangeActAssert
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

        protected override void Arrange()
        {
            base.Arrange();

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
    }
}
