//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Tests.Configuration.LoggingExceptionHandlerDataFixture
{
    public class given_valid_configuration_object_with_no_logger_set : ArrangeActAssert
    {
        protected LoggingExceptionHandlerData data;

        protected override void Arrange()
        {
            Logger.Reset();

            this.data =
                new LoggingExceptionHandlerData
                {
                    EventId = 100,
                    FormatterTypeName = typeof(TextExceptionFormatter).AssemblyQualifiedName,
                    LogCategory = "foo",
                    Priority = 200,
                    Severity = TraceEventType.Error
                };
        }

        [TestClass]
        public class when_creating_handler : given_valid_configuration_object_with_no_logger_set
        {
            InvalidOperationException exception;

            protected override void Act()
            {
                try
                {
                    this.data.BuildExceptionHandler();
                    Assert.Fail("should have thrown");
                }
                catch (InvalidOperationException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_throws()
            {
                Assert.IsNotNull(this.exception);
            }
        }
    }

    public class given_log_writer_set : ArrangeActAssert
    {
        protected MockTraceListener traceListener;

        protected override void Arrange()
        {
            this.traceListener = new MockTraceListener();
            var logWriter =
                new LogWriter(
                    new ILogFilter[0],
                    new LogSource[0],
                    new LogSource("all", new TraceListener[] { this.traceListener }, SourceLevels.All),
                    new LogSource(""),
                    new LogSource(""),
                    "default",
                    false,
                    false);

            Logger.SetLogWriter(logWriter, false);
        }

        protected override void Teardown()
        {
            Logger.Reset();
        }

        [TestClass]
        public class when_creating_handler_with_valid_configuration_object : given_log_writer_set
        {
            private LoggingExceptionHandler handler;

            protected override void Act()
            {
                var data =
                    new LoggingExceptionHandlerData
                    {
                        EventId = 100,
                        FormatterTypeName = typeof(TestFormatter).AssemblyQualifiedName,
                        LogCategory = "foo",
                        Priority = 200,
                        Severity = TraceEventType.Error
                    };

                this.handler = (LoggingExceptionHandler)data.BuildExceptionHandler();
            }

            [TestMethod]
            public void then_creates_handler()
            {
                Assert.IsNotNull(this.handler);
            }

            [TestMethod]
            public void then_handler_has_specified_formatter()
            {
                var exception = new Exception();
                this.handler.HandleException(exception, Guid.NewGuid());

                Assert.AreSame(exception, TestFormatter.LastException);
            }
        }

        [TestClass]
        public class when_creating_handler_with_configuration_object_with_empty_formatter : given_log_writer_set
        {
            private ConfigurationErrorsException exception;

            protected override void Act()
            {
                var data =
                    new LoggingExceptionHandlerData
                    {
                        EventId = 100,
                        FormatterTypeName = "",
                        LogCategory = "foo",
                        Priority = 200,
                        Severity = TraceEventType.Error
                    };

                try
                {
                    data.BuildExceptionHandler();
                }
                catch (ConfigurationErrorsException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_throws()
            {
                Assert.IsNotNull(this.exception);
            }
        }

        [TestClass]
        public class when_creating_handler_with_configuration_object_with_invalid_formatter : given_log_writer_set
        {
            private ConfigurationErrorsException exception;

            protected override void Act()
            {
                var data =
                    new LoggingExceptionHandlerData
                    {
                        EventId = 100,
                        FormatterTypeName = "not a type name",
                        LogCategory = "foo",
                        Priority = 200,
                        Severity = TraceEventType.Error
                    };

                try
                {
                    data.BuildExceptionHandler();
                }
                catch (ConfigurationErrorsException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_throws()
            {
                Assert.IsNotNull(this.exception);
            }
        }

        [TestClass]
        public class when_creating_handler_with_configuration_object_with_non_formatter : given_log_writer_set
        {
            private ConfigurationErrorsException exception;

            protected override void Act()
            {
                var data =
                    new LoggingExceptionHandlerData
                    {
                        EventId = 100,
                        FormatterTypeName = typeof(Uri).AssemblyQualifiedName,
                        LogCategory = "foo",
                        Priority = 200,
                        Severity = TraceEventType.Error
                    };

                try
                {
                    data.BuildExceptionHandler();
                }
                catch (ConfigurationErrorsException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_throws()
            {
                Assert.IsNotNull(this.exception);
            }
        }

        public class TestFormatter : ExceptionFormatter
        {
            public static Exception LastException;

            public TestFormatter(TextWriter ignored, Exception exception, Guid guid)
                : base(exception, guid)
            {
                LastException = exception;
            }

            protected override void WriteDescription()
            {
            }

            protected override void WriteDateTime(DateTime utcNow)
            {
            }

            protected override void WriteExceptionType(Type exceptionType)
            {
            }

            protected override void WriteMessage(string message)
            {
            }

            protected override void WriteSource(string source)
            {
            }

            protected override void WriteHelpLink(string helpLink)
            {
            }

            protected override void WriteStackTrace(string stackTrace)
            {
            }

            protected override void WritePropertyInfo(System.Reflection.PropertyInfo propertyInfo, object value)
            {
            }

            protected override void WriteFieldInfo(System.Reflection.FieldInfo fieldInfo, object value)
            {
            }

            protected override void WriteAdditionalInfo(System.Collections.Specialized.NameValueCollection additionalInformation)
            {
            }
        }
    }
}
