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
using System.Globalization;
using System.Messaging;
using System.Runtime.Serialization;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
    /// <summary>
    /// Receive new log messages from MSMQ and distribute each log entry.
    /// </summary>
    public class MsmqLogDistributor
    {
        private bool isCompleted = true;
        private bool stopReceiving = false;

        private string msmqPath;

        private DistributorEventLogger eventLogger;

        /// <summary>
        /// Setup the queue and the formatter of the messages.
        /// </summary>
        public MsmqLogDistributor(string msmqPath, DistributorEventLogger eventLogger)
        {
            this.msmqPath = msmqPath;
            this.eventLogger = eventLogger;
        }

        /// <summary>
        /// Read-only property to check if the synchronous receive is completed.
        /// </summary>
        public virtual bool IsCompleted
        {
            get { return this.isCompleted; }
        }

        /// <summary>
        /// Instructs the listener to stop receiving messages.
        /// </summary>
        public virtual bool StopReceiving
        {
            get { return this.stopReceiving; }
            set { this.stopReceiving = value; }
        }

        /// <summary>
        /// Start receiving the message(s) from the queue.
        /// The messages will be taken from the queue until the queue is empty.
        /// This method is triggered every x seconds. (x is defined in application configuration file)
        /// </summary>
        public virtual void CheckForMessages()
        {
            try
            {
                ReceiveQueuedMessages();
            }
            catch (MessageQueueException qex)
            {
                string errorMsg = LogMessageQueueException(qex.MessageQueueErrorCode, qex);
                throw new LoggingException(errorMsg, qex);
            }
            catch (LoggingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string errorMsg = string.Format(CultureInfo.CurrentCulture, Resources.MsmqReceiveGeneralError, msmqPath);
                this.eventLogger.LogServiceFailure(
                    errorMsg,
                    ex,
                    TraceEventType.Error);

                throw new LoggingException(errorMsg, ex);
            }
            finally
            {
                this.isCompleted = true;
            }
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="e">The exception, or null.</param>
        /// <returns>The logged message.</returns>
        protected string LogMessageQueueException(MessageQueueErrorCode code, Exception e)
        {
            TraceEventType logType = TraceEventType.Error;
            string errorMsg = string.Empty;

            if (code == MessageQueueErrorCode.TransactionUsage)
            {
                errorMsg = string.Format(CultureInfo.CurrentCulture, Resources.MsmqInvalidTransactionUsage, msmqPath);
            }
            else if (code == MessageQueueErrorCode.IOTimeout)
            {
                errorMsg = string.Format(CultureInfo.CurrentCulture, Resources.MsmqReceiveTimeout, msmqPath);
                logType = TraceEventType.Warning;
            }
            else if (code == MessageQueueErrorCode.AccessDenied)
            {
                errorMsg = string.Format(CultureInfo.CurrentCulture, Resources.MsmqAccessDenied, msmqPath, WindowsIdentity.GetCurrent().Name);
            }
            else
            {
                errorMsg = string.Format(CultureInfo.CurrentCulture, Resources.MsmqReceiveError, msmqPath);
            }

            this.eventLogger.LogServiceFailure(
                errorMsg,
                e,
                logType);

            return errorMsg;
        }

        private MessageQueue CreateMessageQueue()
        {
            MessageQueue messageQueue = new MessageQueue(msmqPath, false, true);
            ((XmlMessageFormatter)messageQueue.Formatter).TargetTypeNames = new string[] { "System.String" };
            return messageQueue;
        }

        private bool IsQueueEmpty()
        {
            bool empty = false;
            try
            {
                using (MessageQueue msmq = CreateMessageQueue())
                {
                    msmq.Peek(new TimeSpan(0));
                }
            }
            catch (MessageQueueException e)
            {
                if (e.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                {
                    empty = true;
                }
            }

            return empty;
        }

        private void ReceiveQueuedMessages()
        {
            this.isCompleted = false;
            while (!IsQueueEmpty())
            {
                using (MessageQueue msmq = CreateMessageQueue())
                {
                    Message message = msmq.Peek();

                    string serializedEntry = message.Body.ToString();

                    LogEntry logEntry = null;
                    try
                    {
                        logEntry = BinaryLogFormatter.Deserialize(serializedEntry);
                    }
                    catch (FormatException formatException)
                    {
                        string logMessage = string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.ExceptionCouldNotDeserializeMessageFromQueue,
                            message.Id,
                            msmq.Path);

                        this.eventLogger.LogServiceFailure(
                            logMessage,
                            formatException,
                            TraceEventType.Error);
                    }
                    catch (SerializationException serializationException)
                    {
                        string logMessage = string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.ExceptionCouldNotDeserializeMessageFromQueue,
                            message.Id,
                            msmq.Path);

                        this.eventLogger.LogServiceFailure(
                            logMessage,
                            serializationException,
                            TraceEventType.Error);
                    }

                    if (logEntry != null)
                    {
                        Logger.Write(logEntry);
                    }

                    message = msmq.Receive();

                    if (this.StopReceiving)
                    {
                        this.isCompleted = true;
                        return;
                    }
                }
            }
        }
    }
}
