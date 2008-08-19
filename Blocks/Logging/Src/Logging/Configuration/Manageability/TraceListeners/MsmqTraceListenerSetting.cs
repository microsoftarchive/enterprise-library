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
using System.Configuration;
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    /// <summary>
    /// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.MsmqTraceListenerData"/>
    /// as an instrumentation class.
    /// </summary>
    [ManagementEntity]
    public partial class MsmqTraceListenerSetting : TraceListenerSetting
    {
        string formatter;
        string messagePriority;
        string queuePath;
        bool recoverable;
        string timeToBeReceived;
        string timeToReachQueue;
        string transactionType;
        bool useAuthentication;
        bool useDeadLetterQueue;
        bool useEncryption;

        /// <summary>
        /// Initialize a new instance of the <see cref="MsmqTraceListenerSetting"/> class.
        /// </summary>
        /// <param name="sourceElement">The configuration source element.</param>
        /// <param name="name">The name of the trace listener.</param>
        /// <param name="formatter">The formatter to use.</param>
        /// <param name="messagePriority">The message priority.</param>
        /// <param name="queuePath">The queue path.</param>
        /// <param name="recoverable">true if the queue is recoverable; otherwise, false.</param>
        /// <param name="timeToBeReceived">The time to be received.</param>
        /// <param name="timeToReachQueue">The time to reach the queue.</param>
        /// <param name="traceOutputOptions">The trace output options.</param>
        /// <param name="transactionType">The queue transaction type.</param>
        /// <param name="useAuthentication">true to use authentication; otherwise false.</param>
        /// <param name="useDeadLetterQueue">true to use the dead letter queue; otherwise false.</param>
        /// <param name="useEncryption">true to use encryption; otherwise false.</param>
		/// <param name="filter">The filter value.</param>
		public MsmqTraceListenerSetting(MsmqTraceListenerData sourceElement,
                                          string name,
                                          string formatter,
                                          string messagePriority,
                                          string queuePath,
                                          bool recoverable,
                                          string timeToBeReceived,
                                          string timeToReachQueue,
                                          string traceOutputOptions,
                                          string transactionType,
                                          bool useAuthentication,
                                          bool useDeadLetterQueue,
                                          bool useEncryption,
										  string filter)
            : base(sourceElement, name, traceOutputOptions, filter)
        {
            this.formatter = formatter;
            this.messagePriority = messagePriority;
            this.queuePath = queuePath;
            this.recoverable = recoverable;
            this.timeToBeReceived = timeToBeReceived;
            this.timeToReachQueue = timeToReachQueue;
            this.transactionType = transactionType;
            this.useAuthentication = useAuthentication;
            this.useDeadLetterQueue = useDeadLetterQueue;
            this.useEncryption = useEncryption;
        }

        /// <summary>
        /// Gets the name of the formatter for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string Formatter
        {
            get { return formatter; }
            set { formatter = value; }
        }

        /// <summary>
        /// Gets the name of value of the message priority for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string MessagePriority
        {
            get { return messagePriority; }
            set { messagePriority = value; }
        }

        /// <summary>
        /// Gets the queue path for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string QueuePath
        {
            get { return queuePath; }
            set { queuePath = value; }
        }

        /// <summary>
        /// Gets the value of the recoverable property for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public bool Recoverable
        {
            get { return recoverable; }
            set { recoverable = value; }
        }

        /// <summary>
        /// Gets the string representation of the <see cref="TimeSpan"/> value of the time to be received 
        /// for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string TimeToBeReceived
        {
            get { return timeToBeReceived; }
            set { timeToBeReceived = value; }
        }

        /// <summary>
        /// Gets the string representation of the <see cref="TimeSpan"/> value of the time to reach queue
        /// for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string TimeToReachQueue
        {
            get { return timeToReachQueue; }
            set { timeToReachQueue = value; }
        }

        /// <summary>
        /// Gets the name of value of the transaction type for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }

        /// <summary>
        /// Gets the value of the use authentication property for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public bool UseAuthentication
        {
            get { return useAuthentication; }
            set { useAuthentication = value; }
        }

        /// <summary>
        /// Gets the value of the use dead letter property for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public bool UseDeadLetterQueue
        {
            get { return useDeadLetterQueue; }
            set { useDeadLetterQueue = value; }
        }

        /// <summary>
        /// Gets the value of the use encryption property for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public bool UseEncryption
        {
            get { return useEncryption; }
            set { useEncryption = value; }
        }

        /// <summary>
        /// Returns the <see cref="MsmqTraceListenerSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="MsmqTraceListenerSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static MsmqTraceListenerSetting BindInstance(string ApplicationName,
                                                            string SectionName,
                                                            string Name)
        {
            return BindInstance<MsmqTraceListenerSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="MsmqTraceListenerSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<MsmqTraceListenerSetting> GetInstances()
        {
            return GetInstances<MsmqTraceListenerSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="MsmqTraceListenerSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return MsmqTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}