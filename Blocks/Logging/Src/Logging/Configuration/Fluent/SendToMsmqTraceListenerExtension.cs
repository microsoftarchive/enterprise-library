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
    /// Extension methods to support configuration of <see cref="MsmqTraceListener"/>.
    /// </summary>
    public static class SendToMsmqTraceListenerExtensions
    {        
        /// <summary>
        /// Adds a new <see cref="MsmqTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="MsmqTraceListener"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="MsmqTraceListenerData"/>. </returns>
        /// <seealso cref="MsmqTraceListenerData"/>
        public static ILoggingConfigurationSendToMsmqTraceListener Msmq(this ILoggingConfigurationSendTo context, string listenerName)
        {
            if (string.IsNullOrEmpty(listenerName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "listenerName");

            return new SendToMsmqTraceListenerBuilder(context, listenerName);
        }

        private class SendToMsmqTraceListenerBuilder : SendToTraceListenerExtension, ILoggingConfigurationSendToMsmqTraceListener
        {
            MsmqTraceListenerData msmqTraceListener;
            public SendToMsmqTraceListenerBuilder(ILoggingConfigurationSendTo context, string listenerName)
                :base(context)
            {
                msmqTraceListener = new MsmqTraceListenerData
                {
                    Name = listenerName
                };

                base.AddTraceListenerToSettingsAndCategory(msmqTraceListener);
            }

            public ILoggingConfigurationSendToMsmqTraceListener WithTransactionType(MessageQueueTransactionType TransactionType)
            {
                msmqTraceListener.TransactionType = TransactionType;

                return this;
            }

            public ILoggingConfigurationSendToMsmqTraceListener SetTimeToReachQueue(TimeSpan maximumTimeToReachQueue)
            {
                msmqTraceListener.TimeToReachQueue = maximumTimeToReachQueue;

                return this;
            }

            public ILoggingConfigurationSendToMsmqTraceListener SetTimeToBeReceived(TimeSpan maximumTimeToBeReceived)
            {
                msmqTraceListener.TimeToBeReceived = maximumTimeToBeReceived;

                return this;
            }

            public ILoggingConfigurationSendToMsmqTraceListener AsRecoverable()
            {
                msmqTraceListener.Recoverable = true;

                return this;
            }

            public ILoggingConfigurationSendToMsmqTraceListener UseDeadLetterQueue()
            {
                msmqTraceListener.UseDeadLetterQueue = true;

                return this;
            }

            public ILoggingConfigurationSendToMsmqTraceListener UseEncryption()
            {
                msmqTraceListener.UseEncryption = true;

                return this;
            }

            public ILoggingConfigurationSendToMsmqTraceListener UseAuthentication()
            {
                msmqTraceListener.UseAuthentication = true;

                return this;
            }

            public ILoggingConfigurationSendToMsmqTraceListener FormatWith(IFormatterBuilder formatBuilder)
            {
                if (formatBuilder == null) throw new ArgumentNullException("formatBuilder");

                FormatterData formatter = formatBuilder.GetFormatterData();
                msmqTraceListener.Formatter = formatter.Name;
                LoggingSettings.Formatters.Add(formatter);

                return this;
            }

            public ILoggingConfigurationSendToMsmqTraceListener FormatWithSharedFormatter(string formatterName)
            {
                if (string.IsNullOrEmpty(formatterName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "formatterName");

                msmqTraceListener.Formatter = formatterName;

                return this;
            }

            public ILoggingConfigurationSendToMsmqTraceListener Filter(SourceLevels sourceLevel)
            {
                msmqTraceListener.Filter = sourceLevel;

                return this;
            }

            public ILoggingConfigurationSendToMsmqTraceListener WithTraceOptions(TraceOptions traceOptions)
            {
                msmqTraceListener.TraceOutputOptions = traceOptions;

                return this;
            }

            public ILoggingConfigurationSendToMsmqTraceListener Prioritize(MessagePriority priority)
            {
                msmqTraceListener.MessagePriority = priority;
                
                return this;
            }


            public ILoggingConfigurationSendToMsmqTraceListener UseQueue(string queuePath)
            {
                msmqTraceListener.QueuePath = queuePath;

                return this;
            }
        }
    }

}
