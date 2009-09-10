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
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// Is a <see cref="TraceListener"/> that delivers the log entries to an Msmq instance.
    /// </summary>
    [ConfigurationElementType(typeof(MsmqTraceListenerData))]
    public class MsmqTraceListener : FormattedTraceListenerBase
    {
        readonly MessagePriority messagePriority;
        readonly IMsmqSendInterfaceFactory msmqInterfaceFactory;
        readonly string queuePath;
        readonly bool recoverable;
        readonly TimeSpan timeToBeReceived;
        readonly TimeSpan timeToReachQueue;
        readonly MessageQueueTransactionType transactionType;
        readonly bool useAuthentication;
        readonly bool useDeadLetterQueue;
        readonly bool useEncryption;

        /// <summary>
        /// Initializes a new instance of <see cref="MsmqTraceListener"/>.
        /// </summary>
        /// <param name="name">The name of the new instance.</param>
        /// <param name="queuePath">The path to the queue to deliver to.</param>
        /// <param name="formatter">The formatter to use.</param>
        /// <param name="messagePriority">The priority for the messages to send.</param>
        /// <param name="recoverable">The recoverable flag for the messages to send.</param>
        /// <param name="timeToReachQueue">The timeToReachQueue for the messages to send.</param>
        /// <param name="timeToBeReceived">The timeToBeReceived for the messages to send.</param>
        /// <param name="useAuthentication">The useAuthentication flag for the messages to send.</param>
        /// <param name="useDeadLetterQueue">The useDeadLetterQueue flag for the messages to send.</param>
        /// <param name="useEncryption">The useEncryption flag for the messages to send.</param>
        /// <param name="transactionType">The <see cref="MessageQueueTransactionType"/> for the message to send.</param>
        public MsmqTraceListener(string name,
                                 string queuePath,
                                 ILogFormatter formatter,
                                 MessagePriority messagePriority,
                                 bool recoverable,
                                 TimeSpan timeToReachQueue,
                                 TimeSpan timeToBeReceived,
                                 bool useAuthentication,
                                 bool useDeadLetterQueue,
                                 bool useEncryption,
                                 MessageQueueTransactionType transactionType)
            : this(name, queuePath, formatter, messagePriority, recoverable,
                   timeToReachQueue, timeToBeReceived,
                   useAuthentication, useDeadLetterQueue, useEncryption,
                   transactionType, new MsmqSendInterfaceFactory())
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="MsmqTraceListener"/>.
        /// </summary>
        /// <param name="name">The name of the new instance.</param>
        /// <param name="queuePath">The path to the queue to deliver to.</param>
        /// <param name="formatter">The formatter to use.</param>
        /// <param name="messagePriority">The priority for the messages to send.</param>
        /// <param name="recoverable">The recoverable flag for the messages to send.</param>
        /// <param name="timeToReachQueue">The timeToReachQueue for the messages to send.</param>
        /// <param name="timeToBeReceived">The timeToBeReceived for the messages to send.</param>
        /// <param name="useAuthentication">The useAuthentication flag for the messages to send.</param>
        /// <param name="useDeadLetterQueue">The useDeadLetterQueue flag for the messages to send.</param>
        /// <param name="useEncryption">The useEncryption flag for the messages to send.</param>
        /// <param name="transactionType">The <see cref="MessageQueueTransactionType"/> for the message to send.</param>
        /// <param name="msmqInterfaceFactory">The factory to create the msmq interfaces.</param>
        public MsmqTraceListener(string name,
                                 string queuePath,
                                 ILogFormatter formatter,
                                 MessagePriority messagePriority,
                                 bool recoverable,
                                 TimeSpan timeToReachQueue,
                                 TimeSpan timeToBeReceived,
                                 bool useAuthentication,
                                 bool useDeadLetterQueue,
                                 bool useEncryption,
                                 MessageQueueTransactionType transactionType,
                                 IMsmqSendInterfaceFactory msmqInterfaceFactory)
            : base(formatter)
        {
            this.queuePath = queuePath;
            this.messagePriority = messagePriority;
            this.recoverable = recoverable;
            this.timeToReachQueue = timeToReachQueue;
            this.timeToBeReceived = timeToBeReceived;
            this.useAuthentication = useAuthentication;
            this.useDeadLetterQueue = useDeadLetterQueue;
            this.useEncryption = useEncryption;
            this.transactionType = transactionType;
            this.msmqInterfaceFactory = msmqInterfaceFactory;
        }

        /// <summary>
        /// Gets the path to the queue.
        /// </summary>
        /// <value>
        /// The path to the queue.
        /// </value>
        public string QueuePath
        {
            get { return queuePath; }
        }

        /// <summary>
        /// Create a message from a <see cref="LogEntry"/>.
        /// </summary>
        /// <param name="logEntry">The <see cref="LogEntry"/></param>
        /// <returns>A <see cref="Message"/> object.</returns>
        public Message CreateMessage(LogEntry logEntry)
        {
            string formattedLogEntry = FormatEntry(logEntry);

            return CreateMessage(formattedLogEntry, logEntry.Title);
        }

        Message CreateMessage(string messageBody,
                              string messageLabel)
        {
            Message queueMessage = new Message();

            queueMessage.Body = messageBody;
            queueMessage.Label = messageLabel;
            queueMessage.Priority = messagePriority;
            queueMessage.TimeToBeReceived = timeToBeReceived;
            queueMessage.TimeToReachQueue = timeToReachQueue;
            queueMessage.Recoverable = recoverable;
            queueMessage.UseAuthentication = useAuthentication;
            queueMessage.UseDeadLetterQueue = useDeadLetterQueue;
            queueMessage.UseEncryption = useEncryption;

            return queueMessage;
        }

        string FormatEntry(LogEntry entry)
        {
            // Initialize all intrinsic properties
            entry.CollectIntrinsicProperties();

            string formattedMessage = Formatter.Format(entry);

            return formattedMessage;
        }

        void SendMessageToQueue(string message)
        {
            using (IMsmqSendInterface messageQueueInterface = msmqInterfaceFactory.CreateMsmqInterface(queuePath))
            {
                using (Message queueMessage = CreateMessage(message, string.Empty))
                {
                    messageQueueInterface.Send(queueMessage, transactionType);
                    messageQueueInterface.Close();
                }
            }
        }

        void SendMessageToQueue(LogEntry logEntry)
        {
            using (IMsmqSendInterface messageQueueInterface = msmqInterfaceFactory.CreateMsmqInterface(queuePath))
            {
                using (Message queueMessage = CreateMessage(logEntry))
                {
                    messageQueueInterface.Send(queueMessage, transactionType);
                    messageQueueInterface.Close();
                }
            }
        }

        /// <summary>
        /// Sends the traced object to its final destination through a <see cref="MessageQueue"/>.
        /// </summary>
        /// <param name="eventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The id of the event.</param>
        /// <param name="data">The data to trace.</param>
        public override void TraceData(TraceEventCache eventCache,
                                       string source,
                                       TraceEventType eventType,
                                       int id,
                                       object data)
        {
            if ((Filter == null) || Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                if (data is LogEntry)
                {
                    SendMessageToQueue(data as LogEntry);
                }
                else if (data is string)
                {
                    Write(data as string);
                }
                else
                {
                    base.TraceData(eventCache, source, eventType, id, data);
                }
            }
        }

        /// <summary>
        /// Writes the specified message to the message queue.
        /// </summary>
        /// <param name="message">Message to be written.</param>
        public override void Write(string message)
        {
            SendMessageToQueue(message);
        }

        /// <summary>
        /// Writes the specified message to the message queue.
        /// </summary>
        /// <param name="message"></param>
        public override void WriteLine(string message)
        {
            Write(message);
        }
    }
}
