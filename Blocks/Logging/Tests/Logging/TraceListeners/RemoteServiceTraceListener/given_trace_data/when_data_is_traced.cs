using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener.given_trace_data
{
    [TestClass]
    public class when_data_is_traced : Context
    {
        protected override void Act()
        {
            Listener.TraceData(TestTraceEventCache, TestSource, TestTraceEventType, TestId, TestLogEntry);
            DoWork();
        }

        [TestMethod]
        public void then_forces_tick_on_scheduler()
        {
            TimerMock.Verify(x => x.ForceDoWork(), Times.AtLeast(2));
        }

        [TestMethod]
        public void then_trace_is_properly_sent_through_channel()
        {
            Assert.AreEqual(1, SendLogEntriesMessages.Count);
            Assert.AreEqual(1, SendLogEntriesMessages.First().Length);
            Assert.AreEqual(TestLogEntry.Message, SendLogEntriesMessages.First()[0].Message);
            Assert.AreEqual(TestLogEntry.Severity, SendLogEntriesMessages.First()[0].Severity);
            Assert.AreEqual(TestCategories.Length, SendLogEntriesMessages.First()[0].Categories.Length);
            Assert.AreEqual(TestCategories[0], SendLogEntriesMessages.First()[0].Categories[0]);
            Assert.AreEqual(TestCategories[1], SendLogEntriesMessages.First()[0].Categories[1]);
        }

        [TestMethod]
        public void then_intrinsic_properties_are_collected_before_sending()
        {
            var actualTimeStamp = SendLogEntriesMessages.First()[0].TimeStamp;
            Assert.AreEqual(TestLogEntry.TimeStamp, DateTime.Parse(actualTimeStamp, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind));
        }

        [TestMethod]
        public void then_extended_properties_are_serialized_as_strings()
        {
            var actual = SendLogEntriesMessages.First()[0];
            Assert.AreEqual(TestExtendedProperties.Count, actual.ExtendedPropertiesKeys.Length);
            Assert.AreEqual(TestExtendedProperties.Count, actual.ExtendedPropertiesValues.Length);
            for (int i = 0; i < TestExtendedProperties.Count; i++)
            {
                Assert.AreEqual(TestExtendedProperties.ElementAt(i).Key, actual.ExtendedPropertiesKeys[i]);
                Assert.AreEqual(TestExtendedProperties.ElementAt(i).Value.ToString(), actual.ExtendedPropertiesValues[i]);
            }
        }

        [TestMethod]
        public void then_not_supported_intrinsic_properties_are_set_with_default_values()
        {
            Assert.IsNull(SendLogEntriesMessages.First()[0].MachineName);
            Assert.IsNull(SendLogEntriesMessages.First()[0].ProcessId);
            Assert.IsNull(SendLogEntriesMessages.First()[0].ProcessName);
            Assert.IsNull(SendLogEntriesMessages.First()[0].Win32ThreadId);
        }

        [TestMethod]
        public void then_end_async_method_is_called()
        {
            LoggingServiceMock.Verify(x => x.EndAdd(It.IsAny<IAsyncResult>()), Times.Once());
        }

        [TestMethod]
        public void then_logging_service_is_disposed()
        {
            LoggingServiceMock.As<IDisposable>().Verify(x => x.Dispose(), Times.Once());
        }
    }
}
