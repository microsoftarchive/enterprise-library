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
using System.Diagnostics;
using System.Messaging;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to specify settings on a <see cref="MsmqTraceListenerData"/>.
    /// </summary>
    /// <seealso cref="MsmqTraceListenerData"/>
    [SecurityCritical]
    public interface ILoggingConfigurationSendToMsmqTraceListener : ILoggingConfigurationContd, ILoggingConfigurationCategoryContd, IFluentInterface
    {
        /// <summary>
        /// Specifies the <see cref="MessageQueueTransactionType"/> that should be used when sending messages by this <see cref="MsmqTraceListener"/>.<br/>
        /// The default is <see cref="System.Messaging.Message.InfiniteTimeout"/>.
        /// </summary>
        /// <param name="TransactionType">The <see cref="MessageQueueTransactionType"/> that should be used.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="MsmqTraceListenerData"/>. </returns>
        /// <seealso cref="MsmqTraceListener"/>
        /// <seealso cref="MsmqTraceListenerData"/>
        ILoggingConfigurationSendToMsmqTraceListener WithTransactionType(MessageQueueTransactionType TransactionType);

        /// <summary>
        /// Specifies the maximum time for messages to reach the queue for this <see cref="MsmqTraceListener"/>.<br/>
        /// The default is <see cref="System.Messaging.Message.InfiniteTimeout"/>.
        /// </summary>
        /// <param name="maximumTimeToReachQueue">The maximum time for messages to reach the queue.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="MsmqTraceListenerData"/>. </returns>
        /// <seealso cref="MsmqTraceListener"/>
        /// <seealso cref="MsmqTraceListenerData"/>
        ILoggingConfigurationSendToMsmqTraceListener SetTimeToReachQueue(TimeSpan maximumTimeToReachQueue);

        /// <summary>
        /// Specifies the maximum time to be received for this <see cref="MsmqTraceListener"/>. <br/>
        /// The default is 
        /// </summary>
        /// <param name="maximumTimeToBeReceived">The maximum time to be received.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="MsmqTraceListenerData"/>. </returns>
        /// <seealso cref="MsmqTraceListener"/>
        /// <seealso cref="MsmqTraceListenerData"/>
        ILoggingConfigurationSendToMsmqTraceListener SetTimeToBeReceived(TimeSpan maximumTimeToBeReceived);

        /// <summary>
        ///
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="MsmqTraceListenerData"/>. </returns>
        /// <seealso cref="MsmqTraceListener"/>
        /// <seealso cref="MsmqTraceListenerData"/>
        ILoggingConfigurationSendToMsmqTraceListener AsRecoverable();

        /// <summary>
        /// Specifies the queue that should be used by this <see cref="MsmqTraceListener"/>.
        /// </summary>
        /// <param name="queuePath">The queue path that should be used.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="MsmqTraceListenerData"/>. </returns>
        /// <seealso cref="MsmqTraceListener"/>
        /// <seealso cref="MsmqTraceListenerData"/>
        ILoggingConfigurationSendToMsmqTraceListener UseQueue(string queuePath);

        /// <summary>
        /// TODOC: review
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="MsmqTraceListenerData"/>. </returns>
        /// <seealso cref="MsmqTraceListener"/>
        /// <seealso cref="MsmqTraceListenerData"/>
        ILoggingConfigurationSendToMsmqTraceListener UseDeadLetterQueue();

        /// <summary>
        /// Specifies that messages send to Msmq by this <see cref="MsmqTraceListener"/> should be encrypted.
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="MsmqTraceListenerData"/>. </returns>
        /// <seealso cref="MsmqTraceListener"/>
        /// <seealso cref="MsmqTraceListenerData"/>
        ILoggingConfigurationSendToMsmqTraceListener UseEncryption();

        /// <summary>
        /// Specifies that authentication should be used when sending messages to Msmq by this <see cref="MsmqTraceListener"/>.
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="MsmqTraceListenerData"/>. </returns>
        /// <seealso cref="MsmqTraceListener"/>
        /// <seealso cref="MsmqTraceListenerData"/>
        ILoggingConfigurationSendToMsmqTraceListener UseAuthentication();

        /// <summary>
        /// Specifies the formatter used to format log messages send by this <see cref="MsmqTraceListener"/>.<br/>
        /// </summary>
        /// <param name="formatBuilder">The <see cref="FormatterBuilder"/> used to create an <see cref="LogFormatter"/> .</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="MsmqTraceListenerData"/>. </returns>
        /// <seealso cref="MsmqTraceListener"/>
        /// <seealso cref="MsmqTraceListenerData"/>
        ILoggingConfigurationSendToMsmqTraceListener FormatWith(IFormatterBuilder formatBuilder);

        /// <summary>
        /// Specifies the formatter used to format log messages send by this <see cref="MsmqTraceListener"/>.<br/>
        /// </summary>
        /// <param name="formatterName">The name of a <see cref="FormatterData"/> configured elsewhere in this section.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="MsmqTraceListenerData"/>. </returns>
        /// <seealso cref="MsmqTraceListener"/>
        /// <seealso cref="MsmqTraceListenerData"/>
        ILoggingConfigurationSendToMsmqTraceListener FormatWithSharedFormatter(string formatterName);

        /// <summary>
        /// Specifies the <see cref="SourceLevels"/> that should be used to filter trace output by this <see cref="MsmqTraceListener"/>.
        /// </summary>
        /// <param name="sourceLevel">The <see cref="SourceLevels"/> that should be used to filter trace output .</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="MsmqTraceListenerData"/>. </returns>
        /// <seealso cref="MsmqTraceListener"/>
        /// <seealso cref="MsmqTraceListenerData"/>
        /// <seealso cref="SourceLevels"/>
        ILoggingConfigurationSendToMsmqTraceListener Filter(SourceLevels sourceLevel);

        /// <summary>
        /// Specifies which options, or elements, should be included in messages send by this <see cref="MsmqTraceListener"/>.<br/>
        /// </summary>
        /// <param name="traceOptions">The options that should be included in the trace output.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="MsmqTraceListenerData"/>. </returns>
        /// <seealso cref="MsmqTraceListener"/>
        /// <seealso cref="MsmqTraceListenerData"/>
        /// <seealso cref="TraceOptions"/>
        ILoggingConfigurationSendToMsmqTraceListener WithTraceOptions(TraceOptions traceOptions);

        /// <summary>
        /// Specifies the <see cref="MessagePriority"/> that will be used to send messages to msmq by this <see cref="MsmqTraceListener"/>.<br/>
        /// The default priority is <see cref="MessagePriority.Normal"/>
        /// </summary>
        /// <param name="priority">The <see cref="MessagePriority"/> that will be used to send messages to msmq.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="MsmqTraceListenerData"/>. </returns>
        /// <seealso cref="MsmqTraceListener"/>
        /// <seealso cref="MsmqTraceListenerData"/>
        /// <seealso cref="MessagePriority"/>
        ILoggingConfigurationSendToMsmqTraceListener Prioritize(MessagePriority priority);
    }
}
