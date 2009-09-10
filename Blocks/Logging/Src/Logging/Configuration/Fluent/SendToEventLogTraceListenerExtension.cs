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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Diagnostics;
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Extension methods to support configuration of <see cref="FormattedEventLogTraceListener"/>.
    /// </summary>
    public static class SendToFormattedEventLogTraceListenerExtension
    {
        /// <summary>
        /// Adds a new <see cref="FormattedEventLogTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="FormattedEventLogTraceListener"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FormattedEventLogTraceListenerData"/>. </returns>
        /// <seealso cref="FormattedEventLogTraceListenerData"/>
        public static ILoggingConfigurationSendToEventLogTraceListener EventLog(this ILoggingConfigurationSendTo context, string listenerName)
        {
            if (string.IsNullOrEmpty(listenerName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "listenerName");

            return new SendToFormattedEventLogTraceListenerBuilder(context, listenerName);
        }

        private class SendToFormattedEventLogTraceListenerBuilder : SendToTraceListenerExtension, ILoggingConfigurationSendToEventLogTraceListener
        {
            FormattedEventLogTraceListenerData eventLogListener;

            public SendToFormattedEventLogTraceListenerBuilder(ILoggingConfigurationSendTo context, string listenerName)
                :base(context)
            {
                eventLogListener = new FormattedEventLogTraceListenerData
                {
                    Name = listenerName
                };

                base.AddTraceListenerToSettingsAndCategory(eventLogListener);
            }

            public ILoggingConfigurationSendToEventLogTraceListener ToLog(string logName)
            {
                eventLogListener.Log = logName;

                return this;
            }

            public ILoggingConfigurationSendToEventLogTraceListener ToMachine(string machineName)
            {
                eventLogListener.MachineName = machineName;

                return this;
            }

            public ILoggingConfigurationSendToEventLogTraceListener FormatWith(IFormatterBuilder formatBuilder)
            {
                if (formatBuilder == null) throw new ArgumentNullException("formatBuilder");

                FormatterData formatter = formatBuilder.GetFormatterData();
                eventLogListener.Formatter = formatter.Name;
                LoggingSettings.Formatters.Add(formatter);

                return this;
            }

            public ILoggingConfigurationSendToEventLogTraceListener FormatWithSharedFormatter(string formatterName)
            {
                eventLogListener.Formatter = formatterName;

                return this;
            }

            public ILoggingConfigurationSendToEventLogTraceListener UsingEventLogSource(string source)
            {
                if (String.IsNullOrEmpty(source)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "source");

                eventLogListener.Source = source;

                return this;
            }

            public ILoggingConfigurationSendToEventLogTraceListener Filter(SourceLevels sourceLevel)
            {
                eventLogListener.Filter = sourceLevel;

                return this;
            }

            public ILoggingConfigurationSendToEventLogTraceListener WithTraceOptions(TraceOptions traceOptions)
            {
                eventLogListener.TraceOutputOptions = traceOptions;

                return this;
            }
        }
    }
}
