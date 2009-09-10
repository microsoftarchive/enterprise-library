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
    /// Extension methods to support configuration of <see cref="RollingFlatFileTraceListener"/>.
    /// </summary>
    public static class SendToRollingFileTraceListenerExtensions
    {
        /// <summary>
        /// Adds a new <see cref="RollingFlatFileTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="RollingFlatFileTraceListener"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RollingFlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="RollingFlatFileTraceListenerData"/>
        public static ILoggingConfigurationSendToRollingFileTraceListener RollingFile(this ILoggingConfigurationSendTo context, string listenerName)
        {
            if (string.IsNullOrEmpty(listenerName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "listenerName");

            return new SendToRollingFileTraceListenerBuilder(context, listenerName);
        }

        private class SendToRollingFileTraceListenerBuilder : SendToTraceListenerExtension, ILoggingConfigurationSendToRollingFileTraceListener
        {
            RollingFlatFileTraceListenerData rollingTraceListenerData;
            public SendToRollingFileTraceListenerBuilder(ILoggingConfigurationSendTo context, string listenerName)
                :base(context)
            {
                rollingTraceListenerData = new RollingFlatFileTraceListenerData()
                {
                    Name = listenerName
                };

                base.AddTraceListenerToSettingsAndCategory(rollingTraceListenerData);
            }

            public ILoggingConfigurationSendToRollingFileTraceListener RollEvery(RollInterval interval)
            {
                rollingTraceListenerData.RollInterval = interval;
                
                return this;
            }

            public ILoggingConfigurationSendToRollingFileTraceListener WhenRollFileExists(RollFileExistsBehavior behavior)
            {
                rollingTraceListenerData.RollFileExistsBehavior = behavior;

                return this;
            }

            public ILoggingConfigurationSendToRollingFileTraceListener RollAfterSize(int rollSizeInKB)
            {
                rollingTraceListenerData.RollSizeKB = rollSizeInKB;

                return this;
            }

            public ILoggingConfigurationSendToRollingFileTraceListener UseTimeStampPattern(string timeStampPattern)
            {
                rollingTraceListenerData.TimeStampPattern = timeStampPattern;

                return this;
            }

            public ILoggingConfigurationSendToRollingFileTraceListener WithFooter(string footer)
            {
                rollingTraceListenerData.Footer = footer;

                return this;
            }

            public ILoggingConfigurationSendToRollingFileTraceListener WithHeader(string header)
            {
                rollingTraceListenerData.Header = header;

                return this;
            }

            public ILoggingConfigurationSendToRollingFileTraceListener ToFile(string filename)
            {
                rollingTraceListenerData.FileName = filename;

                return this;
            }

            public ILoggingConfigurationSendToRollingFileTraceListener FormatWith(IFormatterBuilder formatBuilder)
            {
                if (formatBuilder == null) throw new ArgumentNullException("formatBuilder");

                FormatterData formatter = formatBuilder.GetFormatterData();
                rollingTraceListenerData.Formatter = formatter.Name;
                LoggingSettings.Formatters.Add(formatter);

                return this;
            }

            public ILoggingConfigurationSendToRollingFileTraceListener FormatWithSharedFormatter(string formatterName)
            {
                rollingTraceListenerData.Formatter = formatterName;

                return this;
            }

            public ILoggingConfigurationSendToRollingFileTraceListener Filter(SourceLevels sourceLevel)
            {
                rollingTraceListenerData.Filter = sourceLevel;

                return this;
            }

            public ILoggingConfigurationSendToRollingFileTraceListener WithTraceOptions(TraceOptions traceOptions)
            {
                rollingTraceListenerData.TraceOutputOptions = traceOptions;

                return this;
            }

            public ILoggingConfigurationSendToRollingFileTraceListener CleanUpArchivedFilesWhenMoreThan(int maximumArchivedFiles)
            {
                rollingTraceListenerData.MaxArchivedFiles = maximumArchivedFiles;

                return this;
            }
        }

    }
}
